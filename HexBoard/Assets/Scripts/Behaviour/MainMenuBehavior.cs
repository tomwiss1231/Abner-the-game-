using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class MainMenuBehavior : Photon.MonoBehaviour
    {
        public void StartGame()
        {
            Application.LoadLevel("Game");
        }
        public void Exit()
        {
            Application.Quit();
        }
    }
}