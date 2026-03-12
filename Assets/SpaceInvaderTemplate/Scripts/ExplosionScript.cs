using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace SpaceInvaderTemplate.Scripts
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class ExplosionScript : MonoBehaviour
    {
        [SerializeField] private Vector2 _minMaxSize;
        [SerializeField] private float _timeToMax;
        [SerializeField] private float _decayTime;
        private float _timer;
        
        // small optimization
        private readonly List<Collider2D> _collidedWith = new ();
        
        private CircleCollider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _collider.isTrigger = true;
            _collider.radius = _minMaxSize.x;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            _collider.radius = Mathf.Lerp(_minMaxSize.x, _minMaxSize.y, _timer / _timeToMax);
            
            // ====== Begin vomiting at this code ======
            // Yeah so we have to do this because on trigger enter doesn't work for some reason, not optimal AT ALL
            // but yeah, it'll do
            var colliders = new List<Collider2D>();
            _collider.Overlap(colliders);

            foreach (var other in colliders)
            {
                if (_collidedWith.Contains(other))
                {
                    continue;
                }
                Debug.Log(other.name);
                if (other.GetComponent<Invader>() is { } invader)
                {
                    Debug.Log("Applying poop");
                    invader.ApplyStatus(E_INVADERSTATE.POOP);
                    _collidedWith.Add(other);
                }
            }
            // ====== you should be able to stop now ======
            
            if (_timer >= _timeToMax + _decayTime)
            {
                Destroy(gameObject);
            }
            else if (_timer >= _timeToMax && _collider.enabled)
            {
                _collider.enabled = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Invader>() is { } invader)
            {
                invader.ApplyStatus(E_INVADERSTATE.POOP);
            }
        }
    }
}