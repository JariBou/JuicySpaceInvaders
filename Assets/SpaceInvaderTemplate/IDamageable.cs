using System;

namespace SpaceInvaderTemplate
{
    public interface IDamageable
    {
        public int LifeAmount { get; protected set; }
        public bool DeathTriggered { get; protected set; }

        public void SetLifeAmount(int lifeAmount)
        {
            LifeAmount = lifeAmount;
        }

        public void TakeDamage(int damage)
        {
            LifeAmount -= damage;
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

        void OnDeath();

        protected bool IsDead()
        {
            return LifeAmount <= 0;
        }
    }
}