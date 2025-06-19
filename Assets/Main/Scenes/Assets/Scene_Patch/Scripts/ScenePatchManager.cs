using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace LabHaven.PawHunters
{
    public class ScenePatchManager : SingletonMonoBehaviour<ScenePatchManager>
    {
        [SerializeField] AssetReferenceScene nextScene;
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI sliderLabel;
        [SerializeField] TextMeshProUGUI downloadLabel;

        async void Start()
        {
            AsyncOperationHandle operation;

            await Addressables.InitializeAsync().Task;
            await Addressables.CleanBundleCache().Task;

            List<object> allKeys = new List<object>();
            foreach (var locator in Addressables.ResourceLocators)
                allKeys.AddRange(locator.Keys);

            long totalDownloadSize = await Addressables.GetDownloadSizeAsync(allKeys).Task;
            Debug.Log("Total download size: " + totalDownloadSize + " bytes");

            operation = Addressables.DownloadDependenciesAsync(allKeys, Addressables.MergeMode.Union);
            var operationStatus = await SetOperation(operation, percent => $"Downloading game contents...{((long)percent * totalDownloadSize).ToSize(ByteExtension.SizeUnits.MB)}/{totalDownloadSize.ToSize(ByteExtension.SizeUnits.MB)}");

            if (operationStatus == AsyncOperationStatus.Failed)
                return;

            operation = AppManager.Instance.LoadScene(nextScene, transitionType: AppManager.TransitionType.None);
            operationStatus = await SetOperation(operation, percent => "Laoding next scene...", false);
        }

        async Task<AsyncOperationStatus> SetOperation(AsyncOperationHandle operation, System.Func<float, string> onUpdate, bool autoReleaseHandle = true)
        {
            while (!operation.IsDone)
            {
                slider.value = operation.PercentComplete;
                sliderLabel.text = $"{operation.PercentComplete:P0}";
                downloadLabel.text = onUpdate?.Invoke(operation.PercentComplete) ?? "";
                await Task.Yield();
            }
            
            var operationStatus = operation.Status;
            downloadLabel.text = operationStatus switch
            {
                AsyncOperationStatus.Succeeded => "Done.",
                _ => "Failed.",
            };
            if (autoReleaseHandle)
                Addressables.Release(operation);
            return operationStatus;
        }
    }
}
