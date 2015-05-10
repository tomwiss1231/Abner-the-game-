using System.Collections;
using Assets.Scripts.Util.Abstract;
using UnityEngine;

namespace Assets.Scripts.Util.Buffs
{
    public class MageBuff : Buff
    {
        public MageBuff(int turnLeft) : base(turnLeft) { }

        public override void DoBuff(Soldier soldier)
        {
            soldier.GetHealth().AddHealth(100);
        }

        public override void DisableBuff(Soldier soldier)
        {
            
        }
    }
}