using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Status")]
    public float healthPoint = 20f;
    public float attackPoint = 5f;

    public Transform attackTarget;

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
        if(healthPoint <= 0)
        {
            healthPoint = 0;
            Die();
        }
    }
    void FallDie()
    {
        if(transform.position.y < -20)
            Die();
    }
    void Die()
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
