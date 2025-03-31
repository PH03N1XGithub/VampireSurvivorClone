using System;
using UnityEngine;
using DG.Tweening;

namespace C_.CharacterController
{
    public class AfterImage : MonoBehaviour
    {
        [SerializeField] private float activeTime = 0.1f;
        private float _timeActivated;
        private float _alpha;
        public AnimationCurve curve;
        [SerializeField] private float alphaSet = 0.8f;

        private Transform _player;
        private SpriteRenderer _spriteRenderer;
        private SpriteRenderer _playerSpriteRenderer;

        private Color _afterImageColor;
        private float _colorGreen;
        private float _colorBlue;

        private void OnEnable()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _player = FindObjectOfType<TopDownMovement>().transform;
            _playerSpriteRenderer = _player.GetComponentInChildren<SpriteRenderer>();
            _alpha = alphaSet;
            _spriteRenderer.sprite = _playerSpriteRenderer.sprite;
            transform.position = _player.position;
            transform.rotation = _player.rotation;
            _timeActivated = Time.time;

            DOTween.To(x => _alpha = x, 1, 0, 0.3f).SetEase(curve); 

 
            _colorGreen = 0f; 
            _colorBlue = 1f;  
        }

        private void Update()
        {
            float lifetimeProgress = (Time.time - _timeActivated) / activeTime; 

           
            _colorGreen = Mathf.Lerp(0.4f, 1, lifetimeProgress);
            _colorBlue = Mathf.Lerp(0.8f, 0, lifetimeProgress);

 
            _afterImageColor = new Color(0f, _colorGreen, _colorBlue, _alpha);
            _spriteRenderer.color = _afterImageColor;
            
            if (Time.time >= (_timeActivated + activeTime))
            {
                AfterImagePool.Instance.AddToPool(gameObject);
            }
        }
    }
}
