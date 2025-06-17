using UnityEngine;
using UnityEngine.SceneManagement;

namespace LabHaven.PawHunters
{
    public class DefeatUI : SingletonMonoBehaviour<DefeatUI>
    {
        [SerializeField] GameObject container;

        public void Show(bool value) => container.SetActive(value);

        public void OnClaimClicked() => AppManager.Instance.Reload();
    }
}
