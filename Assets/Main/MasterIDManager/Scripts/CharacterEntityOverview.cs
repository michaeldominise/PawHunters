using Sirenix.OdinInspector;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [CreateAssetMenu(fileName = "CharacterEntityOverview", menuName = "GameData/Overviews/CharacterEntityOverview")]
    public class CharacterEntityOverview : EntityOverview<CharacterEntityOverview.AssetReferenceMasterID, CharacterMainController>
    {
        public static CharacterEntityOverview Instance => MasterIDManager.Instance.characterEntityOverview;

        [System.Serializable]
        public class AssetReferenceMasterID : AssetReferenceMasterID<CharacterMainController>
        {
            [ReadOnly] public ElementType elementType;
        }

        public override AssetReferenceMasterID Create(string guid)
        {
            var assetReferenceMasterID = base.Create(guid);
#if UNITY_EDITOR
            assetReferenceMasterID.elementType = assetReferenceMasterID.AssetReference.Element;
#endif
            return assetReferenceMasterID;
        }
    }
}
