using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public abstract class EntityUIProgressBar : ProgressBarUI
    {
        protected EntityMainController entityMainController;

        public virtual void Init(EntityMainController entityMainController)
        {
            this.entityMainController = entityMainController;
            Init();
        }
    }
}
