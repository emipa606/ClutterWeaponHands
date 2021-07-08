using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

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
            var Tdef = ThingDefOf.Human;
            if (Tdef == null)
            {
                return;
            }

            var Compie = new CompProperties {compClass = typeof(HandDrawer)};
            Tdef.comps.Add(Compie);
        }

        public bool HandCheck()
        {
            return ModsConfig.ActiveModsInLoadOrder.Any(mod => mod.Name == "Clutter Laser Rifle");
        }

        public void LaserLoad()
        {
            if (!HandCheck())
            {
                return;
            }

            var wepzie = ThingDef.Named("LaserRifle");
            if (wepzie == null)
            {
                return;
            }

            var Compie = new WhandCompProps
            {
                compClass = typeof(WhandComp),
                MainHand = new Vector3(-0.2f, 0.3f, -0.05f),
                SecHand = new Vector3(0.25f, 0f, -0.05f)
            };
            wepzie.comps.Add(Compie);
        }

        public void WeaponComps()
        {
            var Tdef = ThingDef.Named("ClutterHandsSettings");

            if (Tdef is not ClutterHandsTDef tDef)
            {
                return;
            }

            if (tDef.WeaponCompLoader.Count <= 0)
            {
                return;
            }

            foreach (var WepSets in tDef.WeaponCompLoader)
            {
                if (WepSets.ThingTargets.Count <= 0)
                {
                    continue;
                }

                foreach (var t in WepSets.ThingTargets)
                {
                    var wepzie = ThingDef.Named(t);
                    if (wepzie == null)
                    {
                        continue;
                    }

                    var Compie = new WhandCompProps {compClass = typeof(WhandComp), MainHand = WepSets.MainHand};


                    if (WepSets.MainHand == Vector3.zero)
                    {
                        Compie.MainHand = Vector3.zero;
                    }

                    Compie.SecHand = WepSets.SecHand;

                    if (WepSets.SecHand == Vector3.zero)
                    {
                        Compie.SecHand = Vector3.zero;
                    }

                    wepzie.comps.Add(Compie);
                }
            }

            LaserLoad();
        }
    }
}