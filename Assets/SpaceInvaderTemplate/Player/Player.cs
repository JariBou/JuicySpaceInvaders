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
    [SerializeField] private List<int> bulletsProbabilities = new List<int>();
    [SerializeField] private List<AudioClip> bulletSounds = new List<AudioClip>();
    [SerializeField] private Transform shootAt = null;
    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private string collideWithTag = "Untagged";
    [SerializeField] private float _rotateAngle = 30f;
    [SerializeField] private float _rotateTime = 0.3f;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSourceBulletLaunch;
    [SerializeField] private AudioSource _audioSourceDamageTaken;
    [SerializeField] private AudioClip _damageTaken;

    private float lastShootTimestamp = Mathf.NegativeInfinity;
    private bool _deathTriggered;

    private void OnEnable()
    {
        DamageTaken += OnDamageTaken;
    }

    private void OnDisable()
    {
        DamageTaken -= OnDamageTaken;
    }

    private void OnDamageTaken()
    {
        _audioSourceDamageTaken.Play();
    }

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
        // float totalProbabilites = 0.0f;
        //
        // foreach (float elem in bulletsProbabilities)
        // {
        //     totalProbabilites += elem;
        // }
        //
        // float randomPick = Random.value * totalProbabilites;
        //
        // int randomBulletIndex = 0;
        //
        // for (int i = 0; i < bulletsProbabilities.Count; i++)
        // {
        //     if (randomPick < bulletsProbabilities[i])
        //     {
        //         randomBulletIndex = i;
        //         break;
        //     }
        //
        //     randomPick -= bulletsProbabilities[i];
        // }

        List<Bullet> prefabs = new List<Bullet>();
        for (int i = 0; i < bulletPrefabList.Count; i++)
        {
            if (IsBulletTypeEnabled(bulletPrefabList[i].BulletType))
            {
                for (int j = 0; j < bulletsProbabilities[i]; j++)
                {
                    prefabs.Add(bulletPrefabList[i]);
                }
            }
        }

        // prefabs.Shuffle();

        Instantiate(prefabs[Random.Range(0, prefabs.Count)], shootAt.position, Quaternion.identity);
        
        if (bulletSounds[randomBulletIndex] != null) _audioSourceBulletLaunch.PlayOneShot(bulletSounds[randomBulletIndex]);

        lastShootTimestamp = Time.time;
    }

    public bool IsBulletTypeEnabled(Bullet.BType bType)
    {
        switch (bType)  
        {
            case Bullet.BType.Weak:
                return GameManager.GetFeatureState(FeelFeature.BulletType_Weak);
            case Bullet.BType.Explosive:
                return GameManager.GetFeatureState(FeelFeature.BulletType_Explosion);
            case Bullet.BType.Vomit:
                return true;
            default:
                throw new ArgumentOutOfRangeException(nameof(bType), bType, null);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(collideWithTag)) { return; }

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

        while (bulletSounds.Count < bulletPrefabList.Count) bulletSounds.Add(null);

        while (bulletSounds.Count > bulletPrefabList.Count) bulletSounds.RemoveAt(bulletsProbabilities.Count - 1);
    }
}
