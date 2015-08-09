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

    public struct ChoiceUnit
    {
        public Unit Unit { get; set; }
        public int Position { get; set; }
    }

    public class GameMenu : Photon.MonoBehaviour
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

        private Unit _currentUnit;
        private List<ChoiceUnit> _playerChooice;
        private List<ChoiceUnit> _oppChooice;

        private List<ChoiceUnit> _currentLst = null;
        private PhotonView _network;

        public List<ChoiceUnit> GetOpSoldiers() { return _oppChooice; }

        public List<ChoiceUnit> GetPlSoldiers() { return _playerChooice; }

        public void AddSoldier(int x)
        {
            if (_currentUnit == Unit.Hunter && HunterAmount <= 0) return;
            if (_currentUnit == Unit.Hunter && HunterAmount > 0) HunterAmount -= 1;
            if (_currentUnit == Unit.Warrior && WarriorAmount <= 0) return;
            if (_currentUnit == Unit.Warrior && WarriorAmount > 0) WarriorAmount -= 1;
            if (_currentUnit == Unit.Mage && MageAmount <= 0) return;
            if (_currentUnit == Unit.Mage && MageAmount > 0) MageAmount -= 1;
            ChoiceUnit choice = new ChoiceUnit {Position = x, Unit = _currentUnit};
            _currentLst.Add(new ChoiceUnit {Position = x, Unit = _currentUnit});

        }

        void Start()
        {
            Instanse = this;
            _playerChooice = new List<ChoiceUnit>();
            _oppChooice = new List<ChoiceUnit>();
            _opReady = false;
            _plReady = false;
            _currentLst = PhotonNetwork.isMasterClient ? _playerChooice : _oppChooice;
            _network = GetComponent<PhotonView>();
        }

        [PunRPC]
        void NotifyServerReady()
        {
            _plReady = !_plReady;
            
            if (_opReady && _plReady) photonView.RPC("StartGame", PhotonTargets.All);
        }

        [PunRPC]
        void NotifyClientReady()
        {
            _opReady = !_opReady;
            if (_plReady && _opReady) photonView.RPC("StartGame", PhotonTargets.All);

        }

        [PunRPC]
        public void StartGame()
        {
            PhotonNetwork.LoadLevel("Game");
        }


        public void ReadyEven()
        {
            if (WarriorAmount > 0 || MageAmount > 0 || HunterAmount > 0) return;
            Ready.interactable = false;
            photonView.RPC(PhotonNetwork.isMasterClient ? "NotifyServerReady" : "NotifyClientReady",PhotonTargets.AllBuffered);
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
            if (PhotonNetwork.isMasterClient)
                Ready.transform.GetChild(0).GetComponent<Text>().text = _opReady ? "Start" : "Ready";
            else
                Ready.transform.GetChild(0).GetComponent<Text>().text = _plReady ? "Start" : "Ready";
            Warrior.text = "Left: " + WarriorAmount;
            Mage.text = "Left: " + MageAmount;
            Hunter.text = "Left: " + HunterAmount;
        }
    }
}