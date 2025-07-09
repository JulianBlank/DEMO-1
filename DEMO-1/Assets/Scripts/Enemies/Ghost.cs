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

    [Header("Follow Settings")]
    public float stoppingDistance = 3f; // Mindestabstand zum Spieler

    private Transform player;
    private NavMeshAgent agent;
    private float lastAttackTime;

    public event Action OnDeath;

    private SpriteRuntimeEditor spriteChanger;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.speed = speed;
            agent.stoppingDistance = stoppingDistance;
            agent.isStopped = false;
        }
    }

    void Update()
    {
        if (player == null) return;

        // Ziel setzen – NavMeshAgent stoppt automatisch beim erreichen des stoppingDistance-Abstands
        agent.SetDestination(player.position);

        // Agent stoppen, wenn nah genug
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

        // Angriff
        if (Time.time > lastAttackTime + attackCooldown)
        {
            ShootAtPlayer();
            lastAttackTime = Time.time;
        }
    }

    void ShootAtPlayer()
    {
        if (projectilePrefab == null || player == null) return;

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

