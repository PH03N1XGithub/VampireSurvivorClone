using System.Collections;
using C_.Manager;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace C_.CharacterController
{
    public class TopDownMovement : MonoBehaviour , IDamageable
    {
        public static TopDownMovement Instance{ get; private set; }

        private float _health = 100f;
        private float _xp = 0f;
        public Slider healthBar;
        public Slider xpBar;
        public TMP_Text levelText;
        internal int _level = 1;

        public float moveSpeed = 5f;
        public float dashSpeed;
        public float dashDuration = 0.2f;
        public float dashCooldown = 1f;
        public float distanceBetweenImages;
        
        public int damage = 10;
        public int dashDamage = 10;
        public int dashStartValue = 50;

        
        
        
        private float _lastImageXPosition;
        private float _lastImageYPosition;

        private Rigidbody2D _rb;
        internal Vector2 Movement;
        [SerializeField]private bool bisDashing;    
        private bool _bCanDash = true;      
        private float _dashEndTime;
        private SpriteRenderer _spriteRenderer;
        

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
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            {
                healthBar.maxValue = _health;
                healthBar.value = _health;
                xpBar.maxValue = 100;
                xpBar.value = 0;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                TakeDamage(10);
            }
            
            
            // Input
            Movement.x = Input.GetAxisRaw("Horizontal");
            Movement.y = Input.GetAxisRaw("Vertical");

            // Trigger dash spacebar 
            if (Input.GetKeyDown(KeyCode.Space) && _bCanDash)
            {
                StartDash();
            }
            
            if (bisDashing && Time.time >= _dashEndTime)
            {
                StopDash();
            }
        }

        private void FixedUpdate()
        {
            // Movement
            if (bisDashing)
            {
                // Dash
                _rb.MovePosition(_rb.position + Movement * (dashSpeed * Time.fixedDeltaTime));
            }
            else
            {
                // Normal
                _rb.MovePosition(_rb.position + Movement * (moveSpeed * Time.fixedDeltaTime));
            }
        }


        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (bisDashing)
            {
             
                var enemy = other.gameObject.GetComponent<Enemy.Enemy>();
                if (enemy)
                {
                    enemy.TakeDamage(dashDamage);
                    Debug.Log("dash damage");
                }   
            }


            if (other.gameObject.CompareTag("XP"))
            {
                Destroy(other.gameObject);
                UpdateXp(45);
            }
            
        }

       

        private void UpdateXp(int add)
        {
            xpBar.value += add;
            _xp += add;
            if (xpBar.value >= 100)
            {
                _xp -= 100;
                _level++;
                xpBar.value = _xp;
                levelText.text = "Level: " + _level; 
                PauseMenu.Instance.ShowPowerUpMenu();
                
            }
        }


        #region Dash
        private void StartDash()
        {
            bisDashing = true;
            _bCanDash = false;               
            _dashEndTime = Time.time + dashDuration; 

            // Initialize afterimage position
            _lastImageXPosition = transform.position.x;
            _lastImageYPosition = transform.position.y;
            
            DOTween.To(x => dashSpeed = x, dashStartValue, 12, 0.1f).SetEase(Ease.InExpo);

            StartCoroutine(PlaySequence());
            
            StartCoroutine(DashCooldownCoroutine());
        }

        private void StopDash()
        {
            bisDashing = false;
        }

        private IEnumerator DashCooldownCoroutine()
        {

            yield return new WaitForSeconds(dashCooldown);
            _bCanDash = true;  
        }
        
        private IEnumerator PlaySequence()
        {
       
            AfterImagePool.Instance.GetFromPool();

           
            _lastImageXPosition = transform.position.x;
            _lastImageYPosition = transform.position.y;

         
            var endTime = Time.time + dashDuration;
            while (Time.time < endTime)
            {
                
                if (Mathf.Sqrt( Mathf.Pow(transform.position.x - _lastImageXPosition, 2) + Mathf.Pow(transform.position.y - _lastImageYPosition,2)) > distanceBetweenImages)
                {
                    AfterImagePool.Instance.GetFromPool();
                    _lastImageXPosition = transform.position.x;
                    _lastImageYPosition = transform.position.y;
                }
                yield return null;
            }

            
            StopDash();
        }
        #endregion


        public void TakeDamage(int damage)
        {
            if (bisDashing)
                return;
            
            _health -= damage;
            Debug.Log("got damage");
            StartCoroutine(ChangeColor());
            UpdateHealthBar();
            if (_health <= 0)
            {
                Debug.Log("Dead");
                PauseMenu.Instance.DeadMenu();
            }
        }

        private void UpdateHealthBar()
        {
            healthBar.value = _health;
        }


        private IEnumerator ChangeColor()
        {
            
            _spriteRenderer.color = Color.red;
            
            yield return new WaitForSeconds(0.25f);
            
            _spriteRenderer.color = Color.white;
        }
    }
}
