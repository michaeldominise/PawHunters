using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LabHaven.PawHunters
{
    public partial class AppManager
    {
        AssetReference currentSceneAssetReference;
        AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> sceneLoadOperation;

        public AsyncOperationHandle Reload() => LoadScene(currentSceneAssetReference);
        public AsyncOperationHandle LoadScene(AssetReference sceneAssetReference)
        {
            currentSceneAssetReference = sceneAssetReference;
            sceneLoadOperation = Addressables.LoadSceneAsync(sceneAssetReference);
            return sceneLoadOperation;
        }
    }
}
