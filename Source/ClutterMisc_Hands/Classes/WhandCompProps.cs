using Verse;
using UnityEngine;

namespace WHands
{

    public class WhandCompProps : CompProperties
    {
        public WhandCompProps()
        {
            compClass = typeof(WHands.WhandComp);
        }
        public Vector3 MainHand = Vector3.zero;
        public Vector3 SecHand = Vector3.zero;



    }
}

