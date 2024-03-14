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
        public List<string> graphicPaths;
        public List<string> graphicPathsFemale;
        public List<BodyTypeGraphicData> bodyTypeGraphicPaths;
        public List<string> bodyPartExpressions;
    }
}