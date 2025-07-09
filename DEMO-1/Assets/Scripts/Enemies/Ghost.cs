using UnityEngine;
using System;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    public int health = 5;
    public int damage = 1;
    public float speed = 2f;

    [Header("Attack Settings")]
    public float attackCooldown = 1f;
    public GameObject projectilePrefab;

    private Transform player;
    private NavMeshAgent agent;
    private float lastAttackTime;

    public event Action OnDeath;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
    }

    void Update()
    {
        if (player == null) return;

        // Move towards the player
        agent.SetDestination(player.position);

        // Attack the player with a projectile
        if (Time.time > lastAttackTime + attackCooldown)
        {
            ShootAtPlayer();
            lastAttackTime = Time.time;
        }
    }

    void ShootAtPlayer()
    {
        if (projectilePrefab == null || player == null) return;

        // Spawn the projectile at the enemy's position, and send it to the player's position
        Projectile.CreateProjectile(projectilePrefab, transform.position, player.position);
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

