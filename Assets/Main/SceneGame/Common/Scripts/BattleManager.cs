using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PawHunters
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance { get; private set; }

        [SerializeField] List<EntityUIPortrait> entityUIList;

        [ShowInInspector, ReadOnly] public int CurrentRound { get; set; }

        int MaxRound => SceneGameManager.Instance.LevelData.maxRound;
        TeamManager_GamePlayer TeamManager_GamePlayer => TeamManager_GamePlayer.Instance;
        TeamManager_GameEnemy TeamManager_GameEnemy => TeamManager_GameEnemy.Instance;

        public event Action<int> OnRoundCountUpdate;

        private void Awake() => Instance = this;

        public void Execute()
        {
            foreach (var entity in TeamManager_GamePlayer.AliveEntityList)
                entityUIList.Add(EntityUIPortraitSpawner.Instance.SpawnPlayer(entity));
            foreach (var entity in TeamManager_GameEnemy.AliveEntityList)
                entityUIList.Add(EntityUIPortraitSpawner.Instance.SpawnEnemy(entity));

            StartRound();
        }

        async void StartRound()
        {
            CurrentRound++;

            RoundUIManager.Instance.UpdateUI(CurrentRound);
            RearrangeEntities();

            await Task.Delay(500);
            await GameActionTriggersManager.Instance.ExecuteOnTrigger(GameActionTriggersManager.TriggerType.StartRound);
            await ExecuteSkills(GameActionTriggersManager.TriggerType.StartRound);
            if (CheckEndBattle())
                return;

            ExecuteInstantSkills();
        }

        async void ExecuteInstantSkills()
        {
            await ExecuteSkills(GameActionTriggersManager.TriggerType.Instant);
            EndRound();
        }

        async void EndRound()
        {
            await GameActionTriggersManager.Instance.ExecuteOnTrigger(GameActionTriggersManager.TriggerType.EndRound);
            await ExecuteSkills(GameActionTriggersManager.TriggerType.EndRound);
            if (CheckEndBattle())
                return;

            StartRound();
        }

        async Task ExecuteSkills(GameActionTriggersManager.TriggerType trigger)
        {
            foreach (var entity in entityUIList)
            {
                await entity.EntityMainController.EntitySkillsController.Execute(trigger);
                if (CheckEndBattle())
                    return;
            }
        }

        bool CheckEndBattle()
        {
            if (!TeamManager_GamePlayer.IsAlive || CurrentRound == MaxRound)
                SceneGameManager.Instance.JourneyFailed();
            else if (!TeamManager_GameEnemy.IsAlive)
                SceneGameManager.Instance.NextJourney();
            else
                return false;

            Clear();

            return true;
        }

        void RearrangeEntities()
        {
            entityUIList = entityUIList.OrderBy(x => x.EntityMainController.BattleAttributes.speed.Value).ToList();
            for (int i = 0; i < entityUIList.Count; i++)
                entityUIList[i].SetOrder(i);
        }

        void Clear()
        {
            CurrentRound = 0;
            entityUIList.Clear();
            RoundUIManager.Instance.UpdateUI(CurrentRound);
            EntityUIPortraitSpawner.Instance.Clear();
        }
    }
}
