

using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Behaviour;
using Assets.Scripts.UI;
using Assets.Scripts.Util.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Util.Abstract
{
    public abstract  class Soldier : MonoBehaviour, ISoldier, IObserverble, ISoldierObserver
    {
        [SerializeField] private const float MinNextTileDist = 0.1f;

        [SerializeField] private Stack<Tile> _walkPosition;
        [SerializeField, HideInInspector] private bool _isMoving;
        [SerializeField, HideInInspector] private bool _rotateAndStop;
        [SerializeField, HideInInspector] private Tile _curTile;
        [SerializeField, HideInInspector] private Vector3 _curTilePos;
        [SerializeField, HideInInspector] private List<ISoldierObserver> _playerObservers;
        
        [SerializeField, HideInInspector] protected Buff Buff;
        [SerializeField, HideInInspector] private Player _player;
        [SerializeField, HideInInspector] protected List<Buff> Buffs;
        
        [SerializeField] public int NumberOfAttacks;
        [SerializeField] public int MissPrecent;
        [SerializeField] public GameObject HealthPrefeb;
        [SerializeField] public GameObject SkillBarPrefeb;
        [SerializeField] public GameObject ReyPrefeb;
        [SerializeField] public int CriticalPrecent;
        [SerializeField] public int MinDamege;
        [SerializeField] public int MaxDamege;
        [SerializeField] public int Armor;
        [SerializeField] public int WalkRange;
        [SerializeField] public int HitRange;
        [SerializeField] public int SpecialHitParameter;
        [SerializeField] public int CriticalHit;
        [SerializeField] public int MaxHealth;
        [SerializeField] public int MaxSkill;
        [SerializeField] public HealthBar Health;
        [SerializeField] public SkillBar SkillBar;
        [SerializeField, HideInInspector] public TileBehaviour Position;
        [SerializeField, HideInInspector] public TileBehaviour Destination = null;
        [SerializeField, HideInInspector] public bool InAttackRange;
        [SerializeField] public float WalkSpeed;
        [SerializeField] public float RotationSpeed;
        [SerializeField] public bool CheckingArea { get; private set; }
        [SerializeField] public bool IsHideing { get;  set; }
                
        public abstract bool SpecialHit(ISoldier enemy);
        public abstract void FillSkillBar();
        
        public void ClearAll()
        {
            NotifyAll();
        }

        public abstract void BuffAction(Soldier teamSoldier);

        protected virtual void Init()
        {
            _isMoving = false;
            _rotateAndStop = false;
            _playerObservers = new List<ISoldierObserver>();
            Buffs = new List<Buff>();
            _player = (GameManager.Instans.Player1.tag.Equals(tag))
                ? GameManager.Instans.Player1
                : GameManager.Instans.Player2;
            _player.AddSoldier(this);
        }


        public void GetBuffFromTeam(Buff buff)
        {
            Buffs.Add(buff);
            buff.DoBuff(this);
        }

        public IEnumerator CheckBuffs()
        {
            Stack<int> placeToDelete = new Stack<int>();
            for (int i = 0; i < Buffs.Count; i++)
            {
                Buffs[i].DecTurns();
                if (!Buffs[i].CheckIfTurn())
                {
                    placeToDelete.Push(i);
                    Buffs[i].DisableBuff(this);
                    yield return new WaitForSeconds(0.5f);
                }
            }
            foreach (var i in placeToDelete) Buffs.RemoveAt(i);
        }

        public IEnumerator ShowHitRange()
        {
            CheckingArea = true;
            var go = Instantiate(ReyPrefeb);
            go.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            go.transform.localScale = new Vector3(1, 1, HitRange);
            
            for (int i = 0; i <= 72; i++)
            {
                go.transform.RotateAround(transform.position, Vector3.up, 5);
                yield return new WaitForEndOfFrame();
            }
            
            Destroy(go);
            CheckingArea = false;
        }

        public void WalkRadius()
        {
            Position.ShowAllPaths(WalkRange);
        }

        public IEnumerator Demage(ISoldier enemy)
        {
            for (int i = 0; i < NumberOfAttacks; i++)
            {

                int demage = CalHit();
                if (CheckIfCritical())
                    demage *= CriticalHit;
                enemy.GetHealth().TakeDamage(demage);
                yield return new WaitForSeconds(0.5f);
            }
        }

        protected int CalHit()
        {
            return Random.Range(MinDamege, MaxDamege);
        }

        protected bool CheckIfCritical()
        {
            return Random.Range(0, 100) <= CriticalPrecent;
        }

        public IHealth GetHealth()
        {
            return Health;
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
            if (GridManager.instance.SelectedSoldier != null && GridManager.instance.SelectedSoldier.CheckingArea)
                return;
            if (Input.GetMouseButtonDown(0))
            {
                Soldier selectSoldier = GridManager.instance.SelectedSoldier;
                if (_player.NowPalying && selectSoldier == null) GridManager.instance.SelectedSoldier = this;
                else if (InAttackRange && !selectSoldier.IsMoving())
                {
                    if(tag == selectSoldier.tag) selectSoldier.BuffAction(this);
                    else StartCoroutine(selectSoldier.Demage(this));
                    GridManager.instance.SelectedSoldier.EndOfTurnAction();
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                Soldier selectedSoldier = GridManager.instance.SelectedSoldier;
                if (selectedSoldier == null) return;
                else if (InAttackRange && !selectedSoldier.IsMoving() && tag != selectedSoldier.tag)
                {
                    if(!selectedSoldier.SpecialHit(this)) return;
                    GridManager.instance.SelectedSoldier.EndOfTurnAction();
                }
            }
        }

        void EndOfTurnAction()
        {
            NotifyAll();
            _player.DecTurns();
            GridManager.instance.SelectedSoldier = null;
        }

        void EndOfSoldierTurn()
        {
            _player.DecTurns();
            _isMoving = false;
            _rotateAndStop = false;
            NotifyAll();
            Position.Soldier = null;
            Position = Destination;
            Destination = null;
            GridManager.instance.SelectedSoldier = null;
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
                    EndOfSoldierTurn();
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
            Init();
            
            var health = Instantiate(HealthPrefeb);
            var skill = Instantiate(SkillBarPrefeb);
            
            health.GetComponent<HealthBar>().Soldier = this;
            health.GetComponent<UiFollowObject>().Following = transform;

            skill.GetComponent<SkillBar>().Soldier = this;
            skill.GetComponent<UiFollowObject>().Following = transform;
        }
        
        void Update()
        {
            if (!Health.IsAlive())
            {
                Health.gameObject.SetActive(false);
                SkillBar.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
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
                    EndOfSoldierTurn();
                    return;
                }
            }
            MoveTowards(_curTilePos);
        }

        public void AddObserver(ISoldierObserver observer)
        {
            _playerObservers.Add(observer);
        }

        public void RemoveObserver(ISoldierObserver observer)
        {
            _playerObservers.Remove(observer);
        }

        public void NotifyAll()
        {
            foreach (ISoldierObserver soldierObserver in _playerObservers)
                soldierObserver.NotifyChange();
            _playerObservers.Clear();
        }

        void OnDisable()
        {
            Position.Soldier = null;
            Position = null;
        }

        void OnCollisionEnter(Collision collision)
        {
            try
            {
                if (collision.gameObject.tag != "HitRange") return;
                if (collision.gameObject.tag == "HitRange" && IsHideing && GridManager.instance.SelectedSoldier.tag != tag) return;
                InAttackRange = true;
                GridManager.instance.SelectedSoldier.AddObserver(this);
                if (GridManager.instance.SelectedSoldier.tag == tag)
                    Position.ChangeToBuff();
                else Position.ChangeToTarget();
            }
            catch(NullReferenceException ex) {}
        }

        public void NotifyChange()
        {
           Position.SetDefault();
        }
    }
}