using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Status")]
    public float healthPoint = 20f;
    public float enemyHealthBarFullX = 14.1f;
    public float attackPoint = 5f;
    public Transform attackTarget;
    public SpriteRenderer enemyHealthBar;
    public GameObject enemy;
    private float initialHealth = 20f;


    [Header("Configuration")]
    [SerializeField] protected float moveSpeed = 2.5f;

    void Start()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        attackTarget = target.transform;
    }
    void Update()
    {
        Movement();
        FallDie();
    }

    protected virtual void Movement()
    {}
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
    protected virtual void FallDie()
    {
        if(transform.position.y < -20)
            Die();
    }
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
    public float GetAttackDamage()
    {
        return attackPoint;
    }
    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Bullet")) // Cek apakah yang terkena adalah peluru
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null && bullet.targetTag == "Enemy")
            {
                float damage = bullet.GetDamage();
                DamagedBy(damage);
                Destroy(collision.gameObject); // Hancurkan peluru setelah terkena
            }
        }
    }

}
