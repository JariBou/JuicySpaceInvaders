using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected Vector3 startVelocity;
    [SerializeField] private string destroyCollider;
    protected Rigidbody2D _rb;

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.linearVelocity = startVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != destroyCollider) return;
        Destroy(gameObject);
    }
}
