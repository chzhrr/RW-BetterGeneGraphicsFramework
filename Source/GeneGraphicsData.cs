using System.Collections.Generic;
using Verse;

namespace BetterGeneGraphicsFramework
{
    public class GeneGraphicsData : IExposable
    {
        public Dictionary<string, int> fixedIndexByGeneDefName = new Dictionary<string, int>();

        public void ExposeData()
        {
            Scribe_Collections.Look(ref fixedIndexByGeneDefName, "fixedIndexByGeneDefName", 
                LookMode.Value, LookMode.Value);
        }
    }
}
