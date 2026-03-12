using System;
using System.Collections.Generic;
using GraphicsLabor.Scripts.Core.Utility;
using UnityEngine;

public enum FeelFeature
{
    BulletApplyStatus,
    BulletType_Explosion,
    BulletType_Weak,
}

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public enum DIRECTION { Right = 0, Up = 1, Left = 2, Down = 3 }

    public static GameManager Instance = null;

    [SerializeField] private Vector2 bounds;
    private Bounds Bounds => new Bounds(transform.position, new Vector3(bounds.x, bounds.y, 1000f));


    [SerializeField] private float gameOverHeight;

    public List<Invader> invadersList = new List<Invader>();
    
    // Disclaimer: this is still in development and doesn't seem to work in builds for whatever reason so be careful
    [SerializeField] private SerializedDictionary<FeelFeature, bool> _feelFeatures = new();
    public SerializedDictionary<FeelFeature, bool> FeelFeatures => _feelFeatures;

    void Awake()
    {
        Instance = this;
        #if !UNITY_EDITOR
        foreach (FeelFeature feelFeature in Enum.GetValues(typeof(FeelFeature)))
        {
            _feelFeatures.TryAdd(feelFeature, false);
        }
        #endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            FeelFeatures[FeelFeature.BulletApplyStatus] = !FeelFeatures[FeelFeature.BulletApplyStatus];
        }
    }

    public Vector3 KeepInBounds(Vector3 position)
    {
        return Bounds.ClosestPoint(position);
    }

    public float KeepInBounds(float position, DIRECTION side)
    {
        switch (side)
        {
            case DIRECTION.Right: return Mathf.Min(position, Bounds.max.x);
            case DIRECTION.Up: return Mathf.Min(position, Bounds.max.y);
            case DIRECTION.Left: return Mathf.Max(position, Bounds.min.x);
            case DIRECTION.Down: return Mathf.Max(position, Bounds.min.y);
            default: return position;
        }
    }

    public bool IsInBounds(Vector3 position)
    {
        return Bounds.Contains(position);
    }

    public bool IsInBounds(Vector3 position, DIRECTION side)
    {
        switch (side)
        {
            case DIRECTION.Right: case DIRECTION.Left: return IsInBounds(position.x, side);
            case DIRECTION.Up: case DIRECTION.Down: return IsInBounds(position.y, side);
            default: return false;
        }
    }

    public bool IsInBounds(float position, DIRECTION side)
    {
        switch (side)
        {
            case DIRECTION.Right: return position <= Bounds.max.x;
            case DIRECTION.Up: return position <= Bounds.max.y;
            case DIRECTION.Left: return position >= Bounds.min.x;
            case DIRECTION.Down: return position >= Bounds.min.y;
            default: return false;
        }
    }

    public bool IsBelowGameOver(float position)
    {        
        return position < transform.position.y + (gameOverHeight - bounds.y * 0.5f);
    }


    /// <summary>
    /// Gives bound corners coordinate in order (index) : (0)bottom-left, (1)top-left, (2)bottom-right, (3)top-right
    /// </summary>
    /// <returns></returns>
    public Vector2[] GetBoundCornersPositions()
    {
        Vector2[] boundCorners = new Vector2[4];

        boundCorners[0] = new Vector2(Bounds.min.x, Bounds.min.y);
        boundCorners[1] = new Vector2(Bounds.min.x, Bounds.max.y);
        boundCorners[2] = new Vector2(Bounds.max.x, Bounds.min.y);
        boundCorners[3] = new Vector2(Bounds.max.x, Bounds.max.x);

        return boundCorners;
    }

    public void PlayGameOver()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0f;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position, new Vector3(bounds.x, bounds.y, 0f));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            transform.position + Vector3.up * (gameOverHeight - bounds.y * 0.5f) - Vector3.right * bounds.x * 0.5f,
            transform.position + Vector3.up * (gameOverHeight - bounds.y * 0.5f) + Vector3.right * bounds.x * 0.5f);
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (FeelFeature feelFeature in Enum.GetValues(typeof(FeelFeature)))
        {
            _feelFeatures.TryAdd(feelFeature, false);
        }
    }
    #endif
    public static bool GetFeatureState(FeelFeature feature)
    {
        if (Instance == null)
        {
            throw new NullReferenceException("Instance of GameManager is null");
        }

        if (!Instance.FeelFeatures.ContainsKey(feature))
        {
            Debug.LogError("Feel feature not found: " + feature);
            return false;
        }
        return Instance.FeelFeatures[feature];
    }
}
