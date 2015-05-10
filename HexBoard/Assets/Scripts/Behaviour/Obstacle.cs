using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class Obstacle : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag.Equals("Tile"))
            {
                TileBehaviour tb = collision.gameObject.GetComponent<TileBehaviour>();
                if(tb.tile.Passable) tb.tile.Passable = false;
            }
        }

    }
}