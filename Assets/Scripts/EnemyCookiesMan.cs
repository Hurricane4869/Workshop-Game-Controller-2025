using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCookiesMan : MonoBehaviour
{
    [Header("Status")]
    public float healthPoint = 20f;
    public float enemyHealthBarFullX = 14.1f;
    public float attackPoint = 5f;
    public Transform attackTarget;
    public SpriteRenderer enemyHealthBar;
    public GameObject enemy;

    [Header("Configuration")]
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float attackMinDistance;
    [SerializeField] private float attackMaxDistance;
    [SerializeField] private float attackTimeMax;
    [SerializeField] private Vector3 shootOffset;
    private float attackTime = 0;

    [Header("Graphics and Shooting")]
    [SerializeField] private SpriteRenderer graphic;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;

    private Rigidbody2D rb;
    private float initialHealth;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        initialHealth = healthPoint; // Simpan nilai awal
    }

    void Start()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        if (target)
            attackTarget = target.transform;

        // Health bar penuh di awal
        UpdateHealthBar();
    }

    void Update()
    {
        Movement();
        FallDie();
    }

    private void Movement()
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
            rb.velocity = Vector2.zero;
        }
        else
        {
            Vector2 direction = (attackTarget.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
    }

    public void DamagedBy(float damagePoint)
    {
        healthPoint -= damagePoint;
        healthPoint = Mathf.Clamp(healthPoint, 0, initialHealth);

        UpdateHealthBar();

        if (healthPoint <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (enemyHealthBar != null)
        {
            float healthPercentage = healthPoint / initialHealth;
            Vector3 currentScale = enemyHealthBar.transform.localScale;
            currentScale.x = healthPercentage * enemyHealthBarFullX;
            enemyHealthBar.transform.localScale = currentScale;
        }
    }

    private void FallDie()
    {
        if (transform.position.y < -20)
            Die();
    }

    void Die()
    {
        ScoreManager.AddScore(10);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
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
