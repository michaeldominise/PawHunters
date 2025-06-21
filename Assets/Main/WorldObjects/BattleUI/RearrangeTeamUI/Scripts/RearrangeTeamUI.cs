using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    public class RearrangeTeamUI : SingletonMonoBehaviour<RearrangeTeamUI>
    {
        public void OnClick() => TeamManager_GamePlayer.Instance.Rearrange();
    }
}
