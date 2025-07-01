using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [CreateAssetMenu(fileName = "CharacterEntityOverview", menuName = "GameData/Overviews/CharacterEntityOverview")]
    public class CharacterEntityOverview : EntityOverview<CharacterMainController>
    {
        public static CharacterEntityOverview Instance => MasterIDManager.Instance.characterEntityOverview;
    }
}
