using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using Verse;

// ReSharper disable UnusedMember.Local

// ReSharper disable UnusedMember.Global

namespace BetterGeneGraphicsFramework
{
    [UsedImplicitly]
    public static class DebugActions
    {
#if DEBUG
        public static float adjustment = 0.01f;

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns,
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void Set2XAdjustment(Pawn p)
        {
            adjustment = 0.02f;
            Log.Message($"Current adjustment is {adjustment}");
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns,
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void Set5XAdjustment(Pawn p)
        {
            adjustment = 0.05f;
            Log.Message($"Current adjustment is {adjustment}");
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns,
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void Set10XAdjustment(Pawn p)
        {
            adjustment = 0.1f;
            Log.Message($"Current adjustment is {adjustment}");
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void IncreaseXOffsetNorth(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetNorth").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetNorth").SetValue(new Vector3(v.x + adjustment, v.y, v.z));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void DecreaseXOffsetNorth(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetNorth").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetNorth").SetValue(new Vector3(v.x - adjustment, v.y, v.z));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void IncreaseYOffsetNorth(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetNorth").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetNorth").SetValue(new Vector3(v.x, v.y + adjustment, v.z));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void DecreaseYOffsetNorth(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetNorth").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetNorth").SetValue(new Vector3(v.x, v.y - adjustment, v.z));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void IncreaseZOffsetNorth(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetNorth").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetNorth").SetValue(new Vector3(v.x, v.y, v.z + adjustment));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void DecreaseZOffsetNorth(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetNorth").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetNorth").SetValue(new Vector3(v.x, v.y, v.z - adjustment));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void LogCurrentOffsetNorth(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    BetterTailRendering extension = geneGraphic.sourceGene.def.GetModExtension<BetterTailRendering>();
                    if (extension != null)
                    {
                        if (p.Rotation == Rot4.North)
                        {
                            Vector3 bodyTypeOffset =
                                Patch_BetterTailRendering.GetBodyTypeOffsetWithFacing(p, p.Rotation, extension);
                            Log.Message($"Current x body type ({p.story.bodyType.defName}) offset for north texture is {bodyTypeOffset.x}.");
                            Log.Message($"Current y body type ({p.story.bodyType.defName}) offset for north texture is {bodyTypeOffset.y}.");
                            Log.Message($"Current z body type ({p.story.bodyType.defName}) offset for north texture is {bodyTypeOffset.z}.");
                        }
                    }
                    if (p.Rotation == Rot4.North)
                    {
                        Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetNorth").GetValue();
                        Log.Message($"Current x offset for north texture is {v.x}.");
                        Log.Message($"Current y offset for north texture is {v.y}.");
                        Log.Message($"Current z offset for north texture is {v.z}.");
                        Log.Message("Exposing current offsets to clipboard.");
                        TextEditor te = new TextEditor();
                        te.text = $"({v.x}, {v.y}, {v.z})";
                        te.SelectAll();
                        te.Copy();
                    }
                    else
                    {
                        Log.Message("It makes no sense to log offset for north texture when pawn is not facing north");
                    }
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void IncreaseXOffsetEast(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetEast").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetEast").SetValue(new Vector3(v.x + adjustment, v.y, v.z));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void DecreaseXOffsetEast(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetEast").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetEast").SetValue(new Vector3(v.x - adjustment, v.y, v.z));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void IncreaseYOffsetEast(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetEast").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetEast").SetValue(new Vector3(v.x, v.y + adjustment, v.z));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void DecreaseYOffsetEast(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetEast").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetEast").SetValue(new Vector3(v.x, v.y - adjustment, v.z));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void IncreaseZOffsetEast(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetEast").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetEast").SetValue(new Vector3(v.x, v.y, v.z + adjustment));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void DecreaseZOffsetEast(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetEast").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetEast").SetValue(new Vector3(v.x, v.y, v.z - adjustment));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void LogCurrentOffsetEast(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    BetterTailRendering extension = geneGraphic.sourceGene.def.GetModExtension<BetterTailRendering>();
                    if (extension != null)
                    {
                        if (p.Rotation == Rot4.East)
                        {
                            Vector3 bodyTypeOffset =
                                Patch_BetterTailRendering.GetBodyTypeOffsetWithFacing(p, p.Rotation, extension);
                            Log.Message($"Current x body type ({p.story.bodyType.defName}) offset for east texture is {bodyTypeOffset.x}.");
                            Log.Message($"Current y body type ({p.story.bodyType.defName}) offset for east texture is {bodyTypeOffset.y}.");
                            Log.Message($"Current z body type ({p.story.bodyType.defName}) offset for east texture is {bodyTypeOffset.z}.");
                        }
                    }
                    if (p.Rotation == Rot4.East)
                    {
                        Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetEast").GetValue();
                        Log.Message($"Current x offset for east texture is {v.x}.");
                        Log.Message($"Current y offset for east texture is {v.y}.");
                        Log.Message($"Current z offset for east texture is {v.z}.");
                        Log.Message("Exposing current offsets to clipboard");
                        TextEditor te = new TextEditor();
                        te.text = $"({v.x}, {v.y}, {v.z})";
                        te.SelectAll();
                        te.Copy();
                    }
                    else
                    {
                        Log.Message("It makes no sense to log offset for east texture when pawn is not facing east");
                    }
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void IncreaseXOffsetSouth(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetSouth").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetSouth").SetValue(new Vector3(v.x + adjustment, v.y, v.z));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void DecreaseXOffsetSouth(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetSouth").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetSouth").SetValue(new Vector3(v.x - adjustment, v.y, v.z));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void IncreaseYOffsetSouth(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetSouth").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetSouth").SetValue(new Vector3(v.x, v.y + adjustment, v.z));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void DecreaseYOffsetSouth(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetSouth").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetSouth").SetValue(new Vector3(v.x, v.y - adjustment, v.z));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void IncreaseZOffsetSouth(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetSouth").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetSouth").SetValue(new Vector3(v.x, v.y, v.z + adjustment));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void DecreaseZOffsetSouth(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetSouth").GetValue();
                    Traverse.Create(graphicData).Field("drawOffsetSouth").SetValue(new Vector3(v.x, v.y, v.z - adjustment));
                }
            }
        }

        [DebugAction("PawnAttachmentsRendering", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void LogCurrentOffsetSouth(Pawn p)
        {
            foreach (GeneGraphicRecord geneGraphic in p.Drawer.renderer.graphics.geneGraphics)
            {
                GeneGraphicData graphicData = geneGraphic.sourceGene.def.graphicData;
                if (graphicData.drawLoc == GeneDrawLoc.Tailbone)
                {
                    BetterTailRendering extension = geneGraphic.sourceGene.def.GetModExtension<BetterTailRendering>();
                    if (extension != null)
                    {
                        if (p.Rotation == Rot4.South)
                        {
                            Vector3 bodyTypeOffset =
                                Patch_BetterTailRendering.GetBodyTypeOffsetWithFacing(p, p.Rotation, extension);
                            Log.Message($"Current x body type ({p.story.bodyType.defName}) offset for south texture is {bodyTypeOffset.x}.");
                            Log.Message($"Current y body type ({p.story.bodyType.defName}) offset for south texture is {bodyTypeOffset.y}.");
                            Log.Message($"Current z body type ({p.story.bodyType.defName}) offset for south texture is {bodyTypeOffset.z}.");
                        }
                    }
                    if (p.Rotation == Rot4.South)
                    {
                        Vector3 v = (Vector3)Traverse.Create(graphicData).Field("drawOffsetSouth").GetValue();
                        Log.Message($"Current x offset for south texture is {v.x}.");
                        Log.Message($"Current y offset for south texture is {v.y}.");
                        Log.Message($"Current z offset for south texture is {v.z}.");
                        Log.Message("Exposing current offsets to clipboard.");
                        TextEditor te = new TextEditor();
                        te.text = $"({v.x}, {v.y}, {v.z})";
                        te.SelectAll();
                        te.Copy();
                    }
                    else
                    {
                        Log.Message("It makes no sense to log offset for south texture when pawn is not facing south");
                    }
                }
            }
        }
    }

#endif
}