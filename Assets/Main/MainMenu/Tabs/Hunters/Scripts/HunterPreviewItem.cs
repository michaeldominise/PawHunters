using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabHavenInteractive.PawHunters
{
    public class HunterPreviewItem : EntityPreviewItem<SaveableCharacterData>
    {
        [SerializeField] TextMeshProUGUI nameLabel;

        public override async Task Init(int index, SaveableCharacterData data)
        {
            await base.Init(index, data);
            nameLabel.text = data.Name;
        }
    }
}
