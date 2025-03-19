using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyCookiesMan : EnemyController
{
    [Header("Cookies Man")]
    [SerializeField] SpriteRenderer graphic;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;

    [SerializeField] private float attackMinDistance;
    [SerializeField] private float attackMaxDistance;
    [SerializeField] private float attackTimeMax;

    [SerializeField] private Vector3 shootOffset;
    private float attackTime = 0;
    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    protected override void Movement()
    {
        if(attackTarget)
        {
            float distance = Vector2.Distance(transform.position, attackTarget.position);
            if(distance < attackMinDistance)
            {
                if(attackTarget.position.x < transform.position.x) // Flip true arah sprite jika di sebelah kiri
                {
                    graphic.flipX = true;
                } 
                else
                {
                    graphic.flipX = false;    
                }

                FollowTarget(distance);

                if(attackTime < 0)
                {
                    Shoot();
                    attackTime = attackTimeMax;
                }

                attackTime -= Time.deltaTime;
            }
        }
    }
    void Shoot()
    {
        int direction = (graphic.flipX == false ? 1 : -1);
        Vector3 shootOffsetDirection = shootOffset;

        if(direction < 0)
        {
            shootOffsetDirection *= -1;
        }

        Vector3 shootPos = transform.position + shootOffsetDirection;

        GameObject bulletObj = Instantiate(bulletPrefab, shootPos, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Launch(new Vector2(direction, 0), "Player", bulletSpeed, attackPoint);
    }

    void FollowTarget(float distance)
    {
        if (attackTarget != null)
        {
            if (distance <= attackMaxDistance)
            {
                rb.velocity = Vector2.zero;
            }
            else
            {
                Vector2 direction = (attackTarget.position - transform.position).normalized;
                rb.velocity = direction * moveSpeed;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
