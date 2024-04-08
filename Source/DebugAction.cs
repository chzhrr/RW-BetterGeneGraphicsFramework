using LudeonTK;
using Verse;

namespace BetterGeneGraphicsFramework
{
    public static class DebugAction
    {
        [DebugAction("GeneGraphicsFramework", "Alternate Graphics", actionType = DebugActionType.ToolMapForPawns, 
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void ViewRenderTree(Pawn p)
        {
            Find.WindowStack.Add(new Dialog_AlternateGraphics(p));
        }
    }
}
