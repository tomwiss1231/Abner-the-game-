using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] public static GameManager Instans = null;

        [SerializeField] public Player Player1;
        
        [SerializeField] public Player Player2;

        void Awake()
        {
            Player1.tag = "A";
            Player1Restart();
            Player2.tag = "B";
            Instans = this;
            Player1.AddOpponent(Player2);
            Player2.AddOpponent(Player1);
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

        public void ExitGame()
        {
            Application.Quit();
        }

        void Update()
        {
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