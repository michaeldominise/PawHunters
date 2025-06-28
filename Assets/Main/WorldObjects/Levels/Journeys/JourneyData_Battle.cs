using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [CreateAssetMenu(fileName = "JourneyData_Battle", menuName = "GameData/JourneyData/Battle")]
    public class JourneyData_Battle : JourneyData
    {
        public enum TransitionToBattleType { PlayerRunToEnemy, EnemyRunToPlayer }

        [SerializeField] TransitionToBattleType transitionToBattleType;
        [SerializeField] SaveableTeamData_CharacterData teamData;

        public override Type JourneyType => Type.Battle;
        public override string ButtonLabel => "Battle";
        public override string Title => teamData.teamName;
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
            if (!SceneGameManager.Instance.IsLastJourney)
                JourneyButtons.Instance.Init(response => base.End(), base.ButtonLabel);
            else
                base.End();
        }

        public override async Task LoadAssets() => await teamData.LoadAssets(SaveableTeamData.AssetType.All);
        public override void UnloadAssets() => teamData.UnloadAssets(SaveableTeamData.AssetType.All);

    }
}
