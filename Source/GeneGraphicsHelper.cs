using System.Collections.Generic;
using Verse;

namespace BetterGeneGraphicsFramework
{
    public class GeneGraphicsHelper : GameComponent
    {
        public HashSet<Pawn> pawnList = new HashSet<Pawn>();

        public GeneGraphicsHelper(Game game)
        {
        }

        public bool ShouldRedrawWhenHediffChange(Pawn pawn)
        {
            return pawn != null && pawnList.Contains(pawn);
        }

        public void AddPawn(Pawn pawn)
        {
            if (pawn != null)
            {
                pawnList.Add(pawn);
            }
        }

        public void RemovePawn(Pawn pawn)
        {
            pawnList.Remove(pawn);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref pawnList, saveDestroyedThings: false, "pawnList", LookMode.Reference);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                pawnList.RemoveWhere((Pawn x) => x == null);
            }
        }
    }
}