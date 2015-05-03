using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private List<Util.Abstract.Soldier> _soldiers;
        
        [SerializeField] private int _turns;

        public void AddSoldier(Util.Abstract.Soldier soldier)
        {
            _soldiers.Add(soldier);
        }

        public void RemoveSoldier(Util.Abstract.Soldier soldier)
        {
            _soldiers.Remove(soldier);
        }

        public void DecTurns()
        {
            _turns -= 1;
        }

        public void IncTurns()
        {
            _turns += 1;
        }

        public void Restart()
        {
            _turns = 2;
        }

        public bool EndTurn()
        {
            return _turns <= 0;
        }

        public void CheckSoldierBuff()
        {
            foreach (Util.Abstract.Soldier soldier in _soldiers)
            {
                soldier.CheckBuffs();
            }
        }

        void Start()
        {
            _turns = 0;
        }
    }
}