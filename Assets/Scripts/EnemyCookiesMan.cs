using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCookiesMan : EnemyController
{
    [Header("Configuration")]
    [SerializeField] private float attackMinDistance;
    [SerializeField] private float attackMaxDistance;
    [SerializeField] private float attackTimeMax;
    [SerializeField] private Vector3 shootOffset;
    // [SerializeField] private float jumpForce = 6.0f;
    // [SerializeField] private float verticalJumpThreshold = 1.5f;
    private bool isGrounded = false;

    private float attackTime = 0;
    
    [Header("Graphics and Shooting")]
    [SerializeField] private SpriteRenderer graphic;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    
    private Rigidbody2D rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Start()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        if (target)
            attackTarget = target.transform;
    }

    void Update()
    {
        Movement();
        FallDie();
    }

    protected override void Movement()
    {
        if (attackTarget)
        {
            float distance = Vector2.Distance(transform.position, attackTarget.position);
            if (distance < attackMinDistance)
            {
                graphic.flipX = attackTarget.position.x < transform.position.x;
                FollowTarget(distance);

                if (attackTime < 0)
                {
                    Shoot();
                    attackTime = attackTimeMax;
                }
                attackTime -= Time.deltaTime;
            }
        }
    }

    private void Shoot()
    {
        int direction = graphic.flipX ? -1 : 1;
        Vector3 shootPos = transform.position + shootOffset * direction;

        GameObject bulletObj = Instantiate(bulletPrefab, shootPos, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Launch(new Vector2(direction, 0), "Player", bulletSpeed, attackPoint);
    }

    private void FollowTarget(float distance)
    {
        if (distance <= attackMaxDistance)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            Vector2 direction = (attackTarget.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

            //float verticalDifference = attackTarget.position.y - transform.position.y;

            // if(isGrounded && verticalDifference > verticalJumpThreshold)
            // {
            //     rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //     isGrounded = false;
            // }
        }
    }

    protected override void FallDie()
    {
        if (transform.position.y < -20)
            Die();
    }

    protected override void Die()
    {
        Debug.Log("Enemy dies");
        ScoreManager.AddScore(10); 
        Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //isGrounded = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //isGrounded = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null && bullet.targetTag == "Enemy")
            {
                float damage = bullet.GetDamage();
                DamagedBy(damage);
                Destroy(collision.gameObject);
            }
        }
    }
}