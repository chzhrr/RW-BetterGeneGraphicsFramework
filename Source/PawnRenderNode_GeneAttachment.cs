using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace BetterGeneGraphicsFramework
{
    public class PawnRenderNode_GeneAttachment : PawnRenderNode
    {
        public PawnRenderNode_GeneAttachment(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) 
            : base(pawn, props, tree)
        {
        }

        public override Graphic GraphicFor(Pawn pawn)
        {
            string path = TexPathFor(pawn);
            if (path.NullOrEmpty())
            {
                return null;
            }
            Shader shader = ShaderFor(pawn);
            if (shader == null)
            {
                return null;
            }
            Color secondColor = GetSecondColor(pawn);
            if (secondColor != Color.white)
            {
                return GraphicDatabase.Get<Graphic_Multi>(path, shader, Vector2.one, ColorFor(pawn), secondColor);
            }
            return GraphicDatabase.Get<Graphic_Multi>(path, shader, Vector2.one, ColorFor(pawn));
        }

        private Color GetSecondColor(Pawn pawn)
        {
            Color color = Color.white;
            ShaderSupport extension = gene.def.GetModExtension<ShaderSupport>();
            if (extension != null)
            {
                if (!extension.colorTwoType.HasValue)
                {
                    return extension.colorTwo;
                }
                switch (extension.colorTwoType)
                {
                    case PawnRenderNodeProperties.AttachmentColorType.Hair:
                        if (pawn.story == null)
                        {
                            Log.ErrorOnce(
                                "Trying to set render node color to hair for " + pawn.LabelShort +
                                " without pawn story. Defaulting to white.",
                                Gen.HashCombine(pawn.thingIDNumber, 828310001));
                            color = Color.white;
                        }
                        else
                        {
                            color = pawn.story.HairColor;
                        }
                        break;
                    case PawnRenderNodeProperties.AttachmentColorType.Skin:
                        if (pawn.story == null)
                        {
                            Log.ErrorOnce(
                                "Trying to set render node color to skin for " + pawn.LabelShort +
                                " without pawn story. Defaulting to white.",
                                Gen.HashCombine(pawn.thingIDNumber, 228340903));
                            color = Color.white;
                        }
                        else
                        {
                            color = pawn.story.SkinColor;
                        }
                        break;
                    default:
                        color = Color.white;
                        break;
                }
            }
            return color;
        }

        protected override string TexPathFor(Pawn pawn)
        {
            GraphicsWithAge extension = gene.def.GetModExtension<GraphicsWithAge>();
            if (extension != null)
            {
                List<float> ages = extension.ages;
                List<string> bodyPartExpressions = extension.bodyPartExpressions;

                List<string> paths = null;
                if (!extension.bodyTypeGraphicPaths.NullOrEmpty())
                {
                    foreach (BodyTypeGraphicData bodyTypeGraphicPath in extension.bodyTypeGraphicPaths)
                    {
                        if (pawn.story.bodyType == bodyTypeGraphicPath.bodyType)
                        {
                            paths = new List<string>{bodyTypeGraphicPath.texturePath};
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
                    return base.TexPathFor(pawn);
                }

                List<string> allowedPaths = new List<string>();
                int texCountPerAgeStage = paths.Count / ages.Count;
                // left inclusive
                int ageStage = ages.FindLastIndex((x) => x <= pawn.ageTracker.AgeBiologicalYearsFloat);
                if (ageStage == -1)
                {
                    Log.ErrorOnce(
                        "[Better Gene Graphics Framework] Pawn's age is not in ages, check your ages settings!",
                        111031659);
                    return base.TexPathFor(pawn);
                }
                List<(string, string)> bodyPartHediffs = new List<(string, string)>();
                if (!bodyPartExpressions.NullOrEmpty())
                {
                    bodyPartHediffs = BodyPartHediffs();
                }
                for (int j = ageStage * texCountPerAgeStage;
                     j < ageStage * texCountPerAgeStage + texCountPerAgeStage;
                     j++)
                {
                    if (bodyPartExpressions.NullOrEmpty() ||
                        CheckExpressionForPawn(bodyPartExpressions[j % bodyPartExpressions.Count], bodyPartHediffs))
                    {
                        allowedPaths.Add(paths[j]);
                    }
                }
                if (allowedPaths.Count == 0)
                {
                    return base.TexPathFor(pawn);
                }
                using (new RandBlock(TexSeedFor(pawn)))
                {
                    return allowedPaths.RandomElement();
                }
            }

            return base.TexPathFor(pawn);

            List<(string, string)> BodyPartHediffs()
            {
                List<(string, string)> res = new List<(string, string)>();
                if (pawn.health?.hediffSet == null)
                    return res;
                res.AddRange(
                    pawn.health.hediffSet.hediffs.Select(x => (x.Part?.untranslatedCustomLabel, x.def?.defName)));
                return res;
            }
        }

        private bool CheckExpressionForPawn(string bodyPartExpression, List<(string, string)> bodyPartHediffs)
        {
            var bodyPartsRequests = bodyPartExpression.Split('|');
            if (bodyPartsRequests.Length == 0)
            {
                Log.ErrorOnce("[Better Gene Graphics Framework] Bad body part expression detected!", 195007889);
            }

            foreach (var request in bodyPartsRequests)
            {
                int dlmIndex = request.IndexOf("--");
                bool isMissingBodyPart = dlmIndex < 0;
                string hediff = isMissingBodyPart ? "MissingBodyPart" : request.Substring(dlmIndex + 3);
                string bodyPart = isMissingBodyPart
                    ? request.Replace("!", string.Empty)
                    : request.Substring(0, dlmIndex - 1).Replace("!", string.Empty);

                // hediff exists when !bodyPart (Missing hediff)
                // or Global-- hediff
                bool expectHediff = (isMissingBodyPart && request.StartsWith("!")) ||
                                    (!isMissingBodyPart && !request.StartsWith("!"));

                if (bodyPartHediffs.Any(x =>
                        (x.Item1?.Equals(bodyPart, System.StringComparison.OrdinalIgnoreCase) ??
                         bodyPart.Equals("Global", System.StringComparison.OrdinalIgnoreCase))
                        && x.Item2.Equals(hediff, System.StringComparison.OrdinalIgnoreCase))
                    != expectHediff)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
