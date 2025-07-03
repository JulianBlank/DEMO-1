using UnityEngine;
using System.Collections;

/// <summary>
/// Steuert den Spieler im Dungeon-Spiel, inklusive Bewegung, Angriff und Sprite-Wechsel basierend auf Richtung und Waffenstatus.
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

    private bool canAttack = true;

    [Tooltip("Referenz auf das Waffenobjekt.")]
    [SerializeField] private AttachOnProximity wpn;

    private SpriteRuntimeEditor spriteChanger;

    [Header("Sprites")]
    [Tooltip("Sprite für Bewegung nach Norden.")]
    [SerializeField] private Sprite north;
    [Tooltip("Sprite für Bewegung nach Süden.")]
    [SerializeField] private Sprite south;
    [Tooltip("Sprite für Bewegung nach Westen.")]
    [SerializeField] private Sprite west;
    [Tooltip("Sprite für Bewegung nach Osten.")]
    [SerializeField] private Sprite east;

    [Tooltip("Sprite mit Schwert nach Norden.")]
    [SerializeField] private Sprite holdingSword_north;
    [Tooltip("Sprite mit Schwert nach Süden.")]
    [SerializeField] private Sprite holdingSword_south;
    [Tooltip("Sprite mit Schwert nach Westen.")]
    [SerializeField] private Sprite holdingSword_west;
    [Tooltip("Sprite mit Schwert nach Osten.")]
    [SerializeField] private Sprite holdingSword_east;

    /// <summary>
    /// Initialisiert Komponenten und sucht ggf. das Waffenobjekt anhand des Tags.
    /// </summary>
    private void Start()
    {
        target = transform.position;
        spriteChanger = GetComponent<SpriteRuntimeEditor>();

        if (wpn == null)
        {
            GameObject weaponGO = GameObject.FindGameObjectWithTag("Weapon");
            if (weaponGO != null)
                wpn = weaponGO.GetComponent<AttachOnProximity>();
        }

        if (wpn == null)
            Debug.LogError("Waffe konnte nicht gefunden werden!");
    }

    /// <summary>
    /// Wird einmal pro Frame aufgerufen. Behandelt Eingaben, Bewegung und Angriff.
    /// </summary>
    private void Update()
    {
        HandleMovementInput();
        MovePlayer();
        HandleAttackInput();
    }

    /// <summary>
    /// Behandelt rechte Maustaste und berechnet Bewegungsrichtung.
    /// </summary>
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

    /// <summary>
    /// Bewegt den Spieler zum Ziel.
    /// </summary>
    private void MovePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Behandelt linken Mausklick für Angriffe.
    /// </summary>
    private void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            Attack();
        }
    }

    /// <summary>
    /// Aktualisiert das aktuelle Sprite basierend auf Richtung und Waffenstatus.
    /// </summary>
    /// <param name="moveDirection">Richtung der Bewegung.</param>
    private void UpdateSprite(Vector3 moveDirection)
    {
        bool hasWeapon = wpn != null && wpn.GetStatus();
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

    /// <summary>
    /// Führt einen Angriff in die zuletzt gewählte Richtung durch.
    /// </summary>
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

    /// <summary>
    /// Setzt den Angriff nach Ablauf des Cooldowns zurück.
    /// </summary>
    private IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        Debug.Log("Attack ready again");
    }

    /// <summary>
    /// Zeichnet die Angriffsbox im Editor zur besseren Sichtbarkeit.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Vector3 attackCenter = transform.position + lastMoveDirection.normalized * attackRange;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackCenter, attackBoxSize);
    }

    /// <summary>
    /// Setzt die Reichweite des Angriffs.
    /// </summary>
    /// <param name="range">Neue Angriffsreichweite.</param>
    public void SetAttackRange(float range) => attackRange = range;

    /// <summary>
    /// Setzt die Größe der Angriffsbox.
    /// </summary>
    /// <param name="width">Breite der Box.</param>
    /// <param name="height">Höhe der Box.</param>
    public void SetAttackBoxSize(float width, float height) => attackBoxSize = new Vector2(width, height);
}
