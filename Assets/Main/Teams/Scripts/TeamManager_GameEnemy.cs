using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using System;

namespace LabHaven.PawHunters
{
    public class TeamManager_GameEnemy : TeamManager_Game
    {
        public static TeamManager_GameEnemy Instance { get; private set; }

        public override float MovementSpeed => -base.MovementSpeed;

        void Awake() => Instance = this;

        [Button]
        public void SpawnEnemy(SaveableTeamData teamData)
        {
            spawnParent.transform.position = TeamManager_GamePlayer.Instance.SpawnParent.transform.position + offset;
            Init(teamData);
        }
    }
}
