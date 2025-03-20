using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("Properties")]
    [SerializeField] private SpriteRenderer graphic;

    [Header("Status")]
    public float health = 100;
    public float attack = 5;

    public bool canBeMoved = true;

    public float healthMax { private set; get; }

    private bool isGrounded;
    private bool isShooting = true;

    [Header("Configuration")]
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float shootingTimeMax = 0.1f;
    [SerializeField] private int jumpCount = 2;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10;


    [SerializeField] private Vector2 gunOffset;

    private float shootingTime = 0;


    private Rigidbody2D rb2;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();

        shootingTime = shootingTimeMax;
        healthMax = health;
    }

    void Update()
    {
        if (canBeMoved)
        {
            MovementController();
            Jump();
            ShootController();
        }
    }

    void MovementController()
    {
        float x = Input.GetAxisRaw("Horizontal");

        Vector2 direction = rb2.velocity;
        direction.x = x * moveSpeed;

        rb2.velocity = direction;

        //sprite dibalik ketika arahnya ke kiri
        if (direction.x < 0)
        {
            graphic.flipX = true;
        }
        else if (direction.x > 0)
        {
            graphic.flipX = false;
        }
    }

private void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            rb2.velocity = new Vector2(rb2.velocity.x, jumpForce);
            jumpCount--; // Mengurangi jumlah kesempatan lompat
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy")) // Pastikan objek bertagar "Ground"
        {
            isGrounded = true;
            jumpCount = 2; // Reset kesempatan lompat saat menyentuh tanah
        }
        if (collision.collider.tag == "Enemy")
        {
            float damage = collision.collider.GetComponent<EnemyController>().GetAttackDamage();
            DamagedBy(damage);
        }
    }

    private void OnCollisionExit2D(Collision2D collision )
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            isGrounded = false;
        }
    }

    void ShootController()
    {
        isShooting = Input.GetButton("Fire1") || Input.GetAxis("Fire1") > 0.5f;

        if (isShooting)
        {
            shootingTime -= Time.deltaTime;
        }
        else
        {
            shootingTime = shootingTimeMax;
        }

        if (isShooting && shootingTime < 0)
        {
            shootingTime = shootingTimeMax;
            Shoot();
        }
    }

    void Shoot()
    {
        int direction = (graphic.flipX == false ? 1 : -1);

        Vector2 gunPos = new Vector2(gunOffset.x * direction + transform.position.x, gunOffset.y + transform.position.y);

        GameObject bulletObj = Instantiate(bulletPrefab, gunPos, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet)
        {
            bullet.Launch(new Vector2(direction, 0), "Enemy", bulletSpeed, attack);
        }
    }

        public void DamagedBy(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    void FallDie()
    {
        if (transform.position.y < -20)
        {
            Die();
        }
    }

    void Die()
    {
        graphic.enabled = false;
        canBeMoved = false;
        GameManager.GameOver();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet.targetTag == "Player")
            {
                float damage = bullet.GetDamage();
                DamagedBy(damage);
                Destroy(collision.gameObject);
            }
        }
    }
}