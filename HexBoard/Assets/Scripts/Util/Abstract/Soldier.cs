

using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Behaviour;
using Assets.Scripts.UI;
using Assets.Scripts.Util.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Util.Abstract
{
    public abstract  class Soldier : MonoBehaviour, ISoldier, IObserverble, ISoldierObserver
    {
        [SerializeField] private const float MinNextTileDist = 0.1f;

        [SerializeField] private Stack<TileBehaviour> _walkPosition;
        [SerializeField, HideInInspector] private bool _isMoving;
        [SerializeField, HideInInspector] private bool _rotateAndStop;
        [SerializeField, HideInInspector] private Tile _curTile;
        [SerializeField, HideInInspector] private Vector3 _curTilePos;
        [SerializeField, HideInInspector] private List<ISoldierObserver> _playerObservers;
        
        [SerializeField, HideInInspector] protected Buff Buff;
        [SerializeField, HideInInspector] protected Player _player;
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
        [SerializeField] public Animator Controller;
        [SerializeField, HideInInspector] public TileBehaviour Position;
        [SerializeField, HideInInspector] public TileBehaviour Destination = null;
        [SerializeField, HideInInspector] public bool InAttackRange;
        [SerializeField] public float WalkSpeed;
        [SerializeField] public float RotationSpeed;
        [SerializeField] public bool CheckingArea { get; private set; }
        [SerializeField] public bool IsAttacking { get; private set; }
        [SerializeField] public bool IsGettingHit { get;  private set; }
        [SerializeField] public bool IsHideing { get;  set; }
                
        public abstract void SpecialHit(ISoldier enemy);
        public abstract void FillSkillBar();
        public abstract void ActivateBuffCast();
        
        public void ClearAll()
        {
            NotifyAll();
        }

        public abstract void BuffAction(Soldier teamSoldier);
        protected abstract string BuffName();

        protected virtual void Init()
        {
            _isMoving = false;
            _rotateAndStop = false;
            IsAttacking = false;
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
            foreach (Soldier soldier in _player.GetOpponent().GetSoldiers())
            {
                if (InRange(soldier))
                {
                    var heading = soldier.transform.position - transform.position;
                    var distance = heading.magnitude;
                    var direction = heading / distance;
                    RaycastHit hit;
                    Physics.Raycast(transform.position, direction, out hit,distance);
                    yield return new WaitForEndOfFrame();
                    if (hit.transform.gameObject.tag == "Obstacle") continue;
                    AddObserver(soldier);
                    soldier.InAttackRange = true;
                    soldier.Position.ChangeToTarget();
                }
            }
            CheckingArea = false;
        }

        private bool InRange(Soldier soldier)
        {
            float x1 = transform.position.x,
                    x2 = soldier.transform.position.x,
                    y1 = transform.position.z,
                    y2 = soldier.transform.position.z;
            double d =  Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
            return d <= HitRange;
        }

        public void WalkRadius()
        {
            Position.ShowAllPaths(WalkRange);
        }

        public IEnumerator Damage(ISoldier enemy)
        {

            IsAttacking = true;
            GridManager.instance.Action.onClick.RemoveAllListeners();
            Controller.SetBool("IsWalk", false);
            Controller.SetBool("IsIdle", false);
            Controller.SetBool("IsAttack", true);
            Controller.SetBool("IsHit", false);
            for (int i = 0; i < NumberOfAttacks; i++)
            {

                int demage = CalHit();
                if (CheckIfCritical())
                    demage *= CriticalHit;
                yield return new WaitForSeconds(0.7f);
                enemy.GetHealth().TakeDamage(demage);
                enemy.GotHit();
            }
            EndOfTurnAction();
            IsAttacking = false;
        }

        protected int CalHit()
        {
            return Random.Range(MinDamege, MaxDamege);
        }

        protected bool CheckIfCritical()
        {
            return Random.Range(0, 100) <= CriticalPrecent;
        }

        public bool CheckIfMiss()
        {
            return Random.Range(0, 100) <= MissPrecent;
        }
        public IHealth GetHealth()
        {
            return Health;
        }

        public void GotHit()
        {
            IsGettingHit = true;
            Controller.SetBool("IsWalk", false);
            Controller.SetBool("IsIdle", false);
            Controller.SetBool("IsAttack", false);
            Controller.SetBool("IsHit", true);
            IsGettingHit = false;
        }

        public bool IsMoving()
        {
            return _isMoving;
        }


        public void SavePath(Stack<TileBehaviour> path)
        {
            _walkPosition = path;
            path.Pop();
            Button action = GridManager.instance.Action;
            action.onClick.RemoveAllListeners();
            action.onClick.AddListener(StartWalking);
            action.transform.GetChild(0).GetComponent<Text>().text = "Walk";
            action.gameObject.SetActive(true);
        }

        public void StartWalking()
        {
            GridManager.instance.Action.onClick.RemoveAllListeners();
            _curTile = _walkPosition.Pop().tile;
            _curTilePos = CalcTilePos(_curTile);
            _isMoving = true;
            Controller.SetBool("IsWalk", true);
            Controller.SetBool("IsIdle", false);
            Controller.SetBool("IsAttack", false);
            Controller.SetBool("IsHit", false);
        }

        private void OnMouseOver()
        {
            if (GridManager.instance.SelectedSoldier != null && GridManager.instance.SelectedSoldier.CheckingArea)
                return;
            if (Input.GetMouseButtonDown(0))
            {
                Soldier selectSoldier = GridManager.instance.SelectedSoldier;
                if (_player.NowPalying && selectSoldier == null)
                {
                    GridManager.instance.SelectedSoldier = this;
                    WalkRadius();
                    StartCoroutine(ShowHitRange());
                    ActivateBuffCast();
                    GridManager.instance.Cancel.gameObject.SetActive(true);
                }
                else if (selectSoldier != null && (InAttackRange && !selectSoldier.IsMoving()))
                {
                    selectSoldier.ClearWalkPath();
                    Button action = GridManager.instance.Action;
                    action.onClick.RemoveAllListeners();
                    action.gameObject.SetActive(true);
                    if (tag == selectSoldier.tag)
                    {
                        action.onClick.AddListener(() => selectSoldier.BuffAction(this));
                        action.transform.GetChild(0).GetComponent<Text>().text = selectSoldier.BuffName();
                    }
                    else
                    {
                        if (transform.position.x > selectSoldier.transform.position.x)
                        {
                            float x = selectSoldier.transform.position.x,
                                y = selectSoldier.transform.position.y, 
                                z = selectSoldier.transform.position.z;
                            selectSoldier.transform.position = new Vector3(x+0.01f, y, z);
                        }
                        if (transform.position.x < selectSoldier.transform.position.x)
                        {
                            float x = selectSoldier.transform.position.x,
                                y = selectSoldier.transform.position.y,
                                z = selectSoldier.transform.position.z;
                            selectSoldier.transform.position = new Vector3(x - 0.01f, y, z);
                        }
                        action.onClick.AddListener(() => StartCoroutine(selectSoldier.Damage(this)));
                        action.transform.GetChild(0).GetComponent<Text>().text = "Attack";
                        Button specialAtk = GridManager.instance.Special;
                        specialAtk.gameObject.SetActive(true);
                        specialAtk.onClick.RemoveAllListeners();
                        specialAtk.onClick.AddListener(() => selectSoldier.SpecialHit(this));
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                Soldier selectedSoldier = GridManager.instance.SelectedSoldier;
                if (selectedSoldier == null) return;
                else if (InAttackRange && !selectedSoldier.IsMoving() && tag != selectedSoldier.tag)
                {
              
                }
            }
        }

        protected void EndOfTurnAction()
        {
            NotifyAll();
            _player.DecTurns();
            GridManager.instance.SelectedSoldier = null;
            GridManager.instance.DisableButtons();
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
            _walkPosition = null;
            GridManager.instance.SelectedSoldier = null;
            GridManager.instance.DisableButtons();
        }

        public void ResetDes()
        {
            if (_walkPosition == null) return;
            _walkPosition = null;
            Destination.Soldier = null;
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
            if (!_isMoving)
            {
                if (!IsAttacking && ! IsGettingHit)
                {
                    Controller.SetBool("IsWalk", false);
                    Controller.SetBool("IsAttack", false);
                    Controller.SetBool("IsIdle", true);
                    Controller.SetBool("IsHit", false);
                }
                return;
            }
            if ((_curTilePos - transform.position).magnitude < MinNextTileDist)
            {
                if (_walkPosition.Count >= 1)
                {
                    _curTile = _walkPosition.Pop().tile;
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
            _player.RemoveSoldier(this);
        }

        public void NotifyChange()
        {
           Position.SetDefault();
        }

        public void ClearWalkPath()
        {
            if (_walkPosition == null) return;
            Position.ChangeToBuff();
            foreach (TileBehaviour tb in _walkPosition) tb.ChangeToWalk();
            Destination.Soldier = null;

        }
    }
}