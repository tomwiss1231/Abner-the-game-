using UnityEngine;

namespace Assets.Scripts.Util
{
    public class FreezeRotation : Photon.MonoBehaviour
    {
        private Quaternion _rotation;
        private Vector3 _soldierPos;
        private bool _left;
        private bool _right;
        void Awake()
        {
            _rotation = transform.rotation;
            _soldierPos = transform.parent.position;
            _right = true;
            _left = false;
        }

        void RotateCher()
        {
            transform.Rotate(-60, 0, -180);
            _rotation = transform.rotation;
        }

        void Update()
        {
            if (_soldierPos.x > transform.parent.position.x && _right)
            {
                RotateCher();
                _left = true;
                _right = false;
            }
            if (_soldierPos.x < transform.parent.position.x && _left)
            {
                RotateCher();
                _left = false;
                _right = true;
            }
            transform.rotation = _rotation;
            _soldierPos = transform.parent.position;
        }
    }
}