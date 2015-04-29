using System;

namespace Assets.Scripts.Util.Abstract
{
    public abstract class Buff
    {
        private int _turnleft;
        
        public Buff(int turnLeft)
        {
            _turnleft = turnLeft;
        }
        public abstract void DoBuff(Soldier soldier);

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