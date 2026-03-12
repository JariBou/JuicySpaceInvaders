using UnityEngine;

public class WeakBullet : Bullet
{

    [Header("USE THIS SOUNDS FOR THIS BULLET")]
    [SerializeField] private SoundPlayer _upSound;
    [SerializeField] private SoundPlayer _downSound;

    private void Start() 
    { 
        _rb.gravityScale = 1.0f;
        _upSound.PlaySound();
    }

    public override void Update()
    {
        base.Update();
        if (_rb.linearVelocity.y < 0)
        {
            tag = "Untagged";
            _downSound.PlaySound();
        }
    }
}