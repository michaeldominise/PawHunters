using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public partial class AddressableEnvironmentSettings
    {
        static AddressableEnvironmentSettings _Instance;
        public static AddressableEnvironmentSettings Instance
        {
            get
            {
                _Instance ??= Resources.Load<AddressableEnvironmentSettings>("AddressableEnvironmentSettings");
                return _Instance;
            }
        }

        public static string ProjectId => CloudProjectSettings.projectId;
        public static string BuildEnvironmentString => CurrentBuildEnvironment.ToString().ToLower();
        public static EnvironmentData.EnvironmentType CurrentBuildEnvironment => Instance.BuildEnvironment;
        public static EnvironmentData CurrentEnvironment => Instance.environments.FirstOrDefault(x => x.environmentType == CurrentBuildEnvironment);
        public static EnvironmentData.BucketData CurrentBucket => CurrentEnvironment.buckets.FirstOrDefault(x => x.buildTarget == EditorUserBuildSettings.activeBuildTarget);

        public static string LocalBuildPath => $"../CCDBuildData/{BuildEnvironmentString}/[BuildTarget]/{System.DateTime.Now:dd.MM.yyyy HH.mm.ss}";
        public static string LocalLoadPath
        {
            get
            {
                string parentPath = Path.GetDirectoryName(LocalBuildPath);
                var directoryInfo = new DirectoryInfo(parentPath);
                var latestDir = directoryInfo.GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault();

                return latestDir?.FullName ?? LocalBuildPath;
            }
        }

        public static string RemoteBuildPath => LocalBuildPath;
        public static string RemoteLoadPath => $"https://{ProjectId}.client-api.unity3dusercontent.com/client_api/v1/environments/{BuildEnvironmentString}/buckets/{CurrentBucket.id}/release_by_badge/latest/entry_by_path/content/?path=";


        public static string OverrideLoadPath => Instance.overrideLoadPath;

        public static string BuildPath => Instance.profile switch
        {
            ProfileType.Remote => RemoteBuildPath,
            _ => LocalBuildPath,
        };

        public static string LoadPath => Instance.useOverrideLoadPath
            ? OverrideLoadPath
            : Instance.profile switch
            {
                ProfileType.Remote => RemoteLoadPath,
                _ => LocalLoadPath,
            };
    }
}