using Assets.Scripts.UI;
using Assets.Scripts.Util;
using Assets.Scripts.Util.Buffs;
using Assets.Scripts.Util.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Behaviour.Soldier
{
    public class Warrior : Util.Abstract.Soldier
    {


        public override void SpecialHit(ISoldier enemy)
        {
            FloatingText.Show("Special!!!", "PlayerSpecialText",
                new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60f));
            //TODO: specialHit ani.
            int damege = CalHit();
            if (CheckIfCritical())
                damege *= CriticalHit;
            damege *= SpecialHitParameter;
            enemy.GetHealth().TakeDamage(damege);
        }

        public override void BuffAction(Util.Abstract.Soldier teamSoldier)
        {
            //TODO: buff ani.
            _buff.DoBuff(teamSoldier);
        }

        protected override void Init()
        {
            base.Init();
            _buff = new WarCryBuff();
        }
    }
}