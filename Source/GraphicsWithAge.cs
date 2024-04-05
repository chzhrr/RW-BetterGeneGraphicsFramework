using System.Collections.Generic;
using RimWorld;
using Verse;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable UnassignedField.Global

namespace BetterGeneGraphicsFramework
{
    public class GraphicsWithAge : DefModExtension
    {
        public List<float> ages;
        /// <summary>
        /// Fallback.
        /// </summary>
        public List<string> graphicPaths;
        /// <summary>
        /// Fallback if no bodyTypeGraphicPaths.
        /// </summary>
        public List<string> graphicPathsFemale;
        /// <summary>
        /// Top priority
        /// </summary>
        public List<BodyTypeGraphicData> bodyTypeGraphicPaths;
        public List<string> bodyPartExpressions;
    }
}