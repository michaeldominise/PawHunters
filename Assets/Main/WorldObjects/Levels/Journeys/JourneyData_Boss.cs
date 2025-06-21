using System;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
    [CreateAssetMenu(fileName = "JourneyData_Boss", menuName = "GameData/JourneyData/Boss")]
    public class JourneyData_Boss : JourneyData_Battle
    {
        public override Type JourneyType => Type.Boss;
    }
}
