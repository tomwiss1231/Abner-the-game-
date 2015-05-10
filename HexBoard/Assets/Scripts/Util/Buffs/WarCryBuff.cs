using System;
using System.Collections;
using Assets.Scripts.UI;
using Assets.Scripts.Util.Abstract;
using UnityEngine;

namespace Assets.Scripts.Util.Buffs
{
    public class WarCryBuff : Buff
    {
        public WarCryBuff() : base(1) {}

        public override void DoBuff(Soldier soldier)
        {
            soldier.Armor *= 3;
            FloatingText.Show(String.Format("Armor {0}%", soldier.Armor), "PlayerTakeBuffText",
                new FromWorldPointTextPositioner(Camera.main, soldier.transform.position, 2f, 60f));
        }

        public override void DisableBuff(Soldier soldier)
        {
            soldier.Armor /= 3;
            FloatingText.Show(String.Format("Armor {0}%", soldier.Armor), "PlayerTakeDeBuffText",
                new FromWorldPointTextPositioner(Camera.main, soldier.transform.position, 2f, 60f));
        }
    }
}