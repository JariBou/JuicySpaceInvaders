using System.Collections;
using System.Collections.Generic;
using SpaceInvaderTemplate;
using SpaceInvaderTemplate.Utils;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private float deadzone = 0.3f;
    [SerializeField] private float speed = 1f;

    [SerializeField] private List<Bullet> bulletPrefabList = new List<Bullet>();
    [SerializeField] private Transform shootAt = null;
    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private string collideWithTag = "Untagged";

    private float lastShootTimestamp = Mathf.NegativeInfinity;
    [SerializeField, Range(1, 10)] private int _lifeAmount;
    private bool _deathTriggered;

    void Update()
    {
        UpdateMovement();
        UpdateActions();
    }

    void UpdateMovement()
    {
        float move = Input.GetAxis("Horizontal");
        if (Mathf.Abs(move) < deadzone) { return; }

        move = Mathf.Sign(move);
        float delta = move * speed * Time.deltaTime;
        transform.position = GameManager.Instance.KeepInBounds(transform.position + Vector3.right * delta);
    }

    void UpdateActions()
    {
        if (    Input.GetKey(KeyCode.Space) 
            &&  Time.time > lastShootTimestamp + shootCooldown )
        {
            Shoot();
        }
    }

    void Shoot()
    {
        int randomBulletIndex = Random.Range(0, bulletPrefabList.Count);

        Instantiate(bulletPrefabList[randomBulletIndex], shootAt.position, Quaternion.identity);
        lastShootTimestamp = Time.time;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != collideWithTag) { return; }

        GameManager.Instance.PlayGameOver();
    }

    int IDamageable.LifeAmount
    {
        get => _lifeAmount;
        set => _lifeAmount = value;
    }

    bool IDamageable.DeathTriggered
    {
        get => _deathTriggered;
        set => _deathTriggered = value;
    }

    public void OnDeath()
    {
        
    }
}
