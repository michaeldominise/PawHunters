using System;
using UnityEngine;

namespace LabHaven.PawHunters
{
    [CreateAssetMenu(fileName = "JourneyData_Battle", menuName = "GameData/JourneyData/Battle")]
    public class JourneyData_Battle : JourneyData
    {
        public enum TransitionToBattleType { PlayerRunToEnemy, EnemyRunToPlayer }

        [SerializeField] TransitionToBattleType transitionToBattleType;
        [SerializeField] string title;
        [SerializeField] SaveableTeamData teamData;

        public override Type JourneyType => Type.Battle;
        public override string ButtonLabel => "Battle";
        public override string Title => title;
        public override float TargetDistance => 2 * TeamManager_GamePlayer.Instance.offset.x;

        public override void Execute() => JourneyButtons.Instance.Init(response => Battle(), ButtonLabel);

        void Battle()
        {
            TeamManager_GameEnemy.Instance.SpawnEnemy(teamData);

            TeamManager_Game movingTeam = transitionToBattleType == TransitionToBattleType.PlayerRunToEnemy ? TeamManager_GamePlayer.Instance : TeamManager_GameEnemy.Instance;
            TeamManager_Game targetTeam = transitionToBattleType == TransitionToBattleType.PlayerRunToEnemy ? TeamManager_GameEnemy.Instance : TeamManager_GamePlayer.Instance;

            targetTeam.SetState(StateSpeed.State.Idle);
            PlayTransition(movingTeam, targetTeam.SpawnParent.position, BattleManager.Instance.InitiateBattle);
        }

        public override void End()
        {
            TeamManager_GamePlayer.Instance.SetState(StateSpeed.State.Walking);
            JourneyLogsUIManager.Instance.Spawn(JourneyType, Title, $"[title] was defeated!");
            if(!SceneGameManager.Instance.IsLastJourney)
                JourneyButtons.Instance.Init(response => base.End(), base.ButtonLabel);
        }
    }
}
