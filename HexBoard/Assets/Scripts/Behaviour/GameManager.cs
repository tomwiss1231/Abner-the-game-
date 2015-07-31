using System.Collections.Generic;
using Assets.Scripts.Behaviour.Soldier;
using Assets.Scripts.UI.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Behaviour
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] public static GameManager Instans = null;

        [SerializeField] public Player Player1;
        
        [SerializeField] public Player Player2;
        
        [SerializeField] public List<TileBehaviour> _tiles;

        [SerializeField] public GameObject Hunter;
        [SerializeField] public GameObject Warrior;
        [SerializeField] public GameObject Mage;
        private bool _finish;

        void BuildServerSolier()
        {
            if (!Application.isPlaying) return;
            List<Tuple> soldiers = null;
            soldiers = GameMenu.Instanse.GetPlSoldiers();
           
            foreach (TileBehaviour tileBehaviour in _tiles)
            {
                foreach (Tuple opSoldier in soldiers)
                {
                    if (tileBehaviour.tile.X == opSoldier.Position.X && tileBehaviour.tile.Y == opSoldier.Position.Y)
                    {
                        GameObject soldier = null;
                        switch (opSoldier.Type)
                        {
                            case Unit.Hunter:
                                soldier = (GameObject)Instantiate(Hunter);
                                break;
                            case Unit.Warrior:
                                
                                soldier = (GameObject)Instantiate(Warrior);
                                break;
                            case Unit.Mage:
                                
                                soldier = (GameObject)Instantiate(Mage);
                                break;
                        }
                        if (soldier != null)
                        {
                            Util.Abstract.Soldier action = soldier.GetComponent<Util.Abstract.Soldier>();
                            soldier.tag = "A";
                            tileBehaviour.Soldier = soldier;
                            action.Position = tileBehaviour;
                            Vector3 pos = tileBehaviour.transform.position;
                            pos.y = 0.66f;
                            soldier.transform.position = pos;
                            action.SetPlayer(Player1);
                            action.Finish();
                        }
                    }
                }
            }
        }

        void BuildClientSoliders()
        {
            List<Tuple> soldiers = null;
            soldiers = GameMenu.Instanse.GetOpSoldiers();
            

            foreach (TileBehaviour tileBehaviour in _tiles)
            {
                foreach (Tuple opSoldier in soldiers)
                {
                    if (tileBehaviour.tile.X == opSoldier.Position.X && tileBehaviour.tile.Y == opSoldier.Position.Y)
                    {
                        GameObject soldier = null;
                        switch (opSoldier.Type)
                        {
                            case Unit.Hunter:
                                soldier = (GameObject)Instantiate(Hunter);
                                break;
                            case Unit.Warrior:
                                soldier = (GameObject)Instantiate(Warrior);
                                break;
                            case Unit.Mage:
                                soldier = (GameObject)Instantiate(Mage);
                                break;
                        }
                        Util.Abstract.Soldier action = soldier.GetComponent<Util.Abstract.Soldier>();
                        soldier.tag = "B";
                        tileBehaviour.Soldier = soldier;
                        action.Position = tileBehaviour;
                        Vector3 pos = tileBehaviour.transform.position;
                        pos.y = 0.66f;
                        soldier.transform.position = pos;
                        action.SetPlayer(Player2);
                        action.Finish();
                    }
                }
            }
        }

        void Awake()
        {
            Player1.tag = "A";
            Player1Restart();
            Player2.tag = "B";
            Instans = this;
            Player1.AddOpponent(Player2);
            Player2.AddOpponent(Player1);
            _finish = false;
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

        public void AddTile(TileBehaviour tb)
        {
            if (!Application.isPlaying) return;
            _tiles.Add(tb);
            if (_tiles.Count > 215)
            {
                _finish = true;

              BuildServerSolier();
              BuildClientSoliders();

            }
        }


        public void ExitGame()
        {
            Application.Quit();
        }

        void Update()
        {
            if(!_finish) return;
            if (Player1.GetSoldiers().Count == 0)
            {
                print("Player2 wins!!!!!");
                Application.LoadLevel("MainMenu");
            }
            if (Player2.GetSoldiers().Count == 0)
            {
                print("Player1 wins!!!!!");
                Application.LoadLevel("MainMenu");
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
    }
}