using Assets.Scripts.Util;
using Assets.Scripts.Util.Abstract;
using Assets.Scripts.Util.Interfaces;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class SkillBar : MonoBehaviour, IHealth
    {
        public Soldier Soldier;
        public Transform ForegroundSprite;
        public SpriteRenderer ForegroundRenderer;
        public Color MaxSkillColor = new Color(64 / 255f, 137 / 255f, 255 / 255f);
        public Color MinSkillColor = new Color(255 / 255f, 63 / 255f, 63 / 255f);

        private float _curSkill;
        
        [HideInInspector] public float MaxSkill;

        void Start()
        {


        }

        public bool DecSkillPoints(float points)
        {
            float prevCur = _curSkill;
            TakeDamage(points);
            return _curSkill < prevCur;
        }

        public void UpdateSkillBar()
        {
            UpdateHealth();
        }

        public float GetTotalSkill()
        {
            return GetTotalHealth();
        }

        public void AddSkillPoints(float points)
        {
            AddHealth(points);
        }

        public float GetCurrentSkillPoints()
        {
            return GetCurrentHealth();
        }

        public void RestoreSkillPoints()
        {
            RestoreHealth();
        }

        public void TakeDamage(float damage)
        {
            float result = _curSkill - damage;
            if (result < 0)
            {

                FloatingText.Show("Not enough skill points", "PlayerTakeDamageText",
                new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60f));
                return;
            }
            _curSkill = result;
            UpdateSkillBar();
        }

        public bool IsAlive()
        {
            return true;
        }

        public float GetTotalHealth()
        {
            return MaxSkill;
        }

        public float GetCurrentHealth()
        {
            return _curSkill;
        }

        public void AddHealth(float healPoints)
        {
            if(_curSkill.Equals(MaxSkill)) return;
            float result = _curSkill + healPoints;
            if (result > MaxSkill)
                result = MaxSkill;
            _curSkill = result;
            UpdateSkillBar();
        }

        public void UpdateHealth()
        {
            float healthPercent = _curSkill / MaxSkill;
            ForegroundSprite.localScale = new Vector3(healthPercent, 1, 1);
            ForegroundRenderer.color = Color.Lerp(MinSkillColor, MaxSkillColor, healthPercent);
        }

        public void RestoreHealth()
        {
            _curSkill = MaxSkill;
            UpdateSkillBar();
        }

        public void Launch()
        {
            MaxSkill = Soldier.MaxSkill;
            _curSkill = MaxSkill;
            Soldier.SkillBar = this;
            UpdateSkillBar();
        }
    }
}