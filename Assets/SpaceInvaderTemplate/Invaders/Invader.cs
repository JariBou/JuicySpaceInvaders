using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private Transform shootAt = null;
    [SerializeField] private string collideWithTag = "Player";

    private E_INVADERSTATE _state;

    internal Action<Invader> onDestroy;

    public Vector2Int GridIndex { get; private set; }

    private E_INVADERSTATE State {
        get { return _state; } 
        set { 
            _state = value;

            //CHANGER LES SPRITES/ANIMS ICI
            switch(_state)
            {
                case E_INVADERSTATE.CLEAN:
                case E_INVADERSTATE.VOMIT:
                case E_INVADERSTATE.POOP:
                case E_INVADERSTATE.BOTH:
                default:
                    break;
            }
        }
    }

    public void Initialize(Vector2Int gridIndex)
    {
        this.GridIndex = gridIndex;
    }

    public void OnDestroy()
    {
        onDestroy?.Invoke(this);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != collideWithTag) { return; }

        Destroy(gameObject);
        //Destroy(collision.gameObject);
    }

    public void Shoot()
    {
        Instantiate(bulletPrefab, shootAt.position, Quaternion.identity);
    }
}
