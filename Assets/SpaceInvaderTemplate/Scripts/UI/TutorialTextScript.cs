using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceInvaderTemplate.Scripts.UI
{
    [RequireComponent(typeof(Animator))]
    public class TutorialTextScript : MonoBehaviour
    {
        private static readonly int BounceAnimName = Animator.StringToHash("Bounce");
        
        private Animator _animator;
        [SerializeField] private Vector2 _minMaxRandTime = new Vector2(5, 12);
        private float _timer;
        private float _selectedTime;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            _timer += Time.fixedDeltaTime;
            if (_timer >= _selectedTime)
            {
                _animator.SetTrigger(BounceAnimName);
                _timer = 0;
                _selectedTime = Random.Range(_minMaxRandTime.x, _minMaxRandTime.y);
            }
        }
    }
}