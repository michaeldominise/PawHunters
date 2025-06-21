using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class StartSceneManager : SingletonMonoBehaviour<StartSceneManager>
    {
        [SerializeField] AssetReferenceScene nextScene;
        [SerializeField] Slider slider;

        async void Start()
        {
            var operation = Addressables.LoadSceneAsync(nextScene);
            while(!operation.IsDone)
            {
                slider.value = operation.PercentComplete;
                await Task.Yield();
            }
        }
    }
}
