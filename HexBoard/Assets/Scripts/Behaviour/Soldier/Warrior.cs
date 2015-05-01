using Assets.Scripts.Util.Buffs;
using Assets.Scripts.Util.Interfaces;

namespace Assets.Scripts.Behaviour.Soldier
{
    public class Warrior : Util.Abstract.Soldier
    {


        public override void SpecialHit(ISoldier enemy)
        {
            //TODO: specialHit ani.
            int damege = CalHit();
            if (CheckIfCritical())
                damege *= CriticalHit;
            damege *= SpecialHitParameter;
            enemy.GetHealth().TakeDemage(damege);
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