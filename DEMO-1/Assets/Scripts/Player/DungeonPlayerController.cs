using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DungeonPlayerController : MonoBehaviour
{
    [Header("Bewegung")]
    [SerializeField] private float moveSpeed = 50f;     //Bewegungsgeschwindigkeit
    private Vector3 target;     //Zu schädigender Gegner
    private Vector3 lastMoveDirection = Vector3.right;  

    [Header("Angriff")]
    public float attackRange = 1f;  //Entfernung des Spielers vom Mittelpunkt der AttackBox
    public Vector2 attackBoxSize = new Vector2(1f, 1f);     //Grösse der Attackbox
    public LayerMask enemyLayer;    // Maske, um Enemies zu kennzeichnen / Legt das Schadenslayer fest
    public int damage = 5;      //Schaden
    public float attackCooldown = 1f;   //Cooldown bis zur nächsten Attacke
    private bool canAttack = true;  

    [SerializeField] private AttachOnProximity wpn;
    private SpriteRuntimeEditor spriteChanger;

    [Header("Sprites")]
    [SerializeField] private Sprite north;
    [SerializeField] private Sprite south;
    [SerializeField] private Sprite west;
    [SerializeField] private Sprite east;
    [SerializeField] private Sprite holdingSword_north;
    [SerializeField] private Sprite holdingSword_south;
    [SerializeField] private Sprite holdingSword_west;
    [SerializeField] private Sprite holdingSword_east;

    private void Start()
    {
        target = transform.position;
        spriteChanger = GetComponent<SpriteRuntimeEditor>();
        if (wpn == null)
        {
            GameObject weaponGO = GameObject.FindGameObjectWithTag("Weapon");
            if (weaponGO != null)
            {
                wpn = weaponGO.GetComponent<AttachOnProximity>();
            }
        }

        if (wpn == null)
            Debug.LogError("Waffe konnte nicht gefunden werden!");
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

            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y) && wpn.GetStatus() == false && wpn != null) // Controls player sprite without a weapon
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

            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y) && wpn.GetStatus() && wpn != null) // Controls player sprite with a weapon
            {
                if (moveDirection.x > 0)
                    spriteChanger.ChangeSprite(holdingSword_west);
                else
                    spriteChanger.ChangeSprite(holdingSword_east);
            }
            else
            {
                if (moveDirection.y > 0)
                    spriteChanger.ChangeSprite(holdingSword_north);
                else
                    spriteChanger.ChangeSprite(holdingSword_south);
            }

        }

        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        // Linke Maustaste: Angriff (mit Cooldown)
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            Attack();
        }
    }

    private void Attack()
    {
        canAttack = false;
        StartCoroutine(ResetAttackCooldown());

        Vector3 attackCenter = transform.position + lastMoveDirection.normalized * attackRange;

        // Alle Gegner im Angriffsbereich finden
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackCenter, attackBoxSize, 0f, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                // Debug.Log("Gegner getroffen!");
            }
        }

        // Debug-Anzeige
        Debug.DrawLine(transform.position, attackCenter, Color.red, 0.2f);
    }

    private IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        Debug.Log("Attack ready again");
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 attackCenter = transform.position + lastMoveDirection.normalized * attackRange;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackCenter, attackBoxSize);
    }

    public void setattackrange(float atkrange)
    {
        attackRange = atkrange;
    }

    public void setattackbox(float x, float y)
    {
        attackBoxSize = new Vector2(x, y);
    }
}
