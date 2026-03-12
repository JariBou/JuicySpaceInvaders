using System.Collections;
using UnityEngine;

namespace SpaceInvaderTemplate.Scripts.UI
{
    public class InvaderHealthDisplayScript : MonoBehaviour
    {
        private static readonly int PercentMatPropertyName = Shader.PropertyToID("_Percent");
        private static readonly int AlphaMatPropertyName = Shader.PropertyToID("_Alpha");
        
        [SerializeField] private Invader _invader;
        [Header("TakeDamage Animation")]
        [SerializeField] private Material _material;
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private AnimationCurve _takeDmgAnimCurve =  new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        [SerializeField] private float _animTime = 0.5f;
        [Header("FadeOut Animation")]
        [SerializeField] private float _fadeOutDelay = 1f;
        [SerializeField] private AnimationCurve _fadeOutCurve =  new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        [SerializeField] private float _minAlpha = 0.4f;
        [SerializeField] private float _fadeOutTime = 0.5f;
        
        private float _targetPercentHp;
        private float _currentPercentHp;
        private float _timerAccu;
        
        private float _fadeOutAccu;
        private IEnumerator _doFadeOutAnim;

        private void Awake()
        {
            _sr.material = new Material(_material);
            _sr.enabled = false;
            _currentPercentHp = _invader.GetPercentHp();
            _sr.material.SetFloat(PercentMatPropertyName, _currentPercentHp);
        }

        private void OnEnable()
        {
            _invader.DamageTaken += InvaderOnDamageTaken;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _invader.DamageTaken -= InvaderOnDamageTaken;
        }
        
        private void InvaderOnDamageTaken()
        {
            _sr.enabled = true;
            _targetPercentHp = _invader.GetPercentHp();
            _currentPercentHp = _sr.material.GetFloat(PercentMatPropertyName);
            _sr.material.SetFloat(AlphaMatPropertyName, 1);
            if (_doFadeOutAnim != null)
            {
                StopCoroutine(_doFadeOutAnim);
            }
            StartCoroutine(DoTakeDmgAnim());
        }

        private IEnumerator DoTakeDmgAnim()
        {
            _timerAccu = 0;
            while (_timerAccu < _animTime)
            {
                _timerAccu += Time.deltaTime / _animTime;
                float t = _takeDmgAnimCurve.Evaluate(_timerAccu);
                _sr.material.SetFloat(PercentMatPropertyName, Mathf.Lerp(_currentPercentHp, _targetPercentHp, t));
                yield return new WaitForEndOfFrame();
            }

            _doFadeOutAnim = DoFadeOutAnim();
            StartCoroutine(_doFadeOutAnim);
        }
        
        private IEnumerator DoFadeOutAnim()
        {
            _fadeOutAccu = 0;
            while (_fadeOutAccu < (_fadeOutTime+_fadeOutDelay))
            {
                _fadeOutAccu += Time.deltaTime / _fadeOutTime;
                float t = _fadeOutCurve.Evaluate(Mathf.Max(_fadeOutAccu-_fadeOutDelay, 0));
                _sr.material.SetFloat(AlphaMatPropertyName, Mathf.Lerp(1, _minAlpha, t));
                yield return new WaitForEndOfFrame();
            }
        }
    }
}