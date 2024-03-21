using UnityEngine;

public class HumanAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public GameObject target; // Reference to the slime GameObject
    public LayerMask groundLayer; // Layer mask for the ground
    public int expValue = 25; // Experience value granted to the slime when this human dies
    public int attackDamage = 5; // Damage dealt to the slime when attacking
    public float attackInterval = 1.5f; // Time interval between attacks
    public float attackRange = 1.5f; // Attack range within which the human can attack the slime
    public bool showAttackRange = true;
    public AudioClip deathSound; // Death sound clip
    [Range(0f, 1f)]
    public float volume = 1f; // Volume level of the death sound

    private float attackCooldown = 0f; // Cooldown timer for attacks
    private int maxHP = 50;
    private int currentHP;
    private Vector3 spawnPosition;
    private Animator animator; // Reference to the animator component
    private LineRenderer attackRangeRenderer; // LineRenderer for displaying attack range
    private AudioSource audioSource; // AudioSource component for playing sounds

    // Event to notify when this human dies
    public delegate void OnDeathEvent(int expValue);
    public event OnDeathEvent OnDeath;

    private void Start()
    {
        currentHP = maxHP;
        spawnPosition = transform.position;

        // Get reference to the Animator component
        animator = GetComponent<Animator>();

        if (target == null)
        {
            Debug.LogWarning("Target (slime) not assigned for HumanAI!");
        }

        // Initialize and set up the LineRenderer for attack range circle
        attackRangeRenderer = gameObject.AddComponent<LineRenderer>();
        attackRangeRenderer.positionCount = 51;
        attackRangeRenderer.useWorldSpace = false;
        attackRangeRenderer.startWidth = 0.1f;
        attackRangeRenderer.endWidth = 0.1f;
        attackRangeRenderer.material = new Material(Shader.Find("Sprites/Default"));
        attackRangeRenderer.startColor = Color.red;
        attackRangeRenderer.endColor = Color.red;
        attackRangeRenderer.enabled = showAttackRange;

        // Create the circle points for the LineRenderer
        UpdateAttackRange();

        // Add AudioSource component for playing sounds
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volume; // Set the initial volume
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;

        if (target != null)
        {
            Vector3 directionToTarget = target.transform.position - transform.position;

            // If the human is within attack range of the slime, attack it
            if (Vector3.Distance(transform.position, target.transform.position) <= attackRange)
            {
                Attack();
            }
            else
            {
                // Move towards the target
                transform.position += directionToTarget.normalized * moveSpeed * Time.deltaTime;

                // Check for collisions with the ground
                RaycastHit groundHit;
                if (Physics.Raycast(transform.position, Vector3.down, out groundHit, Mathf.Infinity, groundLayer))
                {
                    // Adjust position to prevent clipping with the ground
                    transform.position = groundHit.point + Vector3.up * 0.1f;
                }
            }
        }

        // Set animations based on movement
        animator.SetBool("Run", moveSpeed > 0);
    }

    private void Attack()
    {
        // If the attack cooldown has expired, attack the slime
        if (attackCooldown <= 0f)
        {
            SlimeAI slime = target.GetComponent<SlimeAI>();
            if (slime != null)
            {
                slime.TakeDamage(attackDamage);
                attackCooldown = attackInterval;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }

    private void Die()
    {
        transform.position = spawnPosition;
        currentHP = maxHP;

        if (OnDeath != null)
        {
            OnDeath(expValue);
        }

        // Play death sound if it's assigned
        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }

    private void UpdateAttackRange()
    {
        // Create the circle points for the LineRenderer
        Vector3[] circlePoints = new Vector3[51];
        float angle = 0f;
        float angleIncrement = (2f * Mathf.PI) / 50f;
        for (int i = 0; i < circlePoints.Length; i++)
        {
            float x = Mathf.Sin(angle) * attackRange;
            float z = Mathf.Cos(angle) * attackRange;
            circlePoints[i] = new Vector3(x, 0f, z);
            angle += angleIncrement;
        }

        // Set the positions of the LineRenderer
        attackRangeRenderer.SetPositions(circlePoints);
    }
}
