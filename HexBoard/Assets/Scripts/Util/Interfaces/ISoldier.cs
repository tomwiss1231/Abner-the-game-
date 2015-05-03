using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Util.Interfaces
{
    public interface ISoldier
    {
        IEnumerator Demage(ISoldier enemy); //TODO:Change the function to an array or overload the function.
        void Walk(Stack<Tile> path);
        bool IsMoving();
        void WalkRadius();
        void ShowHitRange();
        void SpecialHit(ISoldier enemy);
        void ClearAll();
        IHealth GetHealth();
    }
}



