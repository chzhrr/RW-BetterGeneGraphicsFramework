using RimWorld;
using UnityEngine;
using Verse;

namespace BetterGeneGraphicsFramework
{
    public class PawnRenderNodeWorker_GeneBodyAttachment : PawnRenderNodeWorker_Body
    {
        public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
        {
            Vector3 anchorOffset = Vector3.zero;
            // for rotation
            pivot = PivotFor(node, parms);
            WithBodyTypeOffset extension = node.gene?.def.GetModExtension<WithBodyTypeOffset>();
            Vector3 bodyTypeOffset = Vector3.zero;
            Vector3 rotOffset = Vector3.zero;
            if (node.Props.drawData != null)
            {
                rotOffset = node.Props.drawData.OffsetForRot(parms.facing);
               
                if (node.Props.drawData.scaleOffsetByBodySize && parms.pawn.story != null)
                {
                    Vector2 bodyGraphicScale = parms.pawn.story.bodyType.bodyGraphicScale;
                    float scale = (bodyGraphicScale.x + bodyGraphicScale.y) / 2f;
                    if (extension != null)
                    {
                        bool hasBabyOrChildBodyType = PawnHasBabyOrChildBodyType(parms.pawn);
                        if ((extension.disableBodyTypeScaleForAdult && !hasBabyOrChildBodyType)
                            || (extension.disableBodyTypeScaleForChildren && hasBabyOrChildBodyType))
                        {
                            scale = 1;
                        }
                    }
                    rotOffset *= scale;
                }
            }
            if (extension != null)
            {
                // for babies, parms.facing is LayingFacing()
                Rot4 facing = parms.facing;
                // fix offset for portrait which always use South
                if (parms.Portrait && parms.pawn.DevelopmentalStage.Baby())
                {
                    facing = Rot4.South;
                }
                bodyTypeOffset = GetBodyTypeOffsetWithFacing(parms.pawn, facing, extension);
            }
            pivot += bodyTypeOffset;
            anchorOffset += rotOffset + bodyTypeOffset + node.DebugOffset;
            
            if (node.AnimationWorker != null && node.AnimationWorker.Enabled() && !parms.flags.FlagSet(PawnRenderFlags.Portrait))
            {
                anchorOffset += node.AnimationWorker.OffsetAtTick(node.tree.AnimationTick, parms);
            }
            return anchorOffset;

            bool PawnHasBabyOrChildBodyType(Pawn pawn)
            {
                if (pawn.story?.bodyType != null)
                {
                    return pawn.story.bodyType == BodyTypeDefOf.Baby || pawn.story.bodyType == BodyTypeDefOf.Child;
                }
                return false;
            }
        }

        public Vector3 GetBodyTypeOffsetWithFacing(Pawn pawn, Rot4 bodyFacing, WithBodyTypeOffset extension)
        {
            switch (bodyFacing.AsInt)
            {
                // North
                case 0:
                    {
                        if (extension.bodyTypeOffsetNorth != null)
                        {
                            return GetBodyTypeOffset(pawn, extension.bodyTypeOffsetNorth);
                        }

                        return Vector3.zero;
                    }

                case 1:
                    {
                        if (extension.bodyTypeOffsetEast != null)
                        {
                            return GetBodyTypeOffset(pawn, extension.bodyTypeOffsetEast);
                        }

                        return Vector3.zero;
                    }

                case 2:
                    {
                        if (extension.bodyTypeOffsetSouth != null)
                        {
                            return GetBodyTypeOffset(pawn, extension.bodyTypeOffsetSouth);
                        }

                        return Vector3.zero;
                    }

                case 3:
                    {
                        if (extension.bodyTypeOffsetEast != null)
                        {
                            Vector3 result = GetBodyTypeOffset(pawn, extension.bodyTypeOffsetEast);
                            result.x *= -1f;
                            return result;
                        }

                        return Vector3.zero;
                    }
                default:
                    return Vector3.zero;
            }
        }

        public static Vector3 GetBodyTypeOffset(Pawn pawn, BodyTypeOffset bodyTypeOffset)
        {
            if (pawn.story != null)
            {
                if (pawn.story.bodyType == BodyTypeDefOf.Male)
                {
                    return bodyTypeOffset.male ?? Vector3.zero;
                }
                if (pawn.story.bodyType == BodyTypeDefOf.Female)
                {
                    return bodyTypeOffset.female ?? Vector3.zero;
                }
                if (pawn.story.bodyType == BodyTypeDefOf.Fat)
                {
                    return bodyTypeOffset.fat ?? Vector3.zero;
                }
                if (pawn.story.bodyType == BodyTypeDefOf.Hulk)
                {
                    return bodyTypeOffset.hulk ?? Vector3.zero;
                }
                if (pawn.story.bodyType == BodyTypeDefOf.Thin)
                {
                    return bodyTypeOffset.thin ?? Vector3.zero;
                }
                if (pawn.story.bodyType == BodyTypeDefOf.Baby)
                {
                    return bodyTypeOffset.baby ?? Vector3.zero;
                }
                if (pawn.story.bodyType == BodyTypeDefOf.Child)
                {
                    return bodyTypeOffset.child ?? Vector3.zero;
                }
                if (pawn.story.bodyType == FemaleBBBodyTypeSupport.FemaleBodyTypeDef)
                {
                    return bodyTypeOffset.femaleBB ?? Vector3.zero;
                }
            }
            return Vector3.zero;
        }
    }
}
