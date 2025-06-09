using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace LabHaven.PawHunters
{
    public class BattleManager : SingletonMonoBehaviour<BattleManager>
    {
        public enum State { None, InitiateBattle, BeginRound, ExecuteInstantSkils, FinishRound, JourneyFailed, NextJourney }

        [SerializeField] List<EntityUIPortrait> entityUIList;
        [SerializeField, FormerlySerializedAs("resetSpecialSkill")] SkillData resetSkill;

        [ShowInInspector, ReadOnly] public int CurrentRound { get; set; }
        [ShowInInspector, ReadOnly] public StateController<State> CurrentState { get; private set; } = new();

        int MaxRound => SceneGameManager.Instance.StageData.maxRound;
        TeamManager_GamePlayer TeamManager_GamePlayer => TeamManager_GamePlayer.Instance;
        TeamManager_GameEnemy TeamManager_GameEnemy => TeamManager_GameEnemy.Instance;

        public event Action<int> OnRoundCountUpdate;

        public async void InitiateBattle()
        {
            Init();
            CurrentState.Value = State.InitiateBattle;
            await ExecuteSkills(GameActionTriggersManager.TriggerType.InitiateBattle);
            await Task.Delay(500);
            BeginRound();
        }

        void Init()
        {
            foreach (var entity in TeamManager_GamePlayer.AliveEntityList)
                entityUIList.Add(EntityUIPortraitSpawner.Instance.SpawnPlayer(entity));
            foreach (var entity in TeamManager_GameEnemy.AliveEntityList)
                entityUIList.Add(EntityUIPortraitSpawner.Instance.SpawnEnemy(entity));
        }

        async void BeginRound()
        {
            CurrentRound++;
            RearrangeEntities();
            CurrentState.Value = State.BeginRound;

            await GameActionTriggersManager.Instance.ExecuteOnTrigger(GameActionTriggersManager.TriggerType.BeginRound);
            if (CheckStopBattle())
                return;

            await ExecuteSkills(GameActionTriggersManager.TriggerType.BeginRound);
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

            FinishRound();
        }

        async void FinishRound()
        {
            CurrentState.Value = State.FinishRound;
            await GameActionTriggersManager.Instance.ExecuteOnTrigger(GameActionTriggersManager.TriggerType.FinishRound);
            if (CheckStopBattle())
                return;

            await ExecuteSkills(GameActionTriggersManager.TriggerType.FinishRound);
            if (CheckStopBattle())
                return;

            BeginRound();
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

            EndBattle();
            return true;
        }

        void EndBattle()
        {
            CurrentRound = 0;
            entityUIList.Clear();

            _ = GameActionTriggersManager.Instance.ExecuteOnTrigger(GameActionTriggersManager.TriggerType.EndBattle);
            _ = resetSkill.Execute();

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
