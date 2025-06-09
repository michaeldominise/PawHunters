using System;
using System.Collections;
using Assets.FantasyMonsters.Common.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace LabHaven.PawHunters
{
    public class EntityMainController_Team : EntityMainController
    {
        [SerializeField] BattleAttributes teamBattleAttributes;

        public override BattleAttributes BattleAttributes => teamBattleAttributes;
        public override bool IsAlive => true;

        public override void Init(TeamManager teamManager, SaveableCharacterData characterData) { }
        public void Init(TeamManager_Game teamManager)
        {
            this.teamManager = teamManager;
            entitySkillsController.Init(this);
            entityStatusEffectController.Init(this);

            var entityList = teamManager.EntityList;

            teamBattleAttributes.Reset();
            foreach (var entity in entityList)
            {
                teamBattleAttributes.maxHealth.Update(entity.BattleAttributes.maxHealth.Value);
                teamBattleAttributes.attack.Update(entity.BattleAttributes.attack.Value);
                teamBattleAttributes.speed.Update(entity.BattleAttributes.speed.Value);
            }

            teamBattleAttributes.maxHealth.Reset(teamBattleAttributes.maxHealth.Value / entityList.Count);
            teamBattleAttributes.attack.Update(teamBattleAttributes.attack.Value / entityList.Count);
            teamBattleAttributes.speed.Update(teamBattleAttributes.speed.Value / entityList.Count);
        }
    }
}
