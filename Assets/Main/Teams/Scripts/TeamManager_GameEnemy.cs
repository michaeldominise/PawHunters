using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using System;
using System.Threading.Tasks;

namespace LabHavenInteractive.PawHunters
{
    public class TeamManager_GameEnemy : TeamManager_Game
    {
        public static TeamManager_GameEnemy Instance { get; private set; }

        public override float MovementSpeed => -base.MovementSpeed;

        void Awake() => Instance = this;

        [Button]
        public async Task SpawnEnemy(SaveableTeamData teamData)
        {
            spawnParent.transform.position = TeamManager_GamePlayer.Instance.SpawnParent.transform.position + offset;
            await Init(teamData);
        }
    }
}
