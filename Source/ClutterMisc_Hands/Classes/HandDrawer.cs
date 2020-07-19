using UnityEngine;
using Verse;

namespace WHands
{
    [StaticConstructorOnStartup]
    public class HandDrawer : ThingComp
    {
        public Graphic HandTex;

        public int PrimaryID;

        public Vector3 FHand;

        public Vector3 SHand;

        public bool TwoHand = true;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
        }

        public void ReadXML()
        {
            WhandCompProps whandCompProps = (WhandCompProps)this.props;
            if (whandCompProps.MainHand != Vector3.zero)
            {
                this.FHand = whandCompProps.MainHand;
            }
            if (whandCompProps.SecHand != Vector3.zero)
            {
                this.SHand = whandCompProps.SecHand;
            }
        }

        private bool CarryWeaponOpenly(Pawn pawn)
        {
            return (pawn.carryTracker == null || pawn.carryTracker.CarriedThing == null) && (pawn.Drafted || (pawn.CurJob != null && pawn.CurJob.def.alwaysShowWeapon) || (pawn.mindState.duty != null && pawn.mindState.duty.def.alwaysShowWeapon));
        }

        private void AngleCalc(Vector3 rootLoc)
        {
            Pawn pawn = this.parent as Pawn;
            if (!pawn.Dead && pawn.Spawned)
            {
                if (pawn.equipment != null && pawn.equipment.Primary != null)
                {
                    if (pawn.CurJob == null || !pawn.CurJob.def.neverShowWeapon)
                    {
                        WhandCompProps compProperties = pawn.equipment.Primary.def.GetCompProperties<WhandCompProps>();
                        if (compProperties != null)
                        {
                            this.FHand = compProperties.MainHand;
                            this.SHand = compProperties.SecHand;
                        }
                        else
                        {
                            this.SHand = Vector3.zero;
                            this.FHand = Vector3.zero;
                        }
                        rootLoc.y += 0.0449999981f;
                        Stance_Busy stance_Busy = pawn.stances.curStance as Stance_Busy;
                        if (stance_Busy != null && !stance_Busy.neverAimWeapon && stance_Busy.focusTarg.IsValid)
                        {
                            Vector3 a;
                            if (stance_Busy.focusTarg.HasThing)
                            {
                                a = stance_Busy.focusTarg.Thing.DrawPos;
                            }
                            else
                            {
                                a = stance_Busy.focusTarg.Cell.ToVector3Shifted();
                            }
                            float num = 0f;
                            if (GenGeo.MagnitudeHorizontalSquared(a - pawn.DrawPos) > 0.001f)
                            {
                                num = Vector3Utility.AngleFlat(a - pawn.DrawPos);
                            }
                            Vector3 b = Vector3Utility.RotatedBy(new Vector3(0f, 0f, 0.4f), num);
                            this.DrawHands(pawn.equipment.Primary, rootLoc + b, num);
                        }
                        else if (this.CarryWeaponOpenly(pawn))
                        {
                            if (pawn.Rotation == Rot4.South)
                            {
                                Vector3 drawLoc = rootLoc + new Vector3(0f, 0f, -0.22f);
                                this.DrawHands(pawn.equipment.Primary, drawLoc, 143f);
                            }
                            else if (pawn.Rotation == Rot4.East)
                            {
                                Vector3 drawLoc2 = rootLoc + new Vector3(0.2f, 0f, -0.22f);
                                this.DrawHands(pawn.equipment.Primary, drawLoc2, 143f);
                            }
                            else if (pawn.Rotation == Rot4.West)
                            {
                                Vector3 drawLoc3 = rootLoc + new Vector3(-0.2f, 0f, -0.22f);
                                this.DrawHands(pawn.equipment.Primary, drawLoc3, 217f);
                            }
                        }
                        else
                        {
                            GunDraver(pawn.equipment.Primary, pawn.DrawPos, pawn);
                        }
                    }
                }
            }
        }
        public void GunDraver(Thing eq, Vector3 drawLoc, Pawn pawn)
        {
            if (eq != null && eq.def.defName == "Gun_Pistol")
            {
                Vector3 WepHolderPos = new Vector3(0, 5f, 0);
                Matrix4x4 matrix = default(Matrix4x4);
                Vector3 size = new Vector3(0.84f, 0f, 0.84f);
                Mesh mesh = MeshPool.plane10;
                float num = 90;

                if (pawn.Rotation == Rot4.South)
                {
                    WepHolderPos = new Vector3(0.3f, 5f, -0.3f);
                    num = 270f;
                }
                else if (pawn.Rotation == Rot4.East)
                {
                    WepHolderPos = new Vector3(0, 5f, -0.3f);
                }
                else if (pawn.Rotation == Rot4.West)
                {
                    WepHolderPos = new Vector3(0, 0f, -0.3f);
                    num = 270f;
                }
                else if (pawn.Rotation == Rot4.North)
                {
                    WepHolderPos = new Vector3(-0.3f, 0f, -0.3f);
                    num = 75f;
                }
                matrix.SetTRS(drawLoc + WepHolderPos, Quaternion.AngleAxis(num, Vector3.up), size);
                Graphics.DrawMesh(mesh, matrix, eq.Graphic.MatSingle, 0);
            }
        }

        public void DrawHands(Thing eq, Vector3 drawLoc, float aimAngle)
        {
            bool flag = false;
            Pawn pawn = this.parent as Pawn;
            float num = aimAngle - 90f;
            if (aimAngle > 20f && aimAngle < 160f)
            {
                Mesh mesh = MeshPool.plane10;
                num += eq.def.equippedAngleOffset;
            }
            else if (aimAngle > 200f && aimAngle < 340f)
            {
                Mesh mesh = MeshPool.plane10Flip;
                num -= 180f;
                num -= eq.def.equippedAngleOffset;
                flag = true;
            }
            else
            {
                Mesh mesh = MeshPool.plane10;
                num += eq.def.equippedAngleOffset;
            }
            num %= 360f;
            if (this.HandTex != null)
            {
                Material matSingle = this.HandTex.MatSingle;
                matSingle.color = pawn.story.SkinColor;
                if (matSingle != null)
                {
                    matSingle.color = pawn.story.SkinColor;
                    if (this.FHand != Vector3.zero)
                    {
                        float num2 = this.FHand.x;
                        float z = this.FHand.z;
                        float y = this.FHand.y;
                        if (flag)
                        {
                            num2 = -num2;
                        }
                        Graphics.DrawMesh(MeshPool.plane10, drawLoc + Vector3Utility.RotatedBy(new Vector3(num2, y, z), num), Quaternion.AngleAxis(num, Vector3.up), matSingle, 0);
                    }
                    if (this.SHand != Vector3.zero)
                    {
                        float num3 = this.SHand.x;
                        float z2 = this.SHand.z;
                        float y2 = this.SHand.y;
                        if (flag)
                        {
                            num3 = -num3;
                        }
                        Graphics.DrawMesh(MeshPool.plane10, drawLoc + Vector3Utility.RotatedBy(new Vector3(num3, y2, z2), num), Quaternion.AngleAxis(num, Vector3.up), matSingle, 0);
                    }
                }
            }
            else if (this.HandTex == null)
            {
                this.HandTex = GraphicDatabase.Get<Graphic_Single>("Hand", ShaderDatabase.CutoutSkin, new Vector2(1f, 1f), pawn.story.SkinColor, pawn.story.SkinColor);
            }
        }

        public override void PostDraw()
        {
            if (this.HandTex != null)
            {
                this.AngleCalc(this.parent.DrawPos);
            }
            else
            {
                Pawn pawn = this.parent as Pawn;
                this.HandTex = GraphicDatabase.Get<Graphic_Single>("Hand", ShaderDatabase.CutoutSkin, new Vector2(1f, 1f), pawn.story.SkinColor, pawn.story.SkinColor);
            }
        }
    }
}
