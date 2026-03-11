using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SpaceInvaderTemplate;
using SpaceInvaderTemplate.Utils;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : DamageableBase
{
    [Header("Movements")]
    [SerializeField] private float deadzone = 0.3f;
    [SerializeField] private float speed = 1f;

    [Header("Shoot")]
    [SerializeField] private List<Bullet> bulletPrefabList = new List<Bullet>();
    [SerializeField] private List<float> bulletsProbabilities = new List<float>();
    [SerializeField] private Transform shootAt = null;
    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private string collideWithTag = "Untagged";
    [SerializeField] private float _rotateAngle = 30f;
    [SerializeField] private float _rotateTime = 0.3f;

    private float lastShootTimestamp = Mathf.NegativeInfinity;
    private bool _deathTriggered;

    void Update()
    {
        UpdateMovement();
        UpdateActions();
    }

    void UpdateMovement()
    {
        float move = Input.GetAxis("Horizontal");
        if (Mathf.Abs(move) < deadzone)
        {
            if (transform.eulerAngles.z > 0.1f)
            {
                transform.DORotate(Vector3.zero, _rotateTime);
            }
            return;
        }

        move = Mathf.Sign(move);
        float delta = move * speed * Time.deltaTime;
        transform.position = GameManager.Instance.KeepInBounds(transform.position + Vector3.right * delta);
        
        Vector3 endRotateValue = new(0, 0, _rotateAngle * -move);
        transform.DORotate(endRotateValue, _rotateTime);
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
        float totalProbabilites = 0.0f;

        foreach (float elem in bulletsProbabilities)
        {
            totalProbabilites += elem;
        }

        float randomPick = Random.value * totalProbabilites;

        int randomBulletIndex = 0;

        for (int i = 0; i < bulletsProbabilities.Count; i++)
        {
            if (randomPick < bulletsProbabilities[i])
            {
                randomBulletIndex = i;
                break;
            }
            randomPick -= bulletsProbabilities[i];
        }

        Instantiate(bulletPrefabList[randomBulletIndex], shootAt.position, Quaternion.identity);
        lastShootTimestamp = Time.time;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != collideWithTag) { return; }

        TakeDamage(1);
    }

    public void OnDeath()
    {
        GameManager.Instance.PlayGameOver();
    }

    private void OnValidate()
    {
        while (bulletsProbabilities.Count < bulletPrefabList.Count) bulletsProbabilities.Add(0);

        while (bulletsProbabilities.Count > bulletPrefabList.Count) bulletsProbabilities.RemoveAt(bulletsProbabilities.Count - 1);
    }
}
