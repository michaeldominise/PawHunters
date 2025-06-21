using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public partial class AppManager : SingletonMonoBehaviour<AppManager>
    {
        [SerializeField] AppSettings_Global appSettings_Global;
        [SerializeField] MasterIDManager masterIDManager;

        public AppSettings_Global AppSettings_Global => appSettings_Global;
        public MasterIDManager MasterIDManager => masterIDManager;

        protected override bool SetupInstance()
        {
            if (!base.SetupInstance())
                return false;

            DontDestroyOnLoad(this);
            return true;
        }

        private void Update() => SaveableData.Execute();
    }
}
