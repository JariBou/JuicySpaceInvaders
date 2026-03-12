using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaderTemplate.Scripts.UI
{
    public class InvaderHealthDisplayScript : MonoBehaviour
    {
        private static readonly int PercentMatPropertyName = Shader.PropertyToID("_Percent");
        
        [SerializeField] private Material _material;
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private Invader _invader;
        [SerializeField] private AnimationCurve _takeDmgAnimCurve =  new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        [SerializeField] private float _animTime = 0.5f;
        
        private float _targetPercentHp;
        private float _currentPercentHp;
        private float _timerAccu;

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
            _timerAccu = 0;
            StartCoroutine(DoTakeDmgAnim());
        }

        private IEnumerator DoTakeDmgAnim()
        {
            while (_timerAccu < _animTime)
            {
                _timerAccu += Time.deltaTime / _animTime;
                float t = _takeDmgAnimCurve.Evaluate(_timerAccu);
                _sr.material.SetFloat(PercentMatPropertyName, Mathf.Lerp(_currentPercentHp, _targetPercentHp, t));
                yield return new WaitForEndOfFrame();
            }
        }
    }
}