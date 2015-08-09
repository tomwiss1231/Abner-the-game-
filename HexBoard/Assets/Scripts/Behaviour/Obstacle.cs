using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class Obstacle : Photon.MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.tag.Equals("Tile")) return;
            TileBehaviour tb = collision.gameObject.GetComponent<TileBehaviour>();
            if (tb.tile.Passable) tb.tile.Passable = false;

        }

    }
}