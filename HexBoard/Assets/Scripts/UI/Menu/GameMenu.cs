using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.UI.Menu
{
    public class GameMenu : MonoBehaviour
    {
        public static GameMenu Instanse = null;
        private Dictionary<int, Point> _playerChooice;

        public void AddSoldier(int x, int y)
        {
            
        }

        public void ReturnMain()
        {
            Application.LoadLevel("MainMenu");
        }

        public void StartGame()
        {
            Application.LoadLevel("Game");
        }

        void Start()
        {
            Instanse = this;
            //_playerChooice = new List<Point>();
        }
    }
}