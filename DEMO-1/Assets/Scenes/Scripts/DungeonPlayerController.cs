using UnityEngine;
using System.Collections.Generic;

public class DungeonPlayerController : MonoBehaviour
{
    [Header("Bewegung")]
    [SerializeField] private float moveSpeed = 50f;
    private Vector3 target;
    private Vector3 lastMoveDirection = Vector3.right;

    [Header("Angriff")]
    public float attackRange = 1f;
    public Vector2 attackBoxSize = new Vector2(1f, 1f);
    public LayerMask enemyLayer;
    public int damage = 1;
    [SerializeField] private Sprite north;
    [SerializeField] private Sprite south;
    [SerializeField] private Sprite west;
    [SerializeField] private Sprite east;

    private SpriteRuntimeEditor spriteChanger;

    private void Start()
    {
        target = transform.position;
        // SpriteChanger auf dem gleichen GameObject
        spriteChanger = GetComponent<SpriteRuntimeEditor>();
    }

    private void Update()
    {
        // Rechte Maustaste: Bewegung
        if (Input.GetMouseButtonDown(1))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
        }

        // Bewegung
        Vector3 moveDirection = (target - transform.position).normalized;

        if (moveDirection != Vector3.zero)
        {
            lastMoveDirection = moveDirection;

            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
            {
                if (moveDirection.x > 0)
                    spriteChanger.ChangeSprite(west);
                else
                    spriteChanger.ChangeSprite(east);
            }
            else
            {
                if (moveDirection.y > 0)
                    spriteChanger.ChangeSprite(north);
                else
                    spriteChanger.ChangeSprite(south);
            }
        }


        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        // Linke Maustaste: Angriff
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        Vector3 attackCenter = transform.position + lastMoveDirection.normalized * attackRange;

        // Alle Gegner im Angriffsbereich finden
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackCenter, attackBoxSize, 0f, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                //Debug.Log("Gegner getroffen!");
            }
        }

        // Debug-Anzeige
        Debug.DrawLine(transform.position, attackCenter, Color.red, 0.2f);
    }

    private void OnDrawGizmosSelected()
    {
        // Nur im Editor sichtbar – zeigt das Angriffsrechteck
        Vector3 attackCenter = transform.position + lastMoveDirection.normalized * attackRange;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackCenter, attackBoxSize);
    }
}