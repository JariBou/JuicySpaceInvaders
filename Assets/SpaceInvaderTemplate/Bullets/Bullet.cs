using System.Collections;
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
    [SerializeField] private BType _bulletType;

    [Header("Particle systems")]
    [SerializeField] protected ParticleSystem _loopPS;
    [SerializeField] protected GameObject _impactPSPrefab;
    [SerializeField] protected ParticleSystem _fadeOutPS;

    [Header("Collider")]
    [SerializeField] protected CircleCollider2D _collider;

    [Header("Audio")]
    [SerializeField] protected SoundPlayer _soundPlayer;

    protected Rigidbody2D _rb;
    private float waitTimeCollisionActivation = 0.15f;

    public E_INVADERSTATE StatusType => state;
    
    public BType BulletType => _bulletType;


    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.linearVelocity = startVelocity;

        state = GameManager.GetFeatureState(FeelFeature.BulletApplyStatus) ? state : E_INVADERSTATE.NONE;
        if (_soundPlayer != null)
        {
            _soundPlayer.PlaySound();
        }
    }

    public virtual void Update()
    {
        waitTimeCollisionActivation -= Time.deltaTime;
        if (waitTimeCollisionActivation < 0) _collider.enabled = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Untagged"))
        {
            if (_loopPS != null)
            {
                _loopPS.Stop();
            }

            Vector3 dir = collision.transform.position - transform.position;

            _rb.linearVelocity = Vector3.zero;

            /*if (_rb.linearVelocity.y >= 0) Instantiate(_impactPSPrefab, transform.position, Quaternion.identity);
            else */
            if (_impactPSPrefab != null)
            {
                Instantiate(_impactPSPrefab, transform.position, Quaternion.FromToRotation(transform.forward, dir));
            }

            if (_fadeOutPS != null)
            {
                StartCoroutine(DestroyDefered());
            }
            else
            {
                Destroy(gameObject);
            }
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
