using System;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    [RequireComponent(typeof(Button))]
    public class SceneLoadButton : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] AssetReferenceScene sceneToLoad;
        [SerializeField] AppManager.TransitionType transitionType = AppManager.TransitionType.ShowThenHide;

        private void Reset() => button = GetComponent<Button>();
        private void Start() => button.onClick.AddListener(OnClick);
        private void OnClick() => AppManager.Instance.LoadScene(sceneToLoad, transitionType: transitionType);
    }
}
