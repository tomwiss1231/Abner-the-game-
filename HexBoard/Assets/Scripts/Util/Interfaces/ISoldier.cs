using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Util.Abstract;
using UnityEngine;

namespace Assets.Scripts.Util.Interfaces
{
    public interface ISoldier
    {
        IEnumerator Damage(int oponId); //TODO:Change the function to an array or overload the function.
        void SavePath(Stack<TileBehaviour> path);
        bool IsMoving();
        void WalkRadius();
        IEnumerator ShowHitRange();
        void SpecialHit(int oponId);
        void ClearAll();
        IHealth GetHealth();
        void GotHit();
        void Die();
    }
}



