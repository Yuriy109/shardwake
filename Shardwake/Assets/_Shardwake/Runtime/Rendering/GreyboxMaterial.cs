using UnityEngine;

namespace Shardwake.Rendering
{
    /// <summary>
    /// Creates simple runtime materials that are safe for Android/URP greybox builds.
    /// Magenta objects in Unity usually mean an unsupported or missing shader; this helper
    /// always picks the best available simple shader and writes both _BaseColor and _Color.
    /// </summary>
    public static class GreyboxMaterial
    {
        private static Shader cachedShader;

        public static Material Create(Color color)
        {
            var material = new Material(GetShader());
            ApplyColor(material, color);
            return material;
        }

        public static void Apply(Renderer renderer, Color color)
        {
            if (renderer == null)
            {
                return;
            }

            var material = renderer.sharedMaterial;
            if (material == null || material.shader == null || material.shader.name.Contains("Hidden/InternalErrorShader"))
            {
                material = Create(color);
                renderer.sharedMaterial = material;
                return;
            }

            if (material.shader != GetShader())
            {
                material = Create(color);
                renderer.sharedMaterial = material;
                return;
            }

            ApplyColor(material, color);
        }

        public static Color GetColor(Renderer renderer, Color fallback)
        {
            var material = renderer != null ? renderer.sharedMaterial : null;
            if (material == null)
            {
                return fallback;
            }

            if (material.HasProperty("_BaseColor"))
            {
                return material.GetColor("_BaseColor");
            }

            if (material.HasProperty("_Color"))
            {
                return material.GetColor("_Color");
            }

            return fallback;
        }

        private static Shader GetShader()
        {
            if (cachedShader != null)
            {
                return cachedShader;
            }

            cachedShader = Shader.Find("Universal Render Pipeline/Unlit")
                ?? Shader.Find("Universal Render Pipeline/Lit")
                ?? Shader.Find("Unlit/Color")
                ?? Shader.Find("Sprites/Default")
                ?? Shader.Find("Unlit/Texture")
                ?? Shader.Find("Standard")
                ?? Shader.Find("Hidden/Internal-Colored");

            return cachedShader;
        }

        private static void ApplyColor(Material material, Color color)
        {
            if (material == null)
            {
                return;
            }

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", color);
            }
        }
    }
}
