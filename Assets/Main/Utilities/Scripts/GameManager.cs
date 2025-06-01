using UnityEngine;

namespace PawHunters
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] GlobalSettings globalSettings;
        public GlobalSettings GlobalSettings => globalSettings;

        private void Awake() => Instance = this;
        private void Update() => SaveableData.Execute();
    }
}
