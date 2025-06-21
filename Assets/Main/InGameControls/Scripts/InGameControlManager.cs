using UnityEngine;
using UnityEngine.InputSystem;

namespace LabHavenInteractive.PawHunters
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
