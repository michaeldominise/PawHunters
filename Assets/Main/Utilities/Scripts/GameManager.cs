using UnityEngine;

namespace LabHaven.PawHunters
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        [SerializeField] GameSettings_Global gameSettings_Global;
        [SerializeField] MasterIDManager masterIDManager;

        public GameSettings_Global GameSettings_Global => gameSettings_Global;
        public MasterIDManager MasterIDManager => masterIDManager;

        private void Update() => SaveableData.Execute();
    }
}
