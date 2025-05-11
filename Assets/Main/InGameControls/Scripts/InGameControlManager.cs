using UnityEngine;
using UnityEngine.InputSystem;

namespace PawHunters
{
    public class InGameControlManager : MonoBehaviour
    {
        public static InGameControlManager Instance { get; private set; }

        [SerializeField] InputActionAsset inputActionAsset;

        private void Awake() => Instance = this;
        private void Start() => Init();
        public void Init()
        {
        }
    }
}
