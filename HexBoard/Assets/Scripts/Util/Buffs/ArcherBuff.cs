using System;
using System.Collections;
using Assets.Scripts.UI;
using Assets.Scripts.Util.Abstract;
using UnityEngine;

namespace Assets.Scripts.Util.Buffs
{
    public class ArcherBuff : Buff
    {
        public ArcherBuff() : base(2) { }

        public override void DoBuff(Soldier soldier)
        {
            soldier.MaxDamege *= 3;
            soldier.MinDamege *= 3;
            FloatingText.Show(String.Format("Damage is up by 30%"), "PlayerTakeBuffText",
            new FromWorldPointTextPositioner(Camera.main, soldier.transform.position, 2f, 60f));
        }

        public override void DisableBuff(Soldier soldier)
        {
            soldier.MaxDamege /= 3;
            soldier.MinDamege /= 3;
        }
    }
}