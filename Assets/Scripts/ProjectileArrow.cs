using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileArrow : MonoBehaviour
{
    public int damage = 10;
    public Vector2 moveSpeed = new Vector2(3f, 0);
    public Vector2 knockback = new Vector2(0, 0);

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    // Start is called before the first frame update
    void Start()
    {
        // If you want the projectile to be effected by gravity by default, make it dynamic mode rigidbody
        rb.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);

        Invoke("DestroyArrow", 1.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if(damageable != null)
        {
            // If parent is facing the left by localscale, our knockback x flips its value to face the left as well
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            // Hit the target
            bool gotHit = damageable.Hit(damage, deliveredKnockback,collision);

            if (gotHit)
                Debug.Log(collision.name + " hit for " + damage);
                Destroy(gameObject);
        }
    }
    private void DestroyArrow()
    {
        Destroy(gameObject);
    }
}
