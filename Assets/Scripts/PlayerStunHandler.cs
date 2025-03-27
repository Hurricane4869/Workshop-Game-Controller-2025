using System.Collections;
using UnityEngine;

public class PlayerStunHandler : MonoBehaviour
{
    [Header("Stun Configuration")]
    [SerializeField] private float stunDuration = 0.5f; // Durasi stun dalam detik
    private bool isStunned = false;

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet.targetTag == "Player" && !isStunned)
            {
                float damage = bullet.GetDamage();
                playerController.DamagedBy(damage);
                Destroy(collision.gameObject);

                StartCoroutine(ApplyStun());
            }
        }
    }

    private IEnumerator ApplyStun()
    {
        isStunned = true;
        playerController.canBeMoved = false;

        // (Opsional) Animasi efek stun (misalnya mengganti warna pemain)
        SpriteRenderer spriteRenderer = playerController.GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.gray; // Menunjukkan efek stun

        yield return new WaitForSeconds(stunDuration);

        // Kembalikan kondisi normal
        isStunned = false;
        playerController.canBeMoved = true;
        spriteRenderer.color = originalColor;
    }
}
