using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Behaviour.Soldier;
using Assets.Scripts.UI.Menu;
using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Behaviour
{
    public class GameManager : Photon.MonoBehaviour
    {
        [SerializeField] public static GameManager Instans = null;

        [SerializeField] public Player Player1;
        
        [SerializeField] public Player Player2;
        
        [SerializeField] public List<TileBehaviour> _tiles;

        [SerializeField] public GameObject Hunter;
        [SerializeField] public GameObject Warrior;
        [SerializeField] public GameObject Mage;

        [SerializeField] public GameObject P1Win;
        [SerializeField] public GameObject P2Win;

        private bool _finishServer;
        private bool _finishClient;

        void BuildServerSolier()
        {
            if (!Application.isPlaying) return;
            List<ChoiceUnit> choices = GameMenu.Instanse.GetPlSoldiers();
            foreach (ChoiceUnit choice in choices)
            {
                GameObject soldier = null;
                switch (choice.Unit)
                {
                    case Unit.Hunter:
                        soldier = PhotonNetwork.Instantiate(Hunter.name, Vector3.zero, Quaternion.identity, 0);
                        break;
                    case Unit.Warrior:

                        soldier = PhotonNetwork.Instantiate(Warrior.name, Vector3.zero, Quaternion.identity, 0);
                        break;
                    case Unit.Mage:

                        soldier = PhotonNetwork.Instantiate(Mage.name, Vector3.zero, Quaternion.identity, 0);
                        break;
                }
                Util.Abstract.Soldier action = soldier.GetComponent<Util.Abstract.Soldier>();
                action.photonView.RPC("SetSoldier", PhotonTargets.AllBuffered, "A", choice.Position);
            }
            photonView.RPC("ServerFinishBuilding", PhotonTargets.All);
        }

        void BuildClientSoliders()
        {
            List<ChoiceUnit> choices = GameMenu.Instanse.GetOpSoldiers();
            foreach (ChoiceUnit choice in choices)
            {
                GameObject soldier = null;
                switch (choice.Unit)
                {
                    case Unit.Hunter:
                        soldier = PhotonNetwork.Instantiate(Hunter.name, Vector3.zero, Quaternion.identity, 0);
                        break;
                    case Unit.Warrior:

                        soldier = PhotonNetwork.Instantiate(Warrior.name, Vector3.zero, Quaternion.identity, 0);
                        break;
                    case Unit.Mage:

                        soldier = PhotonNetwork.Instantiate(Mage.name, Vector3.zero, Quaternion.identity, 0);
                        break;
                }
                Util.Abstract.Soldier action = soldier.GetComponent<Util.Abstract.Soldier>();
                action.photonView.RPC("SetSoldier", PhotonTargets.AllBuffered, "B", choice.Position);
            }
            photonView.RPC("CilentFinishBuilding", PhotonTargets.All);
        }

        [PunRPC]
        public void ServerFinishBuilding()
        {
            _finishServer = true;
        }

        [PunRPC]
        public void CilentFinishBuilding()
        {
            _finishClient = true;
        }

        void Awake()
        {
            Player1.tag = "A";
            Player1Restart();
            Player2.tag = "B";
            Instans = this;
            Player1.AddOpponent(Player2);
            Player2.AddOpponent(Player1);
            _finishServer = false;
            _finishClient = false;
            if(PhotonNetwork.isMasterClient) BuildServerSolier();
            else BuildClientSoliders();
        }

        public void Player1Restart()
        {
            Player2.NowPalying = false;
            Player1.Restart();
            Player1.CheckSoldiers();
        }

        public void Player2Restart()
        {
            Player1.NowPalying = false;
            Player2.Restart();
            Player2.CheckSoldiers();
        }

        [PunRPC]
        public void DisconnectAll()
        {
            PhotonNetwork.Disconnect();
        }

        public void ExitGame()
        {
            //print(PhotonNetwork.isMasterClient ? "Player2 wins!!!!!" : "Player1 wins!!!!!");
            photonView.RPC("DisconnectAll", PhotonTargets.All);
        }

        void Update()
        {
            if (!_finishServer || !_finishClient) return;
            if (Player1.GetSoldiers().Count == 0)
            {
               P2Win.SetActive(true);
            }
            if (Player2.GetSoldiers().Count == 0)
            {
                P1Win.SetActive(true);
            }
            if (Player1.NowPalying && Player1.EndTurn())
            {
                Player2Restart();
                return;
            }

            if (Player2.NowPalying && Player2.EndTurn())
            {
                Player1Restart();
                return;
            }
        }

        void OnDisconnectedFromPhoton()
        {
            Application.LoadLevel("MainMenu");
        }
    }
}