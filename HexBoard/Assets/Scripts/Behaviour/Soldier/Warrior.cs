using Assets.Scripts.UI;
using Assets.Scripts.Util;
using Assets.Scripts.Util.Buffs;
using Assets.Scripts.Util.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Behaviour.Soldier
{
    public class Warrior : Util.Abstract.Soldier
    {


        public override bool SpecialHit(ISoldier enemy)
        {
            if (!SkillBar.DecSkillPoints(10)) return false;
            FloatingText.Show("Special!!!", "PlayerSpecialText",
            new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60f));
            //TODO: specialHit ani.
            int damege = CalHit();
            if (CheckIfCritical())
                damege *= CriticalHit;
            damege *= SpecialHitParameter;
            enemy.GetHealth().TakeDamage(damege);
            return true;
        }

        public override void FillSkillBar()
        {
            SkillBar.AddSkillPoints(8);
        }

        public override void BuffAction(Util.Abstract.Soldier teamSoldier)
        {
            //TODO: buff ani.
            WarCryBuff warCryBuff = new WarCryBuff();
            warCryBuff.DoBuff(teamSoldier);
            Buffs.Add(warCryBuff);
        }

        protected override void Init()
        {
            base.Init();
            Buff = new WarCryBuff();
        }
    }
}