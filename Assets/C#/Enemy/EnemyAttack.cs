using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour 
{
    private void Start()
    {
        Invoke(nameof(Destroy),3);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var damageable = other.gameObject.GetComponent<IDamageable>();
            
            damageable.TakeDamage(5);
            Destroy(gameObject);
        }
    }
}
