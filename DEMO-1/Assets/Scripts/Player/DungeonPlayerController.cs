using UnityEngine;
using System.Collections;

/// <summary>
/// Steuert den Spieler im Dungeon-Spiel, inklusive Bewegung, Angriff und Sprite-Wechsel basierend auf Richtung und Waffenstatus.
/// Unterstützt mehrere Waffen.
/// </summary>
public class DungeonPlayerController : MonoBehaviour
{
    [Header("Bewegung")]
    [Tooltip("Bewegungsgeschwindigkeit des Spielers.")]
    [SerializeField] private float moveSpeed = 50f;

    private Vector3 target;
    private Vector3 lastMoveDirection = Vector3.right;

    [Header("Angriff")]
    [Tooltip("Reichweite der Angriffsbox vom Spieler aus.")]
    public float attackRange = 1f;

    [Tooltip("Größe der Angriffsbox.")]
    public Vector2 attackBoxSize = new Vector2(1f, 1f);

    [Tooltip("Layer-Maske, um Gegner zu identifizieren.")]
    public LayerMask enemyLayer;

    [Tooltip("Schaden, der Gegnern zugefügt wird.")]
    public int damage = 5;

    [Tooltip("Cooldown-Zeit zwischen Angriffen.")]
    public float attackCooldown = 1f;

    public bool canAttack = true;

    [Header("Waffen")]
    [Tooltip("Primäre Waffe, z.B. Schwert.")]
    [SerializeField] private AttachOnProximity primaryWeapon;

    [Tooltip("Sekundäre Waffe, z.B. Schild, Bogen, etc.")]
    [SerializeField] private AttachOnProximity secondaryWeapon;

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

    // Weitere Sprites für zweite Waffe könnten hier ergänzt werden...

    private void Start()
    {
        target = transform.position;
        spriteChanger = GetComponent<SpriteRuntimeEditor>();

        TryFindWeapon(ref primaryWeapon, "Weapon");
        TryFindWeapon(ref secondaryWeapon, "SecondaryWeapon");
    }

    private void TryFindWeapon(ref AttachOnProximity weapon, string tag)
    {
        if (weapon == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag(tag);
            if (go != null)
                weapon = go.GetComponent<AttachOnProximity>();
        }

        if (weapon == null)
            Debug.LogWarning($"Waffe mit Tag '{tag}' konnte nicht gefunden werden.");
    }

    private void Update()
    {
        HandleMovementInput();
        MovePlayer();
        HandleAttackInput();
    }

    private void HandleMovementInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
        }

        Vector3 moveDirection = (target - transform.position).normalized;

        if (moveDirection != Vector3.zero)
        {
            lastMoveDirection = moveDirection;
            UpdateSprite(moveDirection);
        }
    }

    private void MovePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    private void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            Attack();
        }
    }

    /// <summary>
    /// Ermittelt, ob irgendeine Waffe aktiv ist.
    /// </summary>
    public bool HasAnyWeapon()
    {
        return (primaryWeapon != null && primaryWeapon.GetStatus()) ||
               (secondaryWeapon != null && secondaryWeapon.GetStatus());
    }

    /// <summary>
    /// Aktualisiert das aktuelle Sprite basierend auf Richtung und Waffenstatus.
    /// </summary>
    private void UpdateSprite(Vector3 moveDirection)
    {
        bool hasWeapon = HasAnyWeapon();
        bool isMovingHorizontally = Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y);

        if (hasWeapon)
        {
            if (isMovingHorizontally)
                spriteChanger.ChangeSprite(moveDirection.x > 0 ? holdingSword_west : holdingSword_east);
            else
                spriteChanger.ChangeSprite(moveDirection.y > 0 ? holdingSword_north : holdingSword_south);
        }
        else
        {
            if (isMovingHorizontally)
                spriteChanger.ChangeSprite(moveDirection.x > 0 ? west : east);
            else
                spriteChanger.ChangeSprite(moveDirection.y > 0 ? north : south);
        }
    }

    private void Attack()
    {
        canAttack = false;
        StartCoroutine(ResetAttackCooldown());

        Vector3 attackCenter = transform.position + lastMoveDirection.normalized * attackRange;
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackCenter, attackBoxSize, 0f, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damage);
        }

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

    public void SetAttackRange(float range) => attackRange = range;

    public void SetAttackBoxSize(float width, float height) => attackBoxSize = new Vector2(width, height);
}
