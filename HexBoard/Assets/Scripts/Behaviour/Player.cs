using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private List<Util.Abstract.Soldier> _soldiers;
        
        [SerializeField] private int _turns;

        [SerializeField] private bool _nowPlaying;

        [SerializeField] private Player _opponent;

        [SerializeField] public bool NowPalying { get { return _nowPlaying; } set { _nowPlaying = value; }}

        public void AddSoldier(Util.Abstract.Soldier soldier)
        {
            _soldiers.Add(soldier);
        }

        public void AddOpponent(Player opponent)
        {
            _opponent = opponent;
        }

        public Player GetOpponent()
        {
            return _opponent;
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
            _nowPlaying = true;
        }

        public bool EndTurn()
        {
            return _turns <= 0;
        }

        public void CheckSoldiers()
        {
            foreach (Util.Abstract.Soldier soldier in _soldiers)
            {
                StartCoroutine(soldier.CheckBuffs());
                soldier.FillSkillBar();
            }
        }

        public List<Util.Abstract.Soldier> GetSoldiers()
        {
            return _soldiers;
        }

        void Awake()
        {
            _turns = 0;
            _nowPlaying = false;
        }
    }
}