using System;
using UnityEngine;

namespace SpaceInvaderTemplate
{
    public class DamageableBase : MonoBehaviour
    {
        public event Action DamageTaken;
        
        [SerializeField, Range(1, 10)] private int _lifeAmount = 3;
        public int MaxLife { get; protected set; }
        
        public bool DeathTriggered { get; protected set; }

        public virtual void Awake()
        {
            MaxLife = LifeAmount;
        }

        public int LifeAmount
        {
            get => _lifeAmount;
            protected set => _lifeAmount = value;
        }

        public void SetLifeAmount(int lifeAmount)
        {
            LifeAmount = lifeAmount;
        }
        
        public void SetMaxLifeAmount(int lifeAmount)
        {
            MaxLife = lifeAmount;
        }

        public void TakeDamage(int damage)
        {
            LifeAmount -= damage;
            DamageTaken?.Invoke();
            CheckDeath();
        }

        protected void CheckDeath()
        {
            if (DeathTriggered || !IsDead())
            {
                return;
            }
            OnDeath();
        }

        protected virtual void OnDeath()
        {
        }

        protected bool IsDead()
        {
            return LifeAmount <= 0;
        }

        public float GetPercentHp()
        {
            return (float)LifeAmount / MaxLife;
        }
    }
}