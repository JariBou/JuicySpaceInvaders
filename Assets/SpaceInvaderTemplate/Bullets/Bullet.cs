using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected Vector3 startVelocity;
    [SerializeField] private string destroyCollider;

    [SerializeField] protected ParticleSystem _loopPS;
    [SerializeField] protected GameObject _impactPSPrefab;

    protected Rigidbody2D _rb;

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.linearVelocity = startVelocity;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Untagged")
        {
            _loopPS.Stop();

            Vector3 dir = collision.transform.position - transform.position;


            /*if (_rb.linearVelocity.y >= 0) Instantiate(_impactPSPrefab, transform.position, Quaternion.identity);
            else */
            Instantiate(_impactPSPrefab, transform.position, Quaternion.FromToRotation(transform.forward, dir));
            Destroy(gameObject);
        }
    }
}
