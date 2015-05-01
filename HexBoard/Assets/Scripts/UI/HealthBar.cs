using Assets.Scripts.Behaviour;
using Assets.Scripts.Util.Abstract;
using Assets.Scripts.Util.Interfaces;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class HealthBar : MonoBehaviour, IHealth
    {
        public Soldier Soldier;
        public Transform ForegroundSprite;
        public SpriteRenderer ForegroundRenderer;
        public Color MaxHealthColor = new Color(64 / 255f, 137 / 255f, 255 / 255f);
        public Color MinHealthColor = new Color(255 / 255f, 63 / 255f, 63 / 255f);
        public float MaxHealth = 100;

        private float _curHealth;


        void Start()
        {
            _curHealth = MaxHealth;
            Soldier.Health = this;
            UpdateHealth();
        }

        public void TakeDemage(float demage)
        {
            float result = _curHealth - (demage / MaxHealth);
            if (result < 0)
                result = 0;
            _curHealth = result;
            print(_curHealth + "%");
            UpdateHealth();
        }

        public bool IsAlive()
        {
            return _curHealth > 0;
        }

        public float GetTotalHealth()
        {
            return MaxHealth;
        }

        public float GetCurrentHealth()
        {
            return _curHealth;
        }

        public void AddHealth(float healPoints)
        {
            float result = _curHealth + healPoints;
            if (result > MaxHealth)
                result = MaxHealth;
            _curHealth = result;
            UpdateHealth();
        }

        public void UpdateHealth()
        {
            float healthPercent = _curHealth/MaxHealth;
            ForegroundSprite.localScale = new Vector3(healthPercent, 1 , 1);
            ForegroundRenderer.color = Color.Lerp(MinHealthColor, MaxHealthColor, healthPercent);
        }

        public void RestoreHealth()
        {
            _curHealth = MaxHealth;
            UpdateHealth();
        }
    }
}