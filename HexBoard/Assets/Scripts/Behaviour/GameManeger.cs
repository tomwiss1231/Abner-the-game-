using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class GameManeger : MonoBehaviour
    {
        [SerializeField] public static GameManeger Instans = null;

        [SerializeField] public Player Player1;

        [SerializeField] public Player Player2;
    }
}