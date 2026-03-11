using System;
using UnityEngine;

namespace SpaceInvaderTemplate
{
    public class DamageableBase : MonoBehaviour
    {
        public event Action DamageTaken;
        
        [SerializeField, Range(1, 10)] private int _lifeAmount = 3;
        
        public bool DeathTriggered { get; protected set; }

        public int LifeAmount
        {
            get => _lifeAmount;
            protected set => _lifeAmount = value;
        }

        public void SetLifeAmount(int lifeAmount)
        {
            LifeAmount = lifeAmount;
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
    }
}