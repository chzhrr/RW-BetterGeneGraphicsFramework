using RimWorld;
using UnityEngine;
using Verse;

namespace BetterGeneGraphicsFramework
{
    public class PawnRenderNodeWorker_GeneAttachment : PawnRenderNodeWorker
    {
        public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
        {
            Vector3 anchorOffset = Vector3.zero;
            // pivot = PivotFor(node, parms);
            WithBodyTypeOffset extension = node.gene?.def.GetModExtension<WithBodyTypeOffset>();
            if (node.Props.drawData != null)
            {
                Vector3 vector = node.Props.drawData.OffsetForRot(parms.facing);
                Vector3 bodyTypeOffset = Vector3.zero;
                if (node.Props.drawData.scaleOffsetByBodySize && parms.pawn.story != null)
                {
                    Vector2 bodyGraphicScale = parms.pawn.story.bodyType.bodyGraphicScale;
                    float num = (bodyGraphicScale.x + bodyGraphicScale.y) / 2f;
                    if (extension != null)
                    {
                        bool hasBabyOrChildBodyType = PawnHasBabyOrChildBodyType(parms.pawn);
                        if ((extension.disableBodyTypeScaleForAdult && !hasBabyOrChildBodyType)
                            || (extension.disableBodyTypeScaleForChildren && hasBabyOrChildBodyType))
                        {
                            // Don't change bodyGraphicScale, it's used to scale the root location
                            // Scaling is passed to new Vector3(graphicData.drawScale * num, 1f, graphicData.drawScale * num)
                            // Where float num = (bodyGraphicScale.x + bodyGraphicScale.y) / 2f;
                            num = 1;
                        }
                    }
                    vector *= num;
                }
                if (extension != null)
                {
                    bodyTypeOffset = GetBodyTypeOffsetWithFacing(parms.pawn, parms.facing, extension);
                }
                anchorOffset += vector + bodyTypeOffset;
            }
            anchorOffset += node.DebugOffset;
            pivot = anchorOffset;
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
