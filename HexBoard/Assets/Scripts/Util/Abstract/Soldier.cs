

using System.Collections.Generic;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Util.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Util.Abstract
{
    public abstract  class Soldier : MonoBehaviour, ISoldier
    {
        [SerializeField] private const float MinNextTileDist = 0.1f;

        [SerializeField] private Stack<Tile> _walkPosition;
        [SerializeField, HideInInspector] private bool _isMoving;
        [SerializeField, HideInInspector] private bool _rotateAndStop;
        [SerializeField, HideInInspector] private Tile _curTile;
        [SerializeField, HideInInspector] private Vector3 _curTilePos;
        
        [SerializeField] public int NumberOfAttacks;
        [SerializeField] public int MissPrecent;
        [SerializeField] public int CriticalPrecent;
        [SerializeField] public int MinDamege;
        [SerializeField] public int MaxDamege;
        [SerializeField] public int Armor;
        [SerializeField] public int WalkRange;
        [SerializeField] public int HitRange;
        [SerializeField] public int SpecialHitParameter;
        [SerializeField] public int CriticalHit;
        [SerializeField] public IHealth Health;
        [SerializeField, HideInInspector] public TileBehaviour Position;
        [SerializeField, HideInInspector] public TileBehaviour Destination = null;
        [SerializeField, HideInInspector] public bool InAttackRange;
        [SerializeField] public float WalkSpeed;
        [SerializeField] public float RotationSpeed;
                
        public abstract void SpecialHit(ISoldier enemy);

        public abstract void BuffAction(Soldier target);

        public void GetBuffFromTeam(Buff buff)
        {
            buff.DoBuff(this);
        }

        public void ShowHitRange()
        {
            Position.CalHitRange(HitRange);
        }

        public void WalkRadius()
        {
            Position.showAllPaths(WalkRange);
        }

        public void Demage(ISoldier enemy)
        {
            for (int i = 0; i < NumberOfAttacks; i++)
            {
                int demage = CalHit();
                if (CheckIfCritical())
                    demage *= CriticalHit;
                enemy.GetHealth().TakeDemage(demage);
            }
        }

        private int CalHit()
        {
            return Random.Range(MinDamege, MaxDamege);
        }

        private bool CheckIfCritical()
        {
            return Random.Range(0, 100) <= CriticalPrecent;
        }

        public IHealth GetHealth()
        {
            return Health;
        }

        public void ClearAll()
        {
            Position.Clear(WalkRange);
            Position.Clear(HitRange);
        }
        
        public bool IsMoving()
        {
            return _isMoving;
        }


        public void Walk(Stack<Tile> path)
        {
            _walkPosition = path;
            path.Pop();
            _curTile = path.Pop();
            _curTilePos = CalcTilePos(_curTile);
            _isMoving = true;
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ISoldier selectSoldier = GridManager.instance.SelectedSoldier;
                if (selectSoldier == null) GridManager.instance.SelectedSoldier = this;
                else if (selectSoldier != this && !InAttackRange)
                {
                    if (selectSoldier.IsMoving()) return;
                    selectSoldier.ClearAll();
                    GridManager.instance.SelectedSoldier = this;
                }
                else if (InAttackRange) selectSoldier.Demage(this);
            }
        }

        void EndMovement()
        {
            _isMoving = false;
            _rotateAndStop = false;
            Position.Clear(WalkRange);
            Position.Soldier = null;
            Position = Destination;
            Destination = null;
        }

        void MoveTowards(Vector3 position)
        {

            Vector3 dir = position - transform.position;

            Quaternion res = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir),
                RotationSpeed * Time.deltaTime);
            transform.rotation = res;

            Vector3 forward = transform.forward * WalkSpeed;
            float speedModifier = Vector3.Dot(dir.normalized, transform.forward);
            forward *= Mathf.Clamp01(speedModifier);
            float speedModMin = 0.97f;
            if (speedModifier > speedModMin)
            {
                if (_rotateAndStop)
                {
                    EndMovement();
                    return;
                }
                transform.position += forward * Time.deltaTime;
            }

        }


        Vector3 CalcTilePos(Tile tile)
        {
            Vector2 tileGridPos = new Vector2(tile.X + tile.Y / 2, tile.Y);
            Vector3 tilePos = GridManager.instance.calcWorldCoord(tileGridPos);
            tilePos.y = transform.position.y;
            return tilePos;
        }

        void Start()
        {
            _isMoving = false;
            _rotateAndStop = false;
        }
        
        void Update()
        {
            if (Input.GetKeyDown("d"))
            {
                Demage(this);              
            }
            if (!Health.IsAlive())
                gameObject.SetActive(false);
         
            if (!_isMoving) return;

            if ((_curTilePos - transform.position).magnitude < MinNextTileDist)
            {
                if (_walkPosition.Count >= 1)
                {
                    _curTile = _walkPosition.Pop();
                    _curTilePos = CalcTilePos(_curTile);
                }
                else
                {
                    EndMovement();
                    return;
                }
            }
            MoveTowards(_curTilePos);
        }
    }

}