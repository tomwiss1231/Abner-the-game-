using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Util.Abstract
{
    public abstract class Buff
    {
        private int _turnleft;

        protected Buff(int turnLeft)
        {
            _turnleft = turnLeft;
        }
        public abstract void DoBuff(Soldier soldier);
        public abstract void DisableBuff(Soldier soldier);

        public  bool CheckIfTurn()
        {
            return _turnleft > 0;
        }

        public void DecTurns()
        {
            _turnleft--;
        }




    }
}