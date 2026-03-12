using System;
using GraphicsLabor.Scripts.Attributes.LaborerAttributes.InspectedAttributes;
using SpaceInvaderTemplate;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Invader : DamageableBase
{
    private static readonly int VomitAnimName = Animator.StringToHash("Vomis");
    private static readonly int PoopAnimName = Animator.StringToHash("Caca");
    
    [SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private Transform shootAt = null;
    [SerializeField] private string collideWithTag = "Player";

    [SerializeField] private GameObject burstDieVomit;
    [SerializeField] private GameObject burstDiePoop;

    private E_INVADERSTATE _state;

    internal Action<Invader> onDestroy;
    [SerializeField] private Animator _animator;

    [Header("Audio")]
    [SerializeField] private SoundPlayer _shootSound;
    [SerializeField] private SoundPlayer _poopShootSound;
    [SerializeField] private SoundPlayer _damagedSound;

    public Vector2Int GridIndex { get; private set; }

    private E_INVADERSTATE State {
        get { return _state; } 
        set { 
            _state = value;

            if (_animator == null)
            {
                return;
            }
            // state_vomis = b 010;
            // state_poop = b 100;
            // _state = b 110;
            // state_vomis & _state => 010 & 010
            // -> 0 & 1 = 0
            // -> 1 & 1 = 1
            // -> 0 & 0 = 0
            // state_poop & _state => 100 & 010
            // -> 1 & 1 = 1
            // -> 0 & 1 = 0
            // -> 0 & 0 = 0
            _animator.SetBool(VomitAnimName, HasStatus(E_INVADERSTATE.VOMIT));
            _animator.SetBool(PoopAnimName, HasStatus(E_INVADERSTATE.POOP));
        }
    }

    private void Awake()
    {
        DamageTaken += OnDamageTaken;

        _animator = GetComponentInChildren<Animator>();

        _animator.SetInteger("Invader_Type", Random.Range(0, 3));
    }

    private void OnDamageTaken()
    {
        _damagedSound.PlayOneShotSound();
        _animator.SetTrigger("Hit");
    }

    public void Initialize(Vector2Int gridIndex)
    {
        GridIndex = gridIndex;
    }

    public void OnDestroy()
    {
        onDestroy?.Invoke(this);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag(collideWithTag)) { return; }

        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            State |= bullet.StatusType;
        }

        TakeDamage(1);
        //Destroy(collision.gameObject);
    }

    protected override void OnDeath()
    {
        _animator.SetBool("Death", true);       
    }

    public void Die()
    {
        if (burstDiePoop != null && HasStatus(E_INVADERSTATE.VOMIT)) Instantiate(burstDieVomit, transform.position, Quaternion.identity);
        if (burstDiePoop != null && HasStatus(E_INVADERSTATE.POOP)) Instantiate(burstDiePoop, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    public void Shoot()
    {
        if (_shootSound != null && ((_state & E_INVADERSTATE.POOP) > 0)) _poopShootSound.PlayOneShotSound();
        else if (_shootSound != null) _shootSound.PlayOneShotSound();

        Instantiate(bulletPrefab, shootAt.position, Quaternion.identity);
    }

    public void ApplyStatus(E_INVADERSTATE status)
    {
        State |= status;
    }

    public bool HasStatus(E_INVADERSTATE status)
    {
        return (_state & status) > 0;
    }
}
