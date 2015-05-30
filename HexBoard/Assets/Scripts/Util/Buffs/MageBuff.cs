using System;
using System.Collections;
using Assets.Scripts.UI;
using Assets.Scripts.Util.Abstract;
using UnityEngine;

namespace Assets.Scripts.Util.Buffs
{
    public class MageBuff : Buff
    {
        public MageBuff() : base(0) { }

        public override void DoBuff(Soldier soldier)
        {
            soldier.GetHealth().AddHealth(100);
            FloatingText.Show(String.Format("Heal"), "Heal",
            new FromWorldPointTextPositioner(Camera.main, soldier.transform.position, 2f, 60f));
        }

        public override void DisableBuff(Soldier soldier)
        {
            
        }
    }
}