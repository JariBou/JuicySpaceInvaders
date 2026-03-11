using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected Vector3 startVelocity;
    [SerializeField] private string destroyCollider;

    [Header("Bullet type")]
    [SerializeField]
    private E_INVADERSTATE state;

    [Header("Particle systems")]
    [SerializeField] protected ParticleSystem _loopPS;
    [SerializeField] protected GameObject _impactPSPrefab;

    [Header("Collider")]
    [SerializeField] protected CircleCollider2D _collider;

    protected Rigidbody2D _rb;
    private float waitTimeCollisionActivation = 0.25f;

    public E_INVADERSTATE StatusType => state;


    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.linearVelocity = startVelocity;
    }

    private void Update()
    {
        waitTimeCollisionActivation -= Time.deltaTime;
        if (waitTimeCollisionActivation < 0) _collider.enabled = true;
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
