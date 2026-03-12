using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float _autoDestroyTime = 5.0f;



    // Update is called once per frame
    void Update()
    {
        _autoDestroyTime -= Time.deltaTime;
        if( _autoDestroyTime < 0 ) Destroy(gameObject);
    }
}
