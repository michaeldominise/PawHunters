using System.Collections.Generic;
using UnityEngine;

namespace LabHavenInteractive.PawHunters
{
	[RequireComponent(typeof(SpriteRenderer))]
    public class ParallaxSpriteRenderer : MonoBehaviour
	{
		[SerializeField] Vector2 parallaxEffectMultiplier;

		Vector2 parallaxEffectMultiplierCached;

		SpriteRenderer _MySpriteRenderer;
		SpriteRenderer MySpriteRenderer
		{
			get
            {
				_MySpriteRenderer = _MySpriteRenderer == null ? GetComponent<SpriteRenderer>() : _MySpriteRenderer;
				return _MySpriteRenderer;
			}
        }

		public void Update()
        {
			if (parallaxEffectMultiplierCached == parallaxEffectMultiplier)
				return;

			MySpriteRenderer.material.SetVector("_Multiplier", parallaxEffectMultiplier);
		}
    }
}
