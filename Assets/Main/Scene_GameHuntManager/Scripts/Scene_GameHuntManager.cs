using System.Collections;
using UnityEngine;

namespace PawHunters
{
    public class Scene_GameHuntManager : MonoBehaviour
    {
        public static Scene_GameHuntManager Instance { get; private set; }

        [SerializeField] TeamManager_GamePlayer playerTeamManager;
        [SerializeField] SaveableTeamData teamData;

        public TeamManager_GamePlayer PlayerTeamManager => playerTeamManager;

        private void Awake() => Instance = this;
        private IEnumerator Start()
        {
            yield return null;
            playerTeamManager.Init(teamData);
            EnvironmentManager.Instance.Init();
        }
    }
}
