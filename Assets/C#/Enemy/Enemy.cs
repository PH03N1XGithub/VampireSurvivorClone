using System;
using System.Collections;
using C_.CharacterController;
using UnityEngine;

namespace C_.Enemy
{
    internal enum EnemyState { Idle = 0, Follow, Attack }
    public class Enemy : MonoBehaviour , IDamageable
    {
        [SerializeField] private int health = 10;
        private Transform _target;
        [SerializeField]private float distance;
        private SpriteRenderer _spriteRenderer;
        [SerializeField]private float speed;
        public GameObject xp;
        private bool isUpdateEnabled = true;
        private bool isdead;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _target = TopDownMovement.Instance.transform;
        }

        private void Update()
        {
            if (!isUpdateEnabled) return;
            //Enemy fallows player only if player is in range(distance) 
            var distanceToPlayer = Vector3.Distance(transform.position, _target.position);
            if (distanceToPlayer < distance)
            {
                FallowTarget();
            }
        }

        private void FallowTarget()
        {
            if (_target == null)
                return;
            Vector2 direction = _target.position - transform.position;
            direction.Normalize(); 
            transform.position = Vector2.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            var damageable = other.GetComponent<IDamageable>();
            damageable?.TakeDamage(5);
        }


        public void TakeDamage(int damage)
        {
            health -= damage;
            Debug.Log("got damage");
            StartCoroutine(changeColor());
            if (health <= 0 && !isdead)
            {
                isdead = true; 
                HighScore_Level.Instance.playerScore += 1;
                isUpdateEnabled = false;
                StartCoroutine(ScaleObject());
            }
        }
        
        private IEnumerator ScaleObject()
        {
            Vector3 startScale = gameObject.transform.localScale; // Starting scale of the object
            float time = 0f;

            while (time < 1)
            {
                gameObject.transform.localScale = Vector3.Lerp(startScale, new Vector3(3,3,3), time / 1);
                time += Time.deltaTime;
                yield return null; 
            }

            // Ensure the object reaches the exact target scale at the end
            gameObject.transform.localScale = new Vector3(3,3,3);
            Instantiate(xp, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        IEnumerator changeColor()
        {
            
            _spriteRenderer.color = Color.red;
            
            yield return new WaitForSeconds(0.25f);
            
            _spriteRenderer.color = Color.white;
        }
    }
}
    
    
    
