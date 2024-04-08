using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace BetterGeneGraphicsFramework
{
    public class Dialog_AlternateGraphics : Window
    {
        private GameComponent_AlternateGraphics comp;
        
        private Pawn pawn;
        private List<Gene> genes = new List<Gene>();

        private Gene currentGene;
        private int currentIndex;
        private string currentIndexBuffer;

        private Vector2 scrollPosition;
        private float scrollHeight;

        public Dialog_AlternateGraphics(Pawn pawn)
        {
            doCloseX = true;
            draggable = true;
            Init(pawn);
            this.comp = Current.Game.GetComponent<GameComponent_AlternateGraphics>();
        }

        private void Init(Pawn pawn)
        {
            this.pawn = pawn;
            this.genes.Clear();
            currentGene = null;
            currentIndex = 0;
            scrollHeight = 0;
            scrollPosition = Vector2.zero;
            foreach (var gene in pawn.genes.GenesListForReading)
            {
                if (gene.def.RenderNodeProperties != null)
                {
                    foreach (var prop in gene.def.RenderNodeProperties)
                    {
                        if (typeof(PawnRenderNode_GeneAttachment).IsAssignableFrom(prop.nodeClass))
                        {
                            genes.Add(gene);
                        }
                    }
                }
            }
            optionalTitle = pawn.LabelShortCap;
        }

        public override void WindowUpdate()
        {
            base.WindowUpdate();
            Pawn pawn = Find.Selector.SelectedPawns.FirstOrDefault();
            if (pawn != null && pawn != this.pawn)
            {
                Init(pawn);
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;
            LeftRect(inRect.LeftHalf());
            RightRect(inRect.RightHalf());
        }

        private void LeftRect(Rect inRect)
        {
            if (pawn == null || comp == null)
            {
                Close();
                return;
            }
            Widgets.DrawMenuSection(inRect);
            inRect = inRect.ContractedBy(Margin / 2f);
            Widgets.BeginScrollView(inRect, ref scrollPosition, new Rect(0f, 0f, inRect.width - 16f, scrollHeight));
            float curY = 0f;
            foreach (var gene in genes)
            {
                ListGene(gene, 0, ref curY, inRect.width - 16f);
            }
            if (Event.current.type == EventType.Layout)
            {
                scrollHeight = curY;
            }
            Widgets.EndScrollView();
        }

        private void ListGene(Gene gene, int indent, ref float curY, float width)
        {
            Rect rowRect = new Rect((indent + 1) * 12f, curY, width, 25f);
            Widgets.Label(rowRect, gene.LabelCap.Truncate(rowRect.width));
            GUI.color = Color.white;
            if (Widgets.ButtonInvisible(rowRect))
            {
                currentIndex = comp.GetFixedIndex(pawn, gene);
                currentIndexBuffer = currentIndex.ToString();
                currentGene = gene;
            }
            Widgets.DrawHighlight(rowRect);
            Widgets.DrawHighlightIfMouseover(rowRect);
            curY += 29f;
        }

        private void RightRect(Rect inRect)
        {
            if (currentGene == null)
            {
                return;
            }
            Widgets.DrawMenuSection(inRect);
            inRect = inRect.ContractedBy(Margin / 2f);
            Rect labelRect = new Rect(inRect.x, inRect.y, inRect.width, 25f);
            Widgets.Label(labelRect, currentGene.def.defName);
            Rect inputRect = new Rect(inRect.x, inRect.y + 35f, inRect.width, 25f);
            Widgets.TextFieldNumericLabeled(inputRect, "Index", ref currentIndex, ref currentIndexBuffer);
            Rect buttonRect = new Rect(inRect.x, inRect.y + 70f, 35f, 25f);
            if (Widgets.ButtonText(buttonRect, "Save"))
            {
                comp.RegisterFixedIndex(pawn, currentGene, currentIndex);
                pawn.Drawer.renderer.SetAllGraphicsDirty();
            }
        }
    }
}
