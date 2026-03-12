using System;
using UnityEngine;

namespace SpaceInvaderTemplate.Scripts
{
    public class InvaderDeathEventScript : MonoBehaviour
    {
        private Invader _invaderScript;

        private void Awake()
        {
            _invaderScript = transform.parent.GetComponent<Invader>();
            if (_invaderScript == null)
            {
                throw new Exception("Invader Script is missing from parent");
            }
        }

        public void Die()
        {
            _invaderScript.Die();
        }
    }
}