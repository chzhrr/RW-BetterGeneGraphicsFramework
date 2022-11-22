using System.Collections.Generic;
using Verse;

namespace BetterGeneGraphicsFramework
{
    public class GeneGraphicsHelper : GameComponent
    {
        public List<string> pawnList = new List<string>();

        public GeneGraphicsHelper(Game game)
        {
        }

        public bool ShouldRedrawWhenHediffChange(Pawn pawn)
        {
            return pawnList.Contains(pawn?.GetUniqueLoadID());
        }

        public void AddPawn(Pawn pawn)
        {
            pawnList.Add(pawn.GetUniqueLoadID());
        }

        public void RemovePawn(Pawn pawn)
        {
            pawnList.Remove(pawn.GetUniqueLoadID());
        }

        public override void FinalizeInit()
        {
            HarmonyPatcher.helper = Current.Game.GetComponent<GeneGraphicsHelper>();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref pawnList, "pawnList");
        }
    }
}