using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class CameraLook : Photon.MonoBehaviour
    {
        public GameObject toLook;
    
        void Update()
        {
            transform.LookAt(toLook.transform.position);
        }
    }
}

