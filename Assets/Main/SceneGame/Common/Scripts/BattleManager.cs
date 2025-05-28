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

        [ShowInInspector, ReadOnly] int CurrentRound { get; set; }

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
            await Task.Delay(250);
            CurrentRound++;
            if (CheckEndBattle())
                return;

            RoundUIManager.Instance.UpdateUI(CurrentRound);
            RearrangeEntities();

            await GameActionTriggersManager.Instance.ExecuteOnTrigger(StatusEffectData.TriggerType.StartRound);
            if (CheckEndBattle())
                return;

            ExecuteSkills();
        }

        async void ExecuteSkills()
        {
            foreach (var entity in entityUIList)
            {
                await entity.EntityMainController.EntitySkillsController.Execute();
                if (CheckEndBattle())
                    return;
            }

            EndRound();
        }

        async void EndRound()
        {
            await GameActionTriggersManager.Instance.ExecuteOnTrigger(StatusEffectData.TriggerType.EndRound);
            if (CheckEndBattle())
                return;

            StartRound();
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
            foreach (var entityUI in entityUIList)
                entityUI.transform.SetAsLastSibling();
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
