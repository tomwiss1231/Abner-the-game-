using System;
using System.Collections.Generic;
using Assets.Scripts.UI;
using Assets.Scripts.Util;
 #if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    [Serializable]
    [ExecuteInEditMode]
    public class GridManager : MonoBehaviour
    {
        [SerializeField] public static GridManager instance = null;

        [SerializeField] public GameObject Hex;

        [SerializeField] public GameObject Ground;
        [SerializeField] public Material SelectM;
        [SerializeField, HideInInspector] public TileBehaviour selecetedTile = null;

        [SerializeField, HideInInspector] public Util.Abstract.Soldier SelectedSoldier = null;

        [SerializeField, HideInInspector] private float hexWidth;
        [SerializeField, HideInInspector] private float hexHeight;
        [SerializeField, HideInInspector] private float groundWidth;
        [SerializeField, HideInInspector] private float groundHeight;
        [SerializeField, HideInInspector] private Vector3 _initPos;

        [SerializeField] private  Dictionary<Point, TileBehaviour> board;

        public void ShowWalkRadiusEvent()
        {
            if (SelectedSoldier != null && !SelectedSoldier.IsMoving())
            {
                SelectedSoldier.ClearAll();
                SelectedSoldier.WalkRadius();
                StartCoroutine(SelectedSoldier.ShowHitRange());
//                SelectedSoldier.ShowHitRange();
            }
        }

        public void CancelEve()
        {
            if (SelectedSoldier != null && !SelectedSoldier.IsMoving())
            {
                SelectedSoldier.ClearAll();
                SelectedSoldier = null;
            }
        }

        void setSizes()
        {
            hexWidth = Hex.transform.GetChild(0).GetComponent<Renderer>().bounds.size.x;
            hexHeight = Hex.transform.GetChild(0).GetComponent<Renderer>().bounds.size.z;
            groundWidth = Ground.GetComponent<Renderer>().bounds.size.x;
            groundHeight = Ground.GetComponent<Renderer>().bounds.size.z;
        }

        Vector2 calcGridSize()
        {
            float sideLength = hexHeight / 2;
            int nrOfSides = (int)(groundHeight / sideLength);
            int gridHeightInHexes = (int)(nrOfSides * 2 / 3);
            if (gridHeightInHexes % 2 == 0
                && (nrOfSides + 0.5f) * sideLength > groundHeight)
                gridHeightInHexes--;
            return new Vector2((int)(groundWidth / hexWidth), gridHeightInHexes);
        }


        Vector3 calcInitPos()
        {
        
            _initPos = new Vector3(-groundWidth / 2 + hexWidth / 2, 0,
                groundHeight / 2 - hexWidth / 2);
            return _initPos;
        }

       public Vector3 calcWorldCoord(Vector2 gridPos)
        {
            Vector3 initPos = _initPos;
            float offset = 0;
            if (gridPos.y % 2 != 0)
                offset = hexWidth / 2;

            float x = initPos.x + offset + gridPos.x * hexWidth;
            float z = initPos.z - gridPos.y * hexHeight * 0.75f;
            return new Vector3(x, 0, z);
        }

        void createGrid()
        {
            Vector2 gridSize = calcGridSize();
            GameObject hexGridGO = new GameObject("HexGrid");
            board = new Dictionary<Point, TileBehaviour>();

            for (float y = 0; y < gridSize.y; y++)
            {
                float sizeX = gridSize.x;
                if (y % 2 != 0 && (gridSize.x + 0.5) * hexWidth > groundWidth)
                    sizeX--;
                for (float x = 0; x < sizeX; x++)
                {
                    GameObject hex = (GameObject) Instantiate(Hex);
                    Vector2 gridPos = new Vector2(x, y);
                    hex.transform.position = calcWorldCoord(gridPos);
                    hex.transform.parent = hexGridGO.transform;
                    TileBehaviour tileB = hex.GetComponent<TileBehaviour>();
                    tileB.tile = new Tile((int)x - (int)(y / 2), (int)y, true);;
                    board.Add(tileB.tile.Location, tileB);
                }
            }
            bool equalLineLengths = (gridSize.x + 0.5) * hexWidth <= groundWidth;
            foreach (TileBehaviour tb in board.Values)
                tb.tile.FindNeighbours(board, gridSize, equalLineLengths);

            hexGridGO.transform.parent = Ground.transform.parent;
            hexGridGO.transform.position = new Vector3(Ground.transform.position.x, Ground.transform.position.y + 0.155f, Ground.transform.position.z);
//            Selection.activeGameObject = hexGridGO;
        }

        public void init()
        {
            setSizes();
            calcInitPos();
            createGrid();
        }

        void Start()
        {
            instance = this;
            setSizes();
            calcInitPos();
        }
    }
}