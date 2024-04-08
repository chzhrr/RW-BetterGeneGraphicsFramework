using System.Collections.Generic;
using Verse;

namespace BetterGeneGraphicsFramework
{
    public class GameComponent_AlternateGraphics : GameComponent
    {
        private Dictionary<Pawn, GeneGraphicsData> PawnGeneGraphicsData =
            new Dictionary<Pawn, GeneGraphicsData>();

        private List<Pawn> pawns;
        private List<GeneGraphicsData> data;

        public GameComponent_AlternateGraphics(Game game)
        {
        }

        public int GetFixedIndex(Pawn pawn, Gene gene)
        {
            if (PawnGeneGraphicsData.TryGetValue(pawn, out GeneGraphicsData _data))
            {
                int ind = _data.fixedIndexByGeneDefName.TryGetValue(gene.def.defName);
                return ind >= 0 ? ind : -1;
            }
            return -1;
        }

        public void RegisterFixedIndex(Pawn pawn, Gene gene, int fixedIndex)
        {
            if (PawnGeneGraphicsData.TryGetValue(pawn, out GeneGraphicsData _data))
            {
                _data.fixedIndexByGeneDefName.SetOrAdd(gene.def.defName, fixedIndex);
            }
            else
            {
                PawnGeneGraphicsData[pawn] = new GeneGraphicsData();
                PawnGeneGraphicsData[pawn].fixedIndexByGeneDefName.Add(gene.def.defName, fixedIndex);
            }
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref PawnGeneGraphicsData, "PawnGeneGraphicsData", 
                LookMode.Reference, LookMode.Deep, ref pawns, ref data, logNullErrors: false);
            
            // no cleanup at runtime, only after save load
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                PawnGeneGraphicsData.RemoveAll((pair) => pair.Key == null);
                
                foreach (var (pawn, _data) in PawnGeneGraphicsData)
                {
                    List<string> keysToRemove = new List<string>();
                    if (_data.fixedIndexByGeneDefName != null)
                    {
                        foreach (var (defName, index) in _data.fixedIndexByGeneDefName)
                        {
                            bool hasGene = pawn.genes?.GenesListForReading?.Any((g) => g.def.defName == defName) ?? true;
                            if (!hasGene)
                            {
                                keysToRemove.Add(defName);
                            }
                        }
                    }
                    foreach (var key in keysToRemove)
                    {
                        _data.fixedIndexByGeneDefName?.Remove(key);
                    }
                }
                PawnGeneGraphicsData.RemoveAll((pair) => pair.Value.fixedIndexByGeneDefName.NullOrEmpty());
            }
        }
    }
}
