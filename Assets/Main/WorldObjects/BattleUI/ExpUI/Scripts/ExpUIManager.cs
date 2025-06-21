using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class ExpUIManager : ProgressBarUI
    {
        public static ExpUIManager Instance { get; private set; }

        [SerializeField] float level;
        [SerializeField] float currentValue;

        protected override float UpdateDuration => AppSettings_Battle.Instance.constantValues.progressUpdateDurationSlow;
        protected override Color ProgressColor => AppSettings_Battle.Instance.colorTheme.expProgressColor;
        protected override float CurrentValue => currentValue;
        protected override float MaxValue => AppSettings_Battle.Instance.constantValues.expMaxValue;

        Action onFinish;

        private void Awake() => Instance = this;
        private IEnumerator Start()
        {
            yield return null;
            Refresh();
        }

        public void AddExp(int value, Action onFinish = null)
        {
            this.onFinish = onFinish;
            currentValue += value;
            Refresh();
        }

        protected override void OnProgressUpdate(GradualChangeValue.Status status)
        {
            base.OnProgressUpdate(status);
            if(status.IsDone)
            {
                var newLevel = (int)(CurrentValue / MaxValue);
                if (newLevel > level)
                    SkillSelectionUIManager.Instance.Init(SkillDataSelected, SceneGameManager_Hunt.Instance_Hunt.TeamSkills.ToArray());
                else
                    onFinish?.Invoke();
                level = newLevel;
            }
        }

        void SkillDataSelected(SkillData skillData)
        {
            JourneyLogsUIManager.Instance.Spawn(skillData.rarity == RarityType.None ? JourneyData.Type.Negative : JourneyData.Type.Default, JourneyData_TeamSkill.GetTitle(skillData), JourneyData_TeamSkill.GetDescription(skillData));
            TeamManager_GamePlayer.Instance.AddSkill(skillData);
            onFinish?.Invoke();
        }
    }
}
