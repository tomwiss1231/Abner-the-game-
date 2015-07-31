using Assets.Scripts.Behaviour;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class Spawn : MonoBehaviour
    {
        public int SpawnId = -1;
        public string SpawnTag = "A";
        public TileBehaviour Tile;

        void Start()
        {
            Tile = GetComponent<TileBehaviour>();
        }
         
    }
}