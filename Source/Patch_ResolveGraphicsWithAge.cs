using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using Verse;

// ReSharper disable UnusedMember.Global
// ReSharper disable once ClassNeverInstantiated.Global

namespace BetterGeneGraphicsFramework
{
    [HarmonyPatch(typeof(PawnGraphicSet), "ResolveGeneGraphics")]
    [UsedImplicitly]
    public static class Patch_ResolveGraphicsWithAge
    {
        /// <summary>
        /// Too hard to explain.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> ResolveGraphicsWithAge(IEnumerable<CodeInstruction> instructions,
            ILGenerator generator)
        {
            // Task: Replace
            //if (item.def.HasGraphic && item.Active)
            //{
            //    (Graphic, Graphic) graphics = item.def.graphicData.GetGraphics(pawn, skinShader, rottingColor);
            //    geneGraphics.Add(new GeneGraphicRecord(graphics.Item1, graphics.Item2, item));
            //}
            // With
            //if (item.def.HasGraphic && item.Active)
            //{
            //    (Graphic, Graphic) graphics =
            //        ((!Patch_ResolveGraphicsWithAge.HasGraphicsWithAgeModDefExtension(item.def) ?
            //            item.def.graphicData.GetGraphics(pawn, skinShader, rottingColor) :
            //            Patch_ResolveGraphicsWithAge.GetGraphicsWithAge(pawn, item, skinShader, rottingColor));
            //    geneGraphics.Add(new GeneGraphicRecord(graphics.Item1, graphics.Item2, item));
            //}

            MethodInfo geneActiveGetter = AccessTools.DeclaredPropertyGetter(typeof(Gene), nameof(Gene.Active));
            MethodInfo hasOurExtensionMI =
                typeof(Patch_ResolveGraphicsWithAge).GetMethod("HasGraphicsWithAgeModDefExtension");
            MethodInfo ourResolveGraphicsMI =
                typeof(Patch_ResolveGraphicsWithAge).GetMethod("GetGraphicsWithAge");
            List<CodeInstruction> codes = instructions.ToList();
            Label falseTransferLabel = generator.DefineLabel();
            Label trueTransferLabel = generator.DefineLabel();
            int index = -99;
            for (int i = 0; i < codes.Count; i++)
            {
                if (i != index)
                {
                    yield return codes[i];
                }
                else
                {
                    // add a nop instruction with label to transfer to
                    yield return new CodeInstruction(OpCodes.Nop).WithLabels(trueTransferLabel);
                    // geneGraphics.Add(new GeneGraphicRecord(graphics.Item1, graphics.Item2, item));
                    // IL_00b9: ldarg.0
                    yield return codes[i];
                }
                if (i - 1 >= 0 && codes[i - 1].Calls(geneActiveGetter))
                {
                    // item
                    yield return new CodeInstruction(OpCodes.Ldloc_3);
                    // item.def
                    yield return CodeInstruction.LoadField(typeof(Gene), nameof(Gene.def));
                    // Patch_ResolveGraphicsWithAge.HasGraphicsWithAgeModDefExtension(item.def)
                    yield return new CodeInstruction(OpCodes.Call, hasOurExtensionMI);
                    // if false, transfer to vanilla method
                    yield return new CodeInstruction(OpCodes.Brfalse, falseTransferLabel);
                    // otherwise use our resolve method
                    // this
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    // this.pawn
                    yield return CodeInstruction.LoadField(typeof(PawnGraphicSet), nameof(PawnGraphicSet.pawn));
                    // item
                    yield return new CodeInstruction(OpCodes.Ldloc_3);
                    // skinShader
                    yield return new CodeInstruction(OpCodes.Ldloc_1);
                    // rottingColor
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    // Patch_ResolveGraphicsWithAge.GetGraphicsWithAge(Pawn pawn, Gene gene, Shader skinShader, Color rottingColor)
                    yield return new CodeInstruction(OpCodes.Call, ourResolveGraphicsMI);
                    // (Graphic, Graphic) graphics =
                    yield return new CodeInstruction(OpCodes.Stloc_S, 4);
                    yield return new CodeInstruction(OpCodes.Nop);
                    // since we have resolve the path with our method, we need to skip the vanilla method and jump to our trueTransferLabel
                    yield return new CodeInstruction(OpCodes.Br_S, trueTransferLabel);
                    // count from il, we need to skip 10 il instructions
                    index = i + 10;
                    // add a new nop instruction so that if the def don't have extension it will use vanilla resolve method
                    yield return new CodeInstruction(OpCodes.Nop).WithLabels(falseTransferLabel);
                }
            }
        }

        public static bool HasGraphicsWithAgeModDefExtension(GeneDef def)
        {
            GraphicsWithAge extension = def.GetModExtension<GraphicsWithAge>();
            return extension != null && (extension.graphicPaths != null || extension.graphicPathsFemale != null);
        }

        /// <summary>
        /// Revised from GeneGraphicData.GetGraphics().
        /// </summary>
        public static (Graphic, Graphic) GetGraphicsWithAge(Pawn pawn, Gene gene, Shader skinShader, Color rottingColor)
        {
            Shader shader = gene.def.graphicData.useSkinShader ? skinShader : ShaderDatabase.Transparent;
            string path = GraphicPathFor();
            Graphic item = GraphicDatabase.Get<Graphic_Multi>(color: GetColorFor(), path: path, shader: shader,
                drawSize: Vector2.one, colorTwo: Color.white);
            Graphic item2 = GraphicDatabase.Get<Graphic_Multi>(path, shader, Vector2.one,
                gene.def.graphicData.color ?? rottingColor, Color.white);
            return (item, item2);

            string GraphicPathFor()
            {
                GraphicsWithAge extension = gene.def.GetModExtension<GraphicsWithAge>();
                List<float> ages = extension.ages;
                List<string> paths;
                if (pawn.gender == Gender.Female && extension.graphicPathsFemale != null)
                {
                    paths = extension.graphicPathsFemale;
                }
                else
                {
                    paths = extension.graphicPaths;
                }
                List<string> allowedPaths = new List<string>();
                int texCountPerAgeStage = paths.Count / ages.Count;
                int ageStage = ages.FindLastIndex((x) => x < pawn.ageTracker.AgeBiologicalYearsFloat);
                for (int j = ageStage * texCountPerAgeStage; j < ageStage * texCountPerAgeStage + texCountPerAgeStage; j++)
                {
                    allowedPaths.Add(paths[j]);
                }
                return allowedPaths[pawn.thingIDNumber % allowedPaths.Count];
            }

            Color GetColorFor()
            {
                Color a;
                switch (gene.def.graphicData.colorType)
                {
                    case GeneColorType.Hair:
                        a = pawn.story.HairColor;
                        break;

                    case GeneColorType.Skin:
                        a = pawn.story.SkinColor;
                        break;

                    default:
                        a = (gene.def.graphicData.color ?? Color.white);
                        break;
                }

                return a * gene.def.graphicData.colorRGBPostFactor;
            }
        }
    }
}