using Assets.Scripts.Behaviour;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class Spawn : Photon.MonoBehaviour
    {
        public int SpawnId = -1;
        public string SpawnTag = "None";
        public TileBehaviour Tile;

        void Start()
        {
            Tile = GetComponent<TileBehaviour>();
        }
         
    }
}