using UnityEditor;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public partial class AddressableEnvironmentSettings
    {
        private const string MenuRootPath = "Tools/Addressables/";
        private const string MenuSetToDev = MenuRootPath + "Set to Dev";
        private const string MenuSetToQA = MenuRootPath + "Set to QA";
        private const string MenuSetToProd = MenuRootPath + "Set to Prod";

        [MenuItem(MenuSetToDev)] static void SetToDev() => SetEnvironment(EnvironmentData.EnvironmentType.Dev);
        [MenuItem(MenuSetToQA)] static void SetToQA() => SetEnvironment(EnvironmentData.EnvironmentType.QA);
        [MenuItem(MenuSetToProd)] static void SetToProd() => SetEnvironment(EnvironmentData.EnvironmentType.Prod);

        [MenuItem(MenuSetToDev, true)] static bool SetToDevValidate() => ValidateEnvironment(MenuSetToDev, EnvironmentData.EnvironmentType.Dev);
        [MenuItem(MenuSetToQA, true)] static bool SetToQAValidate() => ValidateEnvironment(MenuSetToQA, EnvironmentData.EnvironmentType.QA);
        [MenuItem(MenuSetToProd, true)] static bool SetToProdValidate() => ValidateEnvironment(MenuSetToProd, EnvironmentData.EnvironmentType.Prod);

        static bool ValidateEnvironment(string menuItemPath, EnvironmentData.EnvironmentType environmentType)
        {
            Menu.SetChecked(menuItemPath, Instance.buildEnvironment == environmentType);
            return Instance.buildEnvironment != environmentType;
        }

        static void SetEnvironment(EnvironmentData.EnvironmentType environmentType)
        {
            Instance.buildEnvironment = environmentType;
            EditorUtility.SetDirty(Instance);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Addressable environment set to {environmentType}.");
        }
    }
}