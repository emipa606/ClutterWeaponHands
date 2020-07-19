using System.Linq;
using Verse;
using RimWorld;
using UnityEngine;

namespace WHands
{
    public class DefLoader_Extension : Def
    {

        public override void ResolveReferences()
        {
            base.ResolveReferences();
            CompLoader();
            WeaponComps();


        }


        public void CompLoader()
        {
            ThingDef Tdef = ThingDefOf.Human;
            if (Tdef != null)
            {
                CompProperties Compie = new CompProperties();
                Compie.compClass = typeof(HandDrawer);
                Tdef.comps.Add(Compie);
            }

        }

        public bool HandCheck()
        {
            return ModsConfig.ActiveModsInLoadOrder.Any(mod => mod.Name == "Clutter Laser Rifle");
        }

        public void LaserLoad()
        {
            if (HandCheck())
            {
                ThingDef wepzie = ThingDef.Named("LaserRifle");
                if (wepzie != null)
                {
                    WHands.WhandCompProps Compie = new WHands.WhandCompProps();
                    Compie.compClass = typeof(WHands.WhandComp);
                    Compie.MainHand = new Vector3(-0.2f, 0.3f, -0.05f);
                    Compie.SecHand = new Vector3(0.25f, 0f, -0.05f);
                    wepzie.comps.Add(Compie);
                }
            }
        }

        public void WeaponComps()
        {
            ThingDef Tdef = ThingDef.Named("ClutterHandsSettings");
            if (Tdef != null)
            {
                ClutterHandsTDef tDef = Tdef as ClutterHandsTDef;
                if (tDef != null)
                {
                    if (tDef.WeaponCompLoader.Count > 0)
                    {

                        foreach (ClutterHandsTDef.CompTargets WepSets in tDef.WeaponCompLoader)
                        {
                            if (WepSets.ThingTargets.Count > 0)
                            {
                                foreach (string t in WepSets.ThingTargets)
                                {
                                    ThingDef wepzie = ThingDef.Named(t);
                                    if (wepzie != null)
                                    {
                                        WhandCompProps Compie = new WhandCompProps();
                                        Compie.compClass = typeof(WhandComp);

                                        if (WepSets.MainHand != null)
                                        {
                                            Compie.MainHand = WepSets.MainHand;
                                        }
                                        if (WepSets.MainHand == Vector3.zero)
                                        {
                                            Compie.MainHand = Vector3.zero;
                                        }
                                        if (WepSets.SecHand != null)
                                        {
                                            Compie.SecHand = WepSets.SecHand;
                                        }
                                        if (WepSets.SecHand == Vector3.zero)
                                        {
                                            Compie.SecHand = Vector3.zero;
                                        }
                                        wepzie.comps.Add(Compie);
                                    }
                                }
                            }
                        }
                        LaserLoad();
                    }
                }
            }

        }


    }
}

