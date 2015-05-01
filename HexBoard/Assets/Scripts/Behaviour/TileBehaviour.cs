using System;
using Assets.Scripts.Util;
using Assets.Scripts.Util.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    [Serializable]
    [ExecuteInEditMode]
    public class TileBehaviour : MonoBehaviour
    {
        [SerializeField] 
        public GameObject Soldier = null;
        
        [SerializeField] 
        public Tile tile;
    
        [SerializeField]
        public Material OpaqueMaterial;

        [SerializeField]
        public Material defaultMaterial;

        [SerializeField]
        public int steps = 2;
    
        [SerializeField]
        public bool IsNeighbour { get { return _isNeighbour; } set { _isNeighbour = value; } }

        [SerializeField]
        private Color orange = new Color(255f / 255f, 127f / 255f, 0, 127f / 255f);

        [SerializeField]
        private bool _isNeighbour;


        public void HaveEnemy(int n, int weight, string teamTag)
        {
            if(n == 0) return;
            if(weight + 1 >= tile.Weight) return;
            if (IsOccupied())
            {
                Util.Abstract.Soldier soldier = Soldier.GetComponent<Util.Abstract.Soldier>(); 
                soldier.InAttackRange = true;
                if(soldier.tag.Equals(teamTag)) ChangeToBuff(); 
                else ChangeToTarget();
                return;
            }
            IsNeighbour = true;
            tile.Weight = weight + 1;
            foreach (TileBehaviour tb in tile.GetAllNeighbours())
                tb.HaveEnemy(n-1, weight + 1, teamTag);
        }

        public void Neighbour(int n, int weight)
        {
            if (n == 0 || IsOccupied()) return;
            if (weight + 1 >= tile.Weight) return;
            ChangeToWalk();
            IsNeighbour = true;
            tile.Weight = weight + 1;

            foreach (TileBehaviour tb in tile.GetAllNeighbours())
                tb.Neighbour(n - 1, weight + 1);
        }

        public void Clear(int n) {
            if (n == 0) return;
            foreach (TileBehaviour tb in tile.GetAllNeighbours())
            {
                tb.Clear(n - 1);
                tb.SetDefault();
            }
            SetDefault();
        }

        void changeColor(Color color)
        {
            if (color.a == 1)
                color.a = 130f / 255f;
            var renderer = transform.GetChild(0).GetComponent<Renderer>();
            renderer.material = OpaqueMaterial;
            renderer.material.color = color;
        }

        public void showAllPaths(int stp)
        {
            if (!IsNeighbour)
            {
                GridManager.instance.selecetedTile = this;
                tile.Weight = 0;
                foreach (TileBehaviour tb in tile.GetAllNeighbours())
                    tb.Neighbour(stp, 0);
                ChangeToBuff();
                steps = stp;
            }
        }

        public void CalHitRange(int range)
        {
           tile.Weight = 0;
            foreach (TileBehaviour tb in tile.GetAllNeighbours())
                tb.HaveEnemy(range,0, Soldier.tag);

        }

        public void ChangePassebleStatus()
        {
            tile.Passable = !tile.Passable;
            if (!tile.Passable) changeColor(Color.gray);
            else SetDefault();
        }

        void OnMouseExit()
        {
            if (tile.Passable && GridManager.instance.selecetedTile != this && !IsNeighbour)
                SetDefault();
        }


        void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!IsNeighbour || GridManager.instance.SelectedSoldier.IsMoving()) return;
                ChangeToPath();
                Stack<Tile> path = new Stack<Tile>();
                path.Push(tile);
                FindPath(this, path);
            }
        }

        void ChangeColor(bool def, bool walk, bool path, bool target, bool buff)
        {
            transform.GetChild(0).gameObject.SetActive(def);
            transform.GetChild(1).gameObject.SetActive(walk);
            transform.GetChild(2).gameObject.SetActive(path);
            transform.GetChild(3).gameObject.SetActive(target);
            transform.GetChild(4).gameObject.SetActive(buff);
        }

        void ChangeToWalk()
        {
            ChangeColor(false, true, false, false, false);
        }

        void ChangeToDefault()
        {
            ChangeColor(true, false, false, false, false);
        }
        
        void ChangeToPath()
        {
            ChangeColor(false, false, true, false, false);
        }
        
        void ChangeToTarget()
        {
            ChangeColor(false, false, false, true, false);
        }

        void ChangeToBuff()
        {
            ChangeColor(false, false, false, false, true);
        }

        public void SetDefault()
        {
            var renderer = transform.GetChild(0).GetComponent<Renderer>();
            ChangeToDefault();
            if (IsOccupied()) Soldier.GetComponent<Util.Abstract.Soldier>().InAttackRange = false;
            IsNeighbour = false;
            tile.ResetWeight();
        }

        void FindPath(TileBehaviour tb, Stack<Tile> path)
        {
            if (tb.tile.Weight == 0)
            {
                tb.Soldier.GetComponent<Util.Abstract.Soldier>().Walk(path);
                Soldier = tb.Soldier;
                Soldier.GetComponent<Util.Abstract.Soldier>().Destination = this;
                return;
            }
            TileBehaviour min = tb;
            foreach (TileBehaviour neighbour in tb.tile.GetAllNeighbours())
                if (min.tile.Weight > neighbour.tile.Weight) min = neighbour;
            path.Push(min.tile);
            min.ChangeToPath();
            FindPath(min,path);
        }

        private bool IsOccupied()
        {
            return Soldier != null;
        }

        void Start()
        {
            if (Soldier != null) Soldier.GetComponent<Util.Abstract.Soldier>().Position = this;
        }

    }
}
