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
        public enum State { None, StartRound, ExecuteInstantSkils, EndRound, JourneyFailed, NextJourney }

        public static BattleManager Instance { get; private set; }

        [SerializeField] List<EntityUIPortrait> entityUIList;
        [SerializeField] SkillData resetSpecialSkill;

        [ShowInInspector, ReadOnly] public int CurrentRound { get; set; }
        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();

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
            CurrentState.Value = State.StartRound;
            CurrentRound++;

            RoundUIManager.Instance.UpdateUI(CurrentRound);
            RearrangeEntities();

            await Task.Delay(500);
            await GameActionTriggersManager.Instance.ExecuteOnTrigger(GameActionTriggersManager.TriggerType.StartRound);
            if (CheckStopBattle())
                return;

            await ExecuteSkills(GameActionTriggersManager.TriggerType.StartRound);
            if (CheckStopBattle())
                return;

            ExecuteInstantSkills();
        }

        async void ExecuteInstantSkills()
        {
            CurrentState.Value = State.ExecuteInstantSkils;
            await ExecuteSkills(GameActionTriggersManager.TriggerType.Instant);
            if (CheckStopBattle())
                return;

            EndRound();
        }

        async void EndRound()
        {
            CurrentState.Value = State.EndRound;
            await GameActionTriggersManager.Instance.ExecuteOnTrigger(GameActionTriggersManager.TriggerType.EndRound);
            if (CheckStopBattle())
                return;

            await ExecuteSkills(GameActionTriggersManager.TriggerType.EndRound);
            if (CheckStopBattle())
                return;

            StartRound();
        }

        async Task ExecuteSkills(GameActionTriggersManager.TriggerType trigger)
        {
            foreach (var entity in entityUIList)
            {
                await entity.EntityMainController.EntitySkillsController.Execute(trigger);
                if (CheckStopBattle())
                    return;
            }
        }

        bool CheckStopBattle()
        {
            if (CurrentState.Value == State.None)
                return true;
            if (!TeamManager_GamePlayer.IsAlive || CurrentRound == MaxRound)
                CurrentState.Value = State.JourneyFailed;
            else if (!TeamManager_GameEnemy.IsAlive)
                CurrentState.Value = State.NextJourney;
            else
                return false;

            StopBattle();
            return true;
        }

        void StopBattle()
        {
            CurrentRound = 0;
            entityUIList.Clear();
            RoundUIManager.Instance.UpdateUI(CurrentRound);
            EntityUIPortraitSpawner.Instance.Clear();

            _ = GameActionTriggersManager.Instance.ExecuteOnTrigger(GameActionTriggersManager.TriggerType.StopBattle);
            _ = resetSpecialSkill.Execute();

            if (CurrentState.Value == State.JourneyFailed)
                SceneGameManager.Instance.JourneyFailed();
            else
                SceneGameManager.Instance.NextJourney();

            CurrentState.Value = State.None;
        }

        void RearrangeEntities()
        {
            entityUIList = entityUIList.OrderBy(x => x.EntityMainController.BattleAttributes.speed.Value).ToList();
            for (int i = 0; i < entityUIList.Count; i++)
                entityUIList[i].SetOrder(i);
        }
    }
}
