using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace WHands
{
    public class ClutterHandsTDef : ThingDef
    {
        public List<CompTargets> WeaponCompLoader = new List<CompTargets>();

        public class CompTargets
        {
            public Vector3 MainHand = Vector3.zero;
            public Vector3 SecHand = Vector3.zero;
            public List<string> ThingTargets = new List<string>();
        }
    }
}