using System;
using System.IO;
using System.Text;
using MelonLoader;
using UnityEngine;

namespace ArchipelagoEverhood.Util
{
    /// <summary>
    /// A collection of methods to help print out information about gameobjects in the heirachy
    /// </summary>
    public static class AssetHelpers
    {
        public static MelonAssembly Assembly;

        public static Texture2D? LoadTexture(string resourcePath)
        {
            try
            {
                using (var stream = Assembly.Assembly.GetManifestResourceStream(resourcePath))
                {
                    if (stream == null)
                    {
                        Globals.Logging.Warning("LoadExternalAssets", "Help");
                        return null;
                    }

                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        var archIconTexture = new Texture2D(1, 1);
                        archIconTexture.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                        archIconTexture.wrapMode = TextureWrapMode.Clamp;
                        archIconTexture.LoadImage(ms.ToArray());
                        return archIconTexture;
                    }
                }
            }
            catch (Exception e)
            {
                Globals.Logging.Error("AssetHelper", e);
                return null;
            }
        }

        /// <summary>
        /// Overwrites the <see cref="Texture"/> with the loaded texture.
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <param name="texture"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>A replacement texture if the original wasn't overwriteable</returns>
        public static Texture2D? OverwriteTextureWithTexture(string resourcePath, Texture2D texture, int positionX, int positionY, int width, int height)
        {
            var overwrite = LoadTexture("ArchipelagoEverhood.Assets.APIcons.png");
            if (overwrite == null)
            {
                Globals.Logging.Warning("AssetHelper", $"Failed to overwrite texte. {resourcePath} texture doesn't exist.");
                return null;
            }

            if (texture.isReadable)
            {
                texture.SetPixels(positionX, positionY, width, height, overwrite.GetPixels());
                return null;
            }

            var origFilter = texture.filterMode;
            var origRenderTexture = RenderTexture.active;

            texture.filterMode = FilterMode.Point;

            var rt = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32);
            rt.filterMode = FilterMode.Point;
            RenderTexture.active = rt;

            Graphics.Blit(texture, rt);

            var newTex = new Texture2D(texture.width, texture.height);
            newTex.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            newTex.Apply(false);

            RenderTexture.active = origRenderTexture;
            texture.filterMode = origFilter;

            newTex.filterMode = origFilter;
            newTex.SetPixels(positionX, positionY, width, height, overwrite.GetPixels());
            newTex.Apply(false, false);
            return newTex;
        }

        /// <summary>
        /// Makes Variable Names better. Thanks to ErnestSurys https://discussions.unity.com/t/nicefy-variable-names-at-runtime/879079/2
        /// </summary>
        /// <param name="input">Text to nicify</param>
        /// <returns>Nice text</returns>
        public static string NicifyName(string input)
        {
            var result = new StringBuilder(input.Length * 2);

            var prevIsLetter = false;
            var prevIsLetterUpper = false;
            var prevIsDigit = false;
            var prevIsStartOfWord = false;
            var prevIsNumberWord = false;

            var firstCharIndex = 0;
            if (input.StartsWith('_'))
                firstCharIndex = 1;
            else if (input.StartsWith("m_"))
                firstCharIndex = 2;

            for (var i = input.Length - 1; i >= firstCharIndex; i--)
            {
                var currentChar = input[i];
                var currIsLetter = char.IsLetter(currentChar);
                if (i == firstCharIndex && currIsLetter)
                    currentChar = char.ToUpper(currentChar);
                var currIsLetterUpper = char.IsUpper(currentChar);
                var currIsDigit = char.IsDigit(currentChar);
                var currIsSpacer = currentChar == ' ' || currentChar == '_';

                var addSpace = (currIsLetter && !currIsLetterUpper && prevIsLetterUpper) ||
                               (currIsLetter && prevIsLetterUpper && prevIsStartOfWord) ||
                               (currIsDigit && prevIsStartOfWord) ||
                               (!currIsDigit && prevIsNumberWord) ||
                               (currIsLetter && !currIsLetterUpper && prevIsDigit);

                if (!currIsSpacer && addSpace)
                {
                    result.Insert(0, ' ');
                }

                result.Insert(0, currentChar);
                prevIsStartOfWord = currIsLetter && currIsLetterUpper && prevIsLetter && !prevIsLetterUpper;
                prevIsNumberWord = currIsDigit && prevIsLetter && !prevIsLetterUpper;
                prevIsLetterUpper = currIsLetter && currIsLetterUpper;
                prevIsLetter = currIsLetter;
                prevIsDigit = currIsDigit;
            }

            return result.ToString();
        }
    }
}