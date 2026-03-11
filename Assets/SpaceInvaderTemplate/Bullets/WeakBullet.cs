using System.Collections;
using UnityEngine;

public class WeakBullet : Bullet
{
    [SerializeField] private CircleCollider2D Collider2D;

    private float waitTimeCollisionActivation = 0.5f;
    

    private void Start() 
    { 
        _rb.gravityScale = 1.0f;
    }

    private void Update()
    {
        waitTimeCollisionActivation -= Time.deltaTime;
        if (waitTimeCollisionActivation < 0) Collider2D.enabled = true;

        if(_rb.linearVelocity.y < 0) tag = "Untagged";
    }
}