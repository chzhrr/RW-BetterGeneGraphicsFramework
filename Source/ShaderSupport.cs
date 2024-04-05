using UnityEngine;
using Verse;
using static Verse.PawnRenderNodeProperties;

namespace BetterGeneGraphicsFramework
{
    /// <summary>
    /// Allow user to pass the second color to shader.
    /// </summary>
    public class ShaderSupport : DefModExtension
    {
        /// <summary>
        /// Skin, Hair or Custom.
        /// </summary>
        public AttachmentColorType? colorTwoType;
        /// <summary>
        /// Fixed color with Custom.
        /// </summary>
        public Color colorTwo = Color.white;
    }
}