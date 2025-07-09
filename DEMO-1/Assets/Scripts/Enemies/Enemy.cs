using UnityEngine;
using System;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int health = 5;             // Lebenspunkte
    public int damage = 1;             // Schaden pro Angriff
    public float speed = 2f;           // Bewegungsgeschwindigkeit
    public float attackCooldown = 1f;  // Zeit zwischen Angriffen

    private Transform player;
    private NavMeshAgent agent;
    private float lastAttackTime;

    public event Action OnDeath;       // Event beim Tod

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
    }

    void Update()
    {
        if (player == null) return;
        agent.SetDestination(player.position);
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
