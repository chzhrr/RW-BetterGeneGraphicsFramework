using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using UnityEngine;
using Verse;

// ReSharper disable UnusedMember.Global
// ReSharper disable once ClassNeverInstantiated.Global

namespace BetterGeneGraphicsFramework
{
    [HarmonyPatch(typeof(PawnRenderer), "DrawBodyGenes")]
    [UsedImplicitly]
    public static class Patch_BetterTailRendering
    {
        [HarmonyPriority(1000)]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RemoveBodyTypeScaleAndAddBodyTypeOffset(
            IEnumerable<CodeInstruction> instructions)
        {
            /* Task: 
             * Insert
             * PostProcess(GeneDef geneDef, Pawn pawn, Rot4 bodyFacing, ref float num, ref Vector2 bodyGraphicScale, ref Vector3 v)
             *
             * Between
             * Vector3 v = graphicData.DrawOffsetAt(bodyFacing);
             *
             * And
             * v.x *= bodyGraphicScale.x;
             * v.z *= bodyGraphicScale.y;
             */
            MethodInfo drawOffsetAtMI = typeof(GeneGraphicData).GetMethod(nameof(GeneGraphicData.DrawOffsetAt));
            MethodInfo postProcessMI =
                typeof(Patch_BetterTailRendering).GetMethod(nameof(Patch_BetterTailRendering.PostProcess));
            List<CodeInstruction> codes = instructions.ToList();
            int index = codes.FindIndex((x) => x.Calls(drawOffsetAtMI));
            codes.InsertRange(index + 2, new List<CodeInstruction>()
            {
                // geneGraphic.sourceGene.def
                new CodeInstruction(OpCodes.Ldloc_3),
                CodeInstruction.LoadField(typeof(GeneGraphicRecord), nameof(GeneGraphicRecord.sourceGene)),
                CodeInstruction.LoadField(typeof(Gene), nameof(Gene.def)),
                // pawn
                new CodeInstruction(OpCodes.Ldarg_0),
                CodeInstruction.LoadField(typeof(PawnRenderer), "pawn"),
                // bodyFacing
                new CodeInstruction(OpCodes.Ldarg_S, 4),
                // num, scale
                new CodeInstruction(OpCodes.Ldloca, 1),
                // bodyGraphicScale
                new CodeInstruction(OpCodes.Ldloca, 0),
                // v, offsets
                new CodeInstruction(OpCodes.Ldloca_S, 5),
                new CodeInstruction(OpCodes.Call, postProcessMI)
            });
            return codes;
        }

        public static void PostProcess(GeneDef geneDef, Pawn pawn, Rot4 bodyFacing, ref float num,
            ref Vector2 bodyGraphicScale, ref Vector3 v)
        {
            BetterTailRendering extension = geneDef.GetModExtension<BetterTailRendering>();
            if (extension != null)
            {
                if ((extension.disableBodyTypeScaleForAdult && !PawnHasBabyOrChildBodyType())
                    || (extension.disableBodyTypeScaleForChildren && PawnHasBabyOrChildBodyType()))
                {
                    bodyGraphicScale = Vector2.one;
                    num = 1;
                }
                var bodyTypeOffset = GetBodyTypeOffsetWithFacing(pawn, bodyFacing, extension);
                v += bodyTypeOffset;
            }

            bool PawnHasBabyOrChildBodyType()
            {
                if (pawn.story?.bodyType != null)
                {
                    return pawn.story.bodyType == BodyTypeDefOf.Baby || pawn.story.bodyType == BodyTypeDefOf.Child;
                }
                return false;
            }
        }

        public static Vector3 GetBodyTypeOffsetWithFacing(Pawn pawn, Rot4 bodyFacing, BetterTailRendering extension)
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