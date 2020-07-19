using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace WHands
{
    public class ClutterHandsTDef : ThingDef
    {
        public class CompTargets
        {
            public Vector3 MainHand = Vector3.zero;
            public Vector3 SecHand = Vector3.zero;
            public List<string> ThingTargets = new List<string>();
        }

        public List<CompTargets> WeaponCompLoader = new List<CompTargets>();
    }

}

