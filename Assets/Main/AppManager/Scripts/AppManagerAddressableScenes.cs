using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace LabHavenInteractive.PawHunters
{
    public partial class AppManager
    {
        public enum TransitionType { None, ShowThenHide, ShowOnly }

        AssetReferenceScene currentSceneAssetReference;
        AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> sceneLoadOperation;

        public AsyncOperationHandle Reload()
        {
            currentSceneAssetReference.ReleaseAsset();
            return LoadScene(currentSceneAssetReference);
        }

        public AsyncOperationHandle LoadScene(AssetReferenceScene sceneAssetReference, LoadSceneMode loadSceneMode = LoadSceneMode.Single, TransitionType transitionType = TransitionType.ShowThenHide)
        {
            currentSceneAssetReference = sceneAssetReference;
            if (transitionType != TransitionType.None)
                SceneTransitionLoader.Instance.Show();
            sceneLoadOperation = sceneAssetReference.LoadSceneAsync(loadSceneMode);
            if (transitionType == TransitionType.ShowThenHide)
                HideSceneTransitionLoaderOnComplete();
            return sceneLoadOperation;
        }

        public async void HideSceneTransitionLoaderOnComplete()
        {
            await sceneLoadOperation.Task;
            SceneTransitionLoader.Instance.Hide();
        }
    }
}
