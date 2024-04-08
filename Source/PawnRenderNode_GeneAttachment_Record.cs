using RimWorld;
using System.Collections.Generic;
using Verse;

namespace BetterGeneGraphicsFramework
{
    public class PawnRenderNode_GeneAttachment_Record : PawnRenderNode_GeneAttachment
    {
        public PawnRenderNode_GeneAttachment_Record(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
            : base(pawn, props, tree)
        {
        }

        protected override string TexPathFor(Pawn pawn)
        {
            GraphicsWithRecord extension = gene.def.GetModExtension<GraphicsWithRecord>();
            if (extension != null && extension.recordDef != null && !extension.cutoffs.NullOrEmpty())
            {
                List<float> cutoffs = extension.cutoffs;

                List<string> paths = null;
                if (!extension.bodyTypeGraphicPaths.NullOrEmpty())
                {
                    foreach (BodyTypeGraphicData bodyTypeGraphicPath in extension.bodyTypeGraphicPaths)
                    {
                        if (pawn.story.bodyType == bodyTypeGraphicPath.bodyType)
                        {
                            paths = new List<string> { bodyTypeGraphicPath.texturePath };
                        }
                    }
                }
                else if (pawn.gender == Gender.Female && !extension.graphicPathsFemale.NullOrEmpty())
                {
                    paths = extension.graphicPathsFemale;
                }
                else
                {
                    paths = extension.graphicPaths;
                }
                if (paths == null)
                {
                    Log.ErrorOnce(
                        "[Better Gene Graphics Framework] No valid graphicPaths found, check your settings!",
                        1110665686);
                    return BaseTexPathFor(pawn);
                }

                List<string> allowedPaths = new List<string>();
                // simply assume same count for all stages
                // can simply repeat if not the case
                int texCountPerRecordStage = paths.Count / cutoffs.Count;
                int record = pawn.records?.GetAsInt(extension.recordDef) ?? 0;
                // left inclusive
                int recordStage = cutoffs.FindLastIndex((x) => x <= record);
                if (recordStage == -1)
                {
                    Log.ErrorOnce(
                        "[Better Gene Graphics Framework] Pawn's record not in cutoffs, check your cutoffs settings!",
                        111031659);
                    return BaseTexPathFor(pawn);
                }
                for (int j = recordStage * texCountPerRecordStage;
                     j < recordStage * texCountPerRecordStage + texCountPerRecordStage;
                     j++)
                {
                    allowedPaths.Add(paths[j]);
                }
                if (allowedPaths.Count == 0)
                {
                    return BaseTexPathFor(pawn);
                }
                GameComponent_AlternateGraphics comp = Current.Game.GetComponent<GameComponent_AlternateGraphics>();
                if (comp != null)
                {
                    int ind = comp.GetFixedIndex(pawn, gene);
                    if (ind != -1)
                    {
                        return allowedPaths[ind % allowedPaths.Count];
                    }
                }
                using (new RandBlock(TexSeedFor(pawn)))
                {
                    return allowedPaths.RandomElement();
                }
            }
            return BaseTexPathFor(pawn);
        }
    }
}