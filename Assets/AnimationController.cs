using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    // Singleton instance
    public static AnimationController Instance { get; private set; }
    
    public Animator animator;
    private Vector2 _movement;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Optional: Keep this object across scenes if needed
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    private void Update()
    {
        // Get movement input
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        // Set walking animation based on movement
        animator.SetBool("Walk", _movement != Vector2.zero);

        // Update animator parameters
        animator.SetFloat("Horizontal", _movement.x);
        animator.SetFloat("Vertical", _movement.y);
    }

    // Public method to trigger attack animation
    public void AttackTrigger()
    {
        animator.SetTrigger("Attack");
    }
}