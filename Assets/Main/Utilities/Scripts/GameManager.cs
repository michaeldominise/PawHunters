using UnityEngine;

namespace PawHunters
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake() => Instance = this;
    }
}
