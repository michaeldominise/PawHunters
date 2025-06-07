using UnityEngine;
using UnityEngine.InputSystem;

namespace PawHunters
{
    public class InGameControlManager : SingletonMonoBehaviour<InGameControlManager>
    {
        [SerializeField] InputActionAsset inputActionAsset;

        private void Start() => Init();
        public void Init()
        {
        }
    }
}
