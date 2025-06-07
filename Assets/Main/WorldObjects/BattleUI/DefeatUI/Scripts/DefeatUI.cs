using UnityEngine;
using UnityEngine.SceneManagement;

namespace PawHunters
{
    public class DefeatUI : SingletonMonoBehaviour<DefeatUI>
    {
        [SerializeField] GameObject container;

        public void Show(bool value) => container.SetActive(value);

        public void OnClaimClicked()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}
