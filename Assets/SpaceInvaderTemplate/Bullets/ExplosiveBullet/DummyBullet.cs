using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DummyBullet : Bullet
{
    [Header("Random Travel Parameters")]
    [SerializeField] private int _indexCountMaximum;
    [SerializeField] private int _indexCountMinimum;
    [SerializeField] private float _speed;

    [SerializeField] private GameObject _explosionPrefab;

    private List<Vector2> _positionsList = new List<Vector2>();

    private Coroutine _coroutine;
    private Invader _ennemyToTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _collider.enabled = false;

        Vector2[] gameBounds = GameManager.Instance.GetBoundCornersPositions();

        foreach (Vector2 position in gameBounds)
        {
            Debug.Log(position);
        }

        int randomIndexCount = Random.Range(_indexCountMinimum, _indexCountMaximum);

        for(int i = 0; i < randomIndexCount; i++)
        {
            Vector2 randomPos = new(Random.Range(gameBounds[0].x, gameBounds[2].x), Random.Range(gameBounds[0].y, gameBounds[1].y));

            while(!GameManager.Instance.IsInBounds(randomPos))
            {
                Debug.Log("Reroll random pos");
                randomPos = new Vector2(Random.Range(gameBounds[0].x, gameBounds[2].x), Random.Range(gameBounds[0].y, gameBounds[1].y));
            }

            _positionsList.Add(randomPos);
        }
        _positionsList.Add(Vector2.zero);

        _coroutine = StartCoroutine(MoveToPointCoroutine());
        PickRandomEnnemy();
        _positionsList[^1] = _ennemyToTarget.transform.position;

        _soundPlayer.StopSound();
        _soundPlayer.PlaySound();
    }

    private void Update()
    {
        if(_positionsList.Count > 0)
        {
            if (_ennemyToTarget == null) PickRandomEnnemy();

            _positionsList[^1] = _ennemyToTarget.transform.position;
        }
    }

    IEnumerator MoveToPointCoroutine()
    {
        while(Vector3.Distance(transform.position, _positionsList[0]) > 0.1f)
        {
            Vector3 direction = (Vector3)_positionsList[0] - transform.position;
            direction.Normalize();
            startVelocity = new Vector3(direction.x * _speed, direction.y * _speed, direction.z * _speed);
            _rb.linearVelocity = startVelocity;
            Vector3.Lerp(transform.position, _positionsList[0], Time.deltaTime * (startVelocity.x + startVelocity.y));
            yield return new WaitForEndOfFrame();
        }

        _positionsList.RemoveAt(0);

        if(_positionsList.Count > 0)
        {
            StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(MoveToPointCoroutine());
        }
        else
        {
            _collider.enabled = true;
            if (_explosionPrefab)
            {
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    private void PickRandomEnnemy()
    {
        int randomIndex = Random.Range(0, GameManager.Instance.invadersList.Count);

        _ennemyToTarget = GameManager.Instance.invadersList[randomIndex];
    }

    private void OnValidate()
    {
        if (_indexCountMinimum <= 0) _indexCountMinimum = 1;
        if (_indexCountMaximum < _indexCountMinimum) _indexCountMaximum = _indexCountMinimum;
    }
}
