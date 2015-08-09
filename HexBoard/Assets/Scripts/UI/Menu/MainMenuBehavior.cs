using UnityEngine;

namespace Assets.Scripts.UI.Menu
{
    public class MainMenuBehavior : MonoBehaviour
    {
        public void StartGame()
        {
            Application.LoadLevel("lobby");
        }
        public void Exit()
        {
            Application.Quit();
        }
    }
}