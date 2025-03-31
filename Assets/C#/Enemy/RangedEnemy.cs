using System.Collections;
using C_.CharacterController;
using UnityEngine;

namespace C_.Enemy
{
    public class RangedEnemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private int health = 15;
        private Transform _target;
        [SerializeField] private float attackRange = 8f;
        [SerializeField] private float escapeRange = 4f;
        [SerializeField] private float attackCooldown = 1.5f;
        private float _lastAttackTime;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float projectileSpeed = 5f;
        private SpriteRenderer _spriteRenderer;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;
        private float _distanceToPlayer;
        public GameObject xp;
        public bool isUpdateEnabled = true;
        private bool isdead;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _target = TopDownMovement.Instance.transform;
        }

        private void Update()
        {
            if (!isUpdateEnabled) return;
            // Distance to player
            _distanceToPlayer = Vector3.Distance(transform.position, _target.position);

            // Move away if the player is too close
            if (_distanceToPlayer < escapeRange)
            {
                MoveAwayFromTarget();
            }
            // Move closer if out of attack range
            else if (_distanceToPlayer > attackRange)
            {
                FallowTarget();
            }
            // Attack and move in circle if within attack range
            else
            {
                MoveInCircleAroundTarget();
                if (Time.time >= _lastAttackTime + attackCooldown)
                    AttackPlayer();
            }
            
            
        }

        private void FallowTarget()
        {
            if (_target == null)
                return;

            Vector2 direction = _target.position - transform.position;
            direction.Normalize();
            transform.position = Vector2.MoveTowards(transform.position, _target.position, moveSpeed * Time.deltaTime);
        }

        private void MoveInCircleAroundTarget()
        {
            if (_target == null)
                return;

            // Calculate the angle to rotate
            float angle = rotationSpeed * Time.deltaTime;
            // Move the enemy in a circular path around the player
            transform.RotateAround(_target.position, Vector3.forward, angle);

            // Maintain the orbit distance
            Vector3 direction = (transform.position - _target.position).normalized;
            transform.position = _target.position + direction * _distanceToPlayer; // Use fixed radius instead of distance to player
        }

        private void MoveAwayFromTarget()
        {
            if (_target == null)
                return;

            Vector2 direction = transform.position - _target.position; // Get direction away from the target
            direction.Normalize();
            transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)direction, moveSpeed * Time.deltaTime);
        }

        private void AttackPlayer()
        {
            _lastAttackTime = Time.time;

           
            var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            var rb = projectile.GetComponent<Rigidbody2D>();


            var playerVelocity = TopDownMovement.Instance.Movement * TopDownMovement.Instance.moveSpeed;
            
            var distanceToTarget = Vector2.Distance(_target.position + (Vector3)playerVelocity , firePoint.position);
            var travelTime = distanceToTarget / projectileSpeed;


           
            var predictedTargetPosition = (Vector2)_target.position + playerVelocity * travelTime;

           
            var direction = predictedTargetPosition - (Vector2)firePoint.position;
            direction.Normalize();

           
            rb.velocity = direction * projectileSpeed;
            
            // Set rotation to match the direction
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); 
        }



        public void TakeDamage(int damage)
        {
            health -= damage;
            Debug.Log("got damage");
            StartCoroutine(ChangeColor());
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
            Vector3 startScale = gameObject.transform.localScale;
            float time = 0f;

            while (time < 1)
            {
                gameObject.transform.localScale = Vector3.Lerp(startScale, new Vector3(3,3,3), time / 1);
                time += Time.deltaTime;
                yield return null; 
            }
            
            gameObject.transform.localScale = new Vector3(3,3,3);
            Instantiate(xp, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        IEnumerator ChangeColor()
        {
            _spriteRenderer.color = Color.red;
            
            yield return new WaitForSeconds(0.25f);
            
            _spriteRenderer.color = Color.white;
        }
    }
}
