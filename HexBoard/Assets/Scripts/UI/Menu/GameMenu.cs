using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    public enum Unit
    {
        Warrior = 1,
        Mage = 2,
        Hunter = 3
    }

    public struct Tuple
    {
        private Point _point;
        private Unit _type;

        public Point Position
        {
            get { return _point; }
        }
        public Unit Type { get { return _type; } }

        public void SetPosition(Point point)
        {
            _point = point;
        }

        public Tuple(int x, int y, Unit type)
        {
            _point = new Point(x, y);
            _type = type;
        }

    }
    public class GameMenu : MonoBehaviour
    {
        public static GameMenu Instanse = null;
        public Text Warrior;
        public Text Mage;
        public Text Hunter;

        public Button Ready;

        public int WarriorAmount;
        public int MageAmount;
        public int HunterAmount;

        private bool _opReady;
        private bool _plReady;
        private bool _startingGame;

        private Unit _currentUnit;
        private List<Tuple> _playerChooice;
        private List<Tuple> _oppChooice;

        public void AddSoldier(string point)
        {
            if (_plReady) return;
            string[] position = point.Split(',');
            int x = int.Parse(position[0]);
            int y = int.Parse(position[1]);
            AddSoldier(x, y);
        }

        public List<Tuple> GetOpSoldiers() { return _oppChooice; }

        public List<Tuple> GetPlSoldiers() { return _playerChooice; }

        public void AddSoldier(int x, int y)
        {
            if (_currentUnit == Unit.Hunter && HunterAmount <= 0) return;
            if (_currentUnit == Unit.Hunter && HunterAmount > 0) HunterAmount -= 1;
            if (_currentUnit == Unit.Warrior && WarriorAmount <= 0) return;
            if (_currentUnit == Unit.Warrior && WarriorAmount > 0) WarriorAmount -= 1;
            if (_currentUnit == Unit.Mage && MageAmount <= 0) return;
            if (_currentUnit == Unit.Mage && MageAmount > 0) MageAmount -= 1;

            _playerChooice.Add(new Tuple(x, y, _currentUnit));
        }

        public void ClientAddSoldier(string point)
        {
            if (!_plReady) return;
            string[] position = point.Split(',');
            int x = int.Parse(position[0]);
            int y = int.Parse(position[1]);
            if (_currentUnit == Unit.Hunter && HunterAmount <= 0) return;
            if (_currentUnit == Unit.Hunter && HunterAmount > 0) HunterAmount -= 1;
            if (_currentUnit == Unit.Warrior && WarriorAmount <= 0) return;
            if (_currentUnit == Unit.Warrior && WarriorAmount > 0) WarriorAmount -= 1;
            if (_currentUnit == Unit.Mage && MageAmount <= 0) return;
            if (_currentUnit == Unit.Mage && MageAmount > 0) MageAmount -= 1;
            _oppChooice.Add(new Tuple(x, y, _currentUnit));

        }

        void Start()
        {
            Instanse = this;
            _playerChooice = new List<Tuple>();
            _oppChooice = new List<Tuple>();
            _opReady = false;
            _plReady = false;
            _startingGame = false;
        }

        public void ReadyEven()
        {
            if (WarriorAmount > 0 || MageAmount > 0 || HunterAmount > 0) return;
            if (!_plReady)
            {
                WarriorAmount = 1;
                MageAmount = 1;
                HunterAmount = 1;
                _plReady = true;
            }
            else if (_plReady) _opReady = true;
            if(_plReady && _opReady) Application.LoadLevel("Game");
        }

        public void ReturnMain()
        {
            Application.LoadLevel("MainMenu");
        }

        public void ChooseWarrior()
        {
            _currentUnit = Unit.Warrior;
        }

        public void ChooseMage()
        {
            _currentUnit = Unit.Mage;
        }

        public void ChooseHunter()
        {
            _currentUnit = Unit.Hunter;
        }

        void OnGUI()
        {
            if (_plReady)
                Ready.transform.GetChild(0).GetComponent<Text>().text = "Start";
            else Ready.transform.GetChild(0).GetComponent<Text>().text = "Ready";
            Warrior.text = "Left: " + WarriorAmount;
            Mage.text = "Left: " + MageAmount;
            Hunter.text = "Left: " + HunterAmount;
        }
    }
}