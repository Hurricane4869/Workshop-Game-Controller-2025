using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float damagePoint = 2f;
    [SerializeField] private float dieTime = 5f;

    public string targetTag = "Enemy";
    private Vector2 direction;
    private Rigidbody2D rb2;
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        rb2.velocity = direction.normalized * speed;
        dieTime -= Time.deltaTime;
        if(dieTime < 0)
            Destroy(gameObject);
    }
    public void Launch(Vector2 direction, string targetTag, 
                        float speed, float damage)
    {
        this.direction = direction;
        this.speed = speed;
        this.damagePoint = damage;
        this.targetTag = targetTag;
    }
    public float GetDamage()
    {
        return damagePoint;
    }
}
