using Assets.Scripts.Util.Abstract;

namespace Assets.Scripts.Util.Buffs
{
    public class ArcherBuff : Buff
    {
        public ArcherBuff(int turnLeft) : base(turnLeft) { }

        public override void DoBuff(Soldier soldier)
        {
            soldier.MaxDamege *= 3;
            soldier.MinDamege *= 3;
        }
    }
}