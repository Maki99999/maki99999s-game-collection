using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class CameraBlurJitterEffect : MonoBehaviour
    {
        public int effectStep = 0;
        public float jitterXMax = .04f;
        public float jitterYMax = .01f;
        public PlayerController playerController;

        public Shader bloomShader;

        protected Material mainMaterial;

        void Awake()
        {
            mainMaterial = new Material(bloomShader);
        }

        void Update()
        {
            float jitterXCurrentMax = effectStep == 0 ? 0f : effectStep == 1 ? jitterXMax / 2f : jitterXMax;
            float jitterYCurrentMax = effectStep == 0 ? 0f : effectStep == 1 ? jitterYMax / 2f : jitterYMax;
            
            playerController.camOffsetX  = Random.Range(-jitterXCurrentMax, jitterXCurrentMax);
            playerController.camOffsetY  = Random.Range(-jitterYCurrentMax, jitterYCurrentMax);
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            if (effectStep == 0)
            {
                Graphics.Blit(src, dst);
            }
            else if (effectStep == 1)
            {
                mainMaterial.SetFloat("_Spread", 2.0f);
                // Create a temporary RenderTexture to hold the first pass.
                RenderTexture tmp =
                    RenderTexture.GetTemporary(src.width, src.height, 0, src.format);

                // Perform both passes in order.
                Graphics.Blit(src, tmp, mainMaterial, 0);   // First pass.
                Graphics.Blit(tmp, dst, mainMaterial, 1);   // Second pass.

                RenderTexture.ReleaseTemporary(tmp);
            }
            else
            {
                mainMaterial.SetFloat("_Spread", 5.0f);
                // Create a temporary RenderTexture to hold the first pass.
                RenderTexture tmp =
                    RenderTexture.GetTemporary(src.width, src.height, 0, src.format);

                // Perform both passes in order.
                Graphics.Blit(src, tmp, mainMaterial, 0);   // First pass.
                Graphics.Blit(tmp, dst, mainMaterial, 1);   // Second pass.

                RenderTexture.ReleaseTemporary(tmp);
            }

        }
    }
}