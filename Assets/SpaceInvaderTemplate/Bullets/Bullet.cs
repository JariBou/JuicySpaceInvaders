using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BType
    {
        Weak,
        Explosive,
        Vomit
    }
    [SerializeField] protected Vector3 startVelocity;
    [SerializeField] private string destroyCollider;

    [Header("Bullet type")]
    [SerializeField]
    private E_INVADERSTATE state;

    [Header("Particle systems")]
    [SerializeField] protected ParticleSystem _loopPS;
    [SerializeField] protected GameObject _impactPSPrefab;
    [SerializeField] protected ParticleSystem _fadeOutPS;

    [Header("Collider")]
    [SerializeField] protected CircleCollider2D _collider;

    [Header("Audio")]
    [SerializeField] protected SoundPlayer _soundPlayer;

    protected Rigidbody2D _rb;
    private float waitTimeCollisionActivation = 0.25f;

    public E_INVADERSTATE StatusType => state;
    
    [SerializeField] private BType _bulletType;
    public BType BulletType => _bulletType;


    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.linearVelocity = startVelocity;

        state = GameManager.GetFeatureState(FeelFeature.BulletApplyStatus) ? state : E_INVADERSTATE.NONE;
        _soundPlayer.PlaySound();
    }

    private void Update()
    {
        waitTimeCollisionActivation -= Time.deltaTime;
        if (waitTimeCollisionActivation < 0) _collider.enabled = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Untagged"))
        {
            _loopPS.Stop();

            Vector3 dir = collision.transform.position - transform.position;

            _rb.linearVelocity = Vector3.zero;

            /*if (_rb.linearVelocity.y >= 0) Instantiate(_impactPSPrefab, transform.position, Quaternion.identity);
            else */
            Instantiate(_impactPSPrefab, transform.position, Quaternion.FromToRotation(transform.forward, dir));
            StartCoroutine(DestroyDefered());
        }
    }

    private IEnumerator DestroyDefered()
    {
        float time = 0.0f;

        while(time < _fadeOutPS.main.duration)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
