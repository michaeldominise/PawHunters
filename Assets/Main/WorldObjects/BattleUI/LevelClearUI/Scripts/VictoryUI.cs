using UnityEngine;
using UnityEngine.SceneManagement;

namespace LabHavenInteractive.PawHunters
{
    public class VictoryUI : SingletonMonoBehaviour<VictoryUI>
    {
        [SerializeField] GameObject container;

        public void Show(bool value) => container.SetActive(value);

        public void OnClaimClicked() => _ = AppManager.Instance.Reload();
    }
}
