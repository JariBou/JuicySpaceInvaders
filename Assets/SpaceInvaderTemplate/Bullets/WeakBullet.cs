public class WeakBullet : Bullet
{
    private void Start() 
    { 
        _rb.gravityScale = 1.0f;
    }

    public override void Update()
    {
        base.Update();
        if(_rb.linearVelocity.y < 0) tag = "Untagged";
    }
}