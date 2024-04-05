using Verse;

namespace BetterGeneGraphicsFramework
{
    /// <summary>
    /// Scale head attachments with head texture size.
    /// </summary>
    public class PawnRenderNode_GeneHeadAttachment : PawnRenderNode_GeneAttachment
    {
        public PawnRenderNode_GeneHeadAttachment(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) 
            : base(pawn, props, tree)
        {
        }

        public override GraphicMeshSet MeshSetFor(Pawn pawn)
        {
            return HumanlikeMeshPoolUtility.GetHumanlikeHairSetForPawn(pawn);
        }
    }
}
