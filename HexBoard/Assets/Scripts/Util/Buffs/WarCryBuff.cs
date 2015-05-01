using Assets.Scripts.Util.Abstract;

namespace Assets.Scripts.Util.Buffs
{
    public class WarCryBuff : Buff
    {
        public WarCryBuff() : base(3) {}

        public override void DoBuff(Soldier soldier)
        {
            soldier.Armor *= 3;
        }

       
    }
}