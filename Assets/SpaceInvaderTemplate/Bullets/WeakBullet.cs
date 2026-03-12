using UnityEngine;

public class WeakBullet : Bullet
{

    [Header("USE THIS SOUNDS FOR THIS BULLET")]
    [SerializeField] private SoundPlayer _upSound;
    [SerializeField] private SoundPlayer _downSound;

    private bool _isFalling = false;

    private void Start() 
    { 
        _rb.gravityScale = 1.0f;
        _upSound.PlayOneShotSound();
    }

    public override void Update()
    {
        base.Update();
        if (_rb.linearVelocity.y < 0 && !_isFalling)
        {
            _isFalling = true;

            tag = "Untagged";
            _upSound.StopSound();
            _downSound.PlayOneShotSound();
        }
    }
}