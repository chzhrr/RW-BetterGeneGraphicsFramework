using RimWorld;
using System.Collections.Generic;
using Verse;

namespace BetterGeneGraphicsFramework
{
    public class GraphicsWithRecord : DefModExtension
    {
        public RecordDef recordDef;
        public List<float> cutoffs;
        /// <summary>
        /// Fallback.
        /// </summary>
        public List<string> graphicPaths;
        /// <summary>
        /// Fallback if no bodyTypeGraphicPaths.
        /// </summary>
        public List<string> graphicPathsFemale;
        /// <summary>
        /// Top priority.
        /// </summary>
        public List<BodyTypeGraphicData> bodyTypeGraphicPaths;
    }
}
