using UnityEngine;
using UnityEngine.SceneManagement;

namespace LabHavenInteractive.PawHunters
{
    public class DefeatUI : SingletonMonoBehaviour<DefeatUI>
    {
        [SerializeField] GameObject container;

        public void Show(bool value) => container.SetActive(value);

        public void OnClaimClicked() => AppManager.Instance.Reload();
    }
}
