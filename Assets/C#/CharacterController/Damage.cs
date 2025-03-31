using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage);
}


namespace C_.CharacterController
{
    public class Damage : MonoBehaviour
    {
        [SerializeField] private float impulseForce;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var damageable = other.gameObject.GetComponent<IDamageable>();
    
            if (damageable == null )
            {
                return;
            }

            damageable.TakeDamage(TopDownMovement.Instance.damage);
            Debug.Log("sword damage");
            
            var rb = other.gameObject.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogWarning("Rigidbody2D not found on: " + other.gameObject.name);
                return;
            }
            
            var forceVector = other.transform.position - TopDownMovement.Instance.transform.position;
            rb.AddForce(forceVector.normalized * impulseForce, ForceMode2D.Impulse);
            
        }



#region Gizmoz

        private void OnDrawGizmos()
        {
            var boxCollider2D = GetComponent<BoxCollider2D>();
            if (boxCollider2D == null)
                return;

            Gizmos.color = Color.red; 


            var size = boxCollider2D.size;
            var offset = boxCollider2D.offset;
            var angle = transform.rotation.eulerAngles.z;


            var corners = new Vector2[4];
            corners[0] = new Vector2(-size.x / 2, -size.y / 2);
            corners[1] = new Vector2(size.x / 2, -size.y / 2);
            corners[2] = new Vector2(size.x / 2, size.y / 2);
            corners[3] = new Vector2(-size.x / 2, size.y / 2);


            for (var i = 0; i < corners.Length; i++)
            {
                var x = corners[i].x * Mathf.Cos(angle * Mathf.Deg2Rad) - corners[i].y * Mathf.Sin(angle * Mathf.Deg2Rad);
                var y = corners[i].x * Mathf.Sin(angle * Mathf.Deg2Rad) + corners[i].y * Mathf.Cos(angle * Mathf.Deg2Rad);
                corners[i] = new Vector2(x, y) + (Vector2)transform.position + offset;
            }


            Gizmos.DrawLine(corners[0], corners[1]);
            Gizmos.DrawLine(corners[1], corners[2]);
            Gizmos.DrawLine(corners[2], corners[3]);
            Gizmos.DrawLine(corners[3], corners[0]);
        }

#endregion
        
        
    }
}
