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
            GridManager.instance.Special.onClick.RemoveAllListeners();
            if (IsAttacking || !SkillBar.DecSkillPoints(10)) return;
            FloatingText.Show("Special!!!", "PlayerSpecialText",
            new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60f));
            //TODO: specialHit ani.
            int damege = CalHit();
            if (CheckIfCritical())
                damege *= CriticalHit;
            damege *= SpecialHitParameter;
            enemy.GetHealth().TakeDamage(damege);
            EndOfTurnAction();
        }

        public override void FillSkillBar()
        {
            SkillBar.AddSkillPoints(8);
        }

        public override void ActivateBuffCast()
        {
            InAttackRange = true;
        }

        public override void BuffAction(Util.Abstract.Soldier teamSoldier)
        {
            //TODO: buff ani.
            WarCryBuff warCryBuff = new WarCryBuff();
            warCryBuff.DoBuff(teamSoldier);
            Buffs.Add(warCryBuff);
            EndOfTurnAction();
        }

        protected override string BuffName()
        {
            return "War Cry";
        }

        protected override void Init()
        {
            base.Init();
            Buff = new WarCryBuff();
        }
    }
}