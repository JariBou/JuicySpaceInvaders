using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    private static readonly int VomitAnimName = Animator.StringToHash("Vomis");
    private static readonly int PoopAnimName = Animator.StringToHash("Caca");
    
    [SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private Transform shootAt = null;
    [SerializeField] private string collideWithTag = "Player";

    private E_INVADERSTATE _state;

    internal Action<Invader> onDestroy;
    [SerializeField] private Animator _animator;

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
            _animator.SetBool(VomitAnimName, (_state & E_INVADERSTATE.VOMIT) > 0);
            _animator.SetBool(PoopAnimName, (_state & E_INVADERSTATE.POOP) > 0);
        }
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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

        Destroy(gameObject);
        //Destroy(collision.gameObject);
    }

    public void Shoot()
    {
        Instantiate(bulletPrefab, shootAt.position, Quaternion.identity);
    }
}
