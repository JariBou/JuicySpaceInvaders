using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Vector3 startVelocity;
    [SerializeField] private string destroyCollider;

    // Start is called before the first frame update
    void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = startVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != destroyCollider) return;
        Destroy(gameObject);
    }
}
