using System.Collections;
using Shardwake.Rendering;
using UnityEngine;

namespace Shardwake.Combat
{
    public sealed class HitFeedback : MonoBehaviour
    {
        [SerializeField] private Color flashColor = Color.white;
        [SerializeField] private float flashDuration = 0.08f;

        private Renderer targetRenderer;
        private Color baseColor;
        private Coroutine flashRoutine;

        private void Awake()
        {
            targetRenderer = GetComponentInChildren<Renderer>();
            if (targetRenderer != null)
            {
                baseColor = GreyboxMaterial.GetColor(targetRenderer, Color.white);
            }
        }

        public void Play()
        {
            if (targetRenderer == null)
            {
                return;
            }

            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }

            flashRoutine = StartCoroutine(Flash());
        }

        private IEnumerator Flash()
        {
            GreyboxMaterial.Apply(targetRenderer, flashColor);
            yield return new WaitForSeconds(flashDuration);
            GreyboxMaterial.Apply(targetRenderer, baseColor);
            flashRoutine = null;
        }
    }
}
