using System.Collections.Generic;
using Assets.Scripts.Util;
using Assets.Scripts.Util.Interfaces;

namespace Assets.Scripts.Behaviour.Soldier
{
    public class Warrior : Util.Abstract.Soldier
    {


        public override void SpecialHit(ISoldier enemy)
        {
            throw new System.NotImplementedException();
        }

        public override void BuffAction(Util.Abstract.Soldier target)
        {
            throw new System.NotImplementedException();
        }
    }
}