using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public int health = 5;
    public int damage = 1;
    public float speed = 2f;
    public float attackCooldown = 1f;

    private Transform player;
    private float lastAttackTime;

    public event Action OnDeath;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        // Bewege dich in Richtung Spieler
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += speed * Time.deltaTime * direction;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time > lastAttackTime + attackCooldown)
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                lastAttackTime = Time.time;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}