﻿using Assets.Scripts.Behaviour;
using Assets.Scripts.Util;
using Assets.Scripts.Util.Abstract;
using Assets.Scripts.Util.Interfaces;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class HealthBar : Photon.MonoBehaviour, IHealth
    {
        public Soldier Soldier;
        public Transform ForegroundSprite;
        public SpriteRenderer ForegroundRenderer;
        public Color MaxHealthColor = new Color(64 / 255f, 137 / 255f, 255 / 255f);
        public Color MinHealthColor = new Color(255 / 255f, 63 / 255f, 63 / 255f);
        [HideInInspector] public float MaxHealth;

        private float _curHealth;


        void Start()
        {

        }

        public void Launch()
        {
            MaxHealth = Soldier.MaxHealth;
            _curHealth = MaxHealth;
            Soldier.Health = this;
            UpdateHealth();
        }

        public void TakeDamage(float damage)
        {
            if (Soldier.CheckIfMiss())
            {
                FloatingText.Show(string.Format("Miss"), "Miss",
                new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60f));
                return;
            }
            float afterCalDamage = Mathf.Round(damage - (damage/Soldier.Armor));
            FloatingText.Show(string.Format("-{0}", afterCalDamage), "PlayerTakeDamageText",
                new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60f));
            float result = _curHealth - afterCalDamage;
            if (result < 0)
                result = 0;
            _curHealth = result;
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