using System;
using System.Collections;
using C_.Manager;
using UnityEngine;
using UnityEngine.VFX;

namespace C_.CharacterController
{
    public class AttackController : MonoBehaviour
    {
        public static AttackController Instance{ get; private set; }
        
        public GameObject weapon; 
        public float attackCooldown = 0.5f;
        private float _lastAttackTime;
        
        public VisualEffect dashEffect;
        
        public Collider2D weaponCollider; 
        public float attackDuration = 0.2f;
        
        public float weaponDistanceFromPlayer = 1.5f;

        private bool _isAttacking; 


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;  
            }
            else
            {
                Destroy(gameObject);  
            }
        }

        private void Update()
        {
            if (PauseMenu.Instance.isPaused == true)
                return;
            if (Input.GetMouseButtonDown(0) && Time.time >= _lastAttackTime + attackCooldown)
            {
                Attack();
                AnimationController.Instance.AttackTrigger();
                Debug.Log("attacking");
                _lastAttackTime = Time.time; 
                dashEffect.Play();
            }
            
            if (!_isAttacking)
            {
                RotateWeaponAroundPlayer();
            }
        }

        private void Attack()
        {
            Debug.Log("Melee Attack Triggered");
            StartCoroutine(EnableColliderTemporarily());
        }

        private IEnumerator EnableColliderTemporarily()
        {
            _isAttacking = true;
            
            weaponCollider.enabled = true;
            
            yield return new WaitForSeconds(attackDuration);
            
            weaponCollider.enabled = false;
            
            _isAttacking = false;
        }

        private void RotateWeaponAroundPlayer()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; 
            
            var direction = mousePos - transform.position;
            direction.Normalize(); 
            
            weapon.transform.position = transform.position + direction * weaponDistanceFromPlayer;
            
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
