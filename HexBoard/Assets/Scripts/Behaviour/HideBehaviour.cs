using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class HideBehaviour : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag.Equals("A") || collision.gameObject.tag.Equals("B"))
            {
                var soldier = collision.gameObject.GetComponent<Util.Abstract.Soldier>();
                print("enter");
                if (!soldier.IsHideing)
                {
                    soldier.IsHideing = true;
                }
            }
        }

        void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag.Equals("A") || collision.gameObject.tag.Equals("B"))
            {
                var soldier = collision.gameObject.GetComponent<Util.Abstract.Soldier>();
                print("exit");
                if (soldier.IsHideing)
                {
                    soldier.IsHideing = false;
                }
            }
            
        }
    }
}