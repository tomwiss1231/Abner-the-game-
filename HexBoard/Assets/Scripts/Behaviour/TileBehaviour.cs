﻿using System;
using Assets.Scripts.Util;
using Assets.Scripts.Util.Interfaces;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    [Serializable]
    [ExecuteInEditMode]
    public class TileBehaviour : Photon.MonoBehaviour , ISoldierObserver
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

        [PunRPC]
        public void CalcWalk()
        {
            var selectedSoldier = GridManager.instance.SelectedSoldier;
            if (selectedSoldier == null) return;
            if (!IsNeighbour || selectedSoldier.IsMoving()) return;
            selectedSoldier.ClearWalkPath();
            ChangeToPath();
            Stack<TileBehaviour> path = new Stack<TileBehaviour>();
            path.Push(this);
            FindPath(this, path);
        }

        public void HaveEnemy(int n, int weight, string teamTag)
        {
            if(n == 0) return;
            if (IsOccupied())
            {
/*                GridManager.instance.SelectedSoldier.AddObserver(this);
                Util.Abstract.Soldier soldier = Soldier.GetComponent<Util.Abstract.Soldier>(); 
                //soldier.InAttackRange = true;
                if(soldier.tag.Equals(teamTag)) ChangeToBuff(); 
                else ChangeToTarget();
                return;*/
            }
            IsNeighbour = true;
            tile.WeightArea = weight + 1;
            foreach (TileBehaviour tb in tile.GetAllNeighbours().Where(tb => tb.tile.WeightArea > weight + 1))
                tb.HaveEnemy(n-1, weight + 1, teamTag);
        }

        public void Neighbour(int n, int weight)
        {
            if (n == 0) return;
            ChangeToWalk();
            IsNeighbour = true;
            tile.WeightWalk = weight + 1;
            GridManager.instance.SelectedSoldier.AddObserver(this);
            foreach (TileBehaviour tb in tile.GetAllNeighbours().Where(tb => !tb.IsOccupied() && tb.tile.WeightWalk > weight + 1))
                tb.Neighbour(n - 1, weight + 1);
        }

        void changeColor(Color color)
        {
            if (color.a == 1)
                color.a = 130f / 255f;
            var renderer = transform.GetChild(0).GetComponent<Renderer>();
            renderer.material = OpaqueMaterial;
            renderer.material.color = color;
        }

        public void ShowAllPaths(int stp)
        {
            if (!IsNeighbour)
            {
                GridManager.instance.selecetedTile = this;
                tile.WeightWalk = 0;
                GridManager.instance.SelectedSoldier.AddObserver(this);
                foreach (TileBehaviour tb in tile.GetAllNeighbours().Where(tb => !tb.IsOccupied() && tb.tile.WeightWalk > tile.WeightWalk + 1))
                    tb.Neighbour(stp, 0);
                ChangeToBuff();
                steps = stp;
            }
        }

        public void CalHitRange(int range)
        {
           tile.WeightArea = 0;
            foreach (TileBehaviour tb in tile.GetAllNeighbours().Where(tb => tb.tile.WeightArea > tile.WeightArea + 1))
                tb.HaveEnemy(range,0, Soldier.tag);
            

        }

        public void ChangePassebleStatus()
        {
            tile.Passable = !tile.Passable;
            if (!tile.Passable) changeColor(Color.gray);
            else SetDefault();
        }

        void OnMouseOver()
        {
            if (GridManager.instance.SelectedSoldier != null && GridManager.instance.SelectedSoldier.CheckingArea)
                return;
            if (Input.GetMouseButtonDown(0))
            {
                photonView.RPC("CalcWalk",PhotonTargets.All);
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

        public void ChangeToWalk()
        {
            ChangeColor(false, true, false, false, false);
        }

        public void ChangeToDefault()
        {
            ChangeColor(true, false, false, false, false);
        }
        
        public void ChangeToPath()
        {
            ChangeColor(false, false, true, false, false);
        }
        
        public void ChangeToTarget()
        {
            ChangeColor(false, false, false, true, false);
        }

        public void ChangeToBuff()
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

        void FindPath(TileBehaviour tb, Stack<TileBehaviour> path)
        {
            if (tb.tile.WeightWalk == 0)
            {
                tb.Soldier.GetComponent<Util.Abstract.Soldier>().SavePath(path);
                Soldier = tb.Soldier;
                Soldier.GetComponent<Util.Abstract.Soldier>().Destination = this;
                return;
            }
            TileBehaviour min = tb;
            foreach (TileBehaviour neighbour in tb.tile.GetAllNeighbours())
                if (min.tile.WeightWalk > neighbour.tile.WeightWalk) min = neighbour;
            path.Push(min);
            min.ChangeToPath();
            FindPath(min,path);
        }

        private bool IsOccupied()
        {
            return Soldier != null;
        }

        void Start()
        {
            if (!Application.isPlaying) return;
            if (Soldier != null) Soldier.GetComponent<Util.Abstract.Soldier>().Position = this;
            tile.ResetWeight();
            Spawn spawn = GetComponent<Spawn>();
            spawn.enabled = true;
        }

        public void NotifyChange()
        {
            SetDefault();
        }
    }
}
