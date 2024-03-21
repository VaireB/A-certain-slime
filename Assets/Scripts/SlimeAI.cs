using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public string targetTag = "Human"; // Tag of the prefabs to attack
    public int baseAttackDamage = 10;
    public float attackAngle = 180f; // Angle within which the slime can attack
    public float attackInterval = 1.0f; // Time interval between attacks
    private float attackCooldown = 0f; // Cooldown timer for attacks
    public float attackRange = 1.5f; // Attack range within which the slime can attack the humans
    public bool showAttackRange = true; // Toggle to show/hide attack range in the inspector
    public LayerMask groundLayer; // Layer mask for the ground
    public LayerMask obstacleLayer;
    public AudioClip levelUpClip; // Sound clip for level up
    public float levelUpVolume = 0.5f; // Volume level for level up sound
    public AudioClip deathClip; // Sound clip for death
    [Range(0f, 1f)]
    public float deathVolume = 0.5f; // Volume level for death sound

    public Animator animator; // Reference to the animator component

    private AudioSource levelUpSound; // Reference to the AudioSource component for level up sound
    private AudioSource deathSound; // Reference to the AudioSource component for death sound

    public string slimeName = "Nameless Slime";

    // Slime attributes
    public int maxHP = 100;
    public int currentHP;
    public int expValue = 25;
    public int level = 1;
    public int experience = 0;
    public int experienceToNextLevel = 100;
    public int attackDamage; // Updated attack damage based on level
    private Vector3 spawnPoint; // Spawn point of the slime
    private LineRenderer attackRangeRenderer; // LineRenderer for displaying attack range
    public int rebirthCount = 0; // Rebirth count

    private void Start()
    {
        currentHP = maxHP;
        spawnPoint = transform.position;

        // Find all GameObjects with the "Human" tag
        GameObject[] humanObjects = GameObject.FindGameObjectsWithTag("Human");

        // Loop through each GameObject to find HumanAI components
        foreach (GameObject humanObject in humanObjects)
        {
            // Get the HumanAI component from the GameObject
            HumanAI human = humanObject.GetComponent<HumanAI>();

            // Check if the HumanAI component exists
            if (human != null)
            {
                // Subscribe to the OnDeath event of HumanAI
                human.OnDeath += GainExperienceFromHumanDeath;
            }
        }

        // Initialize attack damage based on base attack damage
        attackDamage = baseAttackDamage;

        // Initialize and set up the LineRenderer for attack range circle
        attackRangeRenderer = gameObject.AddComponent<LineRenderer>();
        attackRangeRenderer.positionCount = 51;
        attackRangeRenderer.useWorldSpace = false;
        attackRangeRenderer.startWidth = 0.1f;
        attackRangeRenderer.endWidth = 0.1f;
        attackRangeRenderer.material = new Material(Shader.Find("Sprites/Default"));
        attackRangeRenderer.startColor = Color.blue;
        attackRangeRenderer.endColor = Color.blue;
        attackRangeRenderer.enabled = showAttackRange;

        // Create the circle points for the LineRenderer
        UpdateAttackRange();

        // Initialize level up sound
        InitializeLevelUpSound();
        // Initialize death sound
        InitializeDeathSound();
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;

        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

        bool isMoving = false;

        foreach (GameObject target in targets)
        {
            Vector3 directionToTarget = target.transform.position - transform.position;

            float angle = Vector3.Angle(transform.forward, directionToTarget);

            // Check if the target is within attack range
            if (angle <= attackAngle && attackCooldown <= 0f && Vector3.Distance(transform.position, target.transform.position) <= attackRange)
            {
                // Attack the target
                Attack(target);
                attackCooldown = attackInterval;
            }
            else
            {
                // Move towards the target
                if (angle > 5f)
                {
                    transform.LookAt(target.transform);
                }

                transform.position += transform.forward * moveSpeed * Time.deltaTime;

                RaycastHit groundHit;
                if (Physics.Raycast(transform.position, Vector3.down, out groundHit, Mathf.Infinity, groundLayer))
                {
                    transform.position = groundHit.point + Vector3.up * 0.1f;
                }

                RaycastHit obstacleHit;
                if (Physics.Raycast(transform.position, directionToTarget.normalized, out obstacleHit, directionToTarget.magnitude, obstacleLayer))
                {
                    transform.position = obstacleHit.point - directionToTarget.normalized * 1.5f;
                }

                isMoving = true;
            }
        }

        animator.SetBool("Walk", isMoving);
    }

    private void Attack(GameObject target)
    {
        animator.SetTrigger("Attack");

        HumanAI humanAI = target.GetComponent<HumanAI>();
        if (humanAI != null)
        {
            humanAI.TakeDamage(attackDamage);
            if (humanAI.IsDead())
            {
                GainExperience(expValue);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Damage");
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Play death sound
        if (deathSound != null)
        {
            deathSound.Play();
        }

        transform.position = spawnPoint;
        currentHP = maxHP;
    }

    private bool isFirstRebirth = true; // Flag to track if it's the first rebirth

    public void Rebirth()
    {
        if (level == 100)
        {
            // Reset level to 1
            level = 1;

            // Increase max HP and attack damage by 10%
            maxHP = Mathf.RoundToInt(maxHP * 1.1f);
            attackDamage = Mathf.RoundToInt(attackDamage * 1.1f);

            experience = 0;

            if (isFirstRebirth)
            {
                // Set the slime's name to "Remu" after the first rebirth
                slimeName = "Remu";
                isFirstRebirth = false; // Mark that the first rebirth has occurred
            }

            rebirthCount++; // Increment rebirth count

            Debug.Log("Slime reborn at level 1 with 10% increase in base stats.");
        }
        else
        {
            Debug.Log("Error: Slime can only rebirth at level 100.");
        }
    }

    private void GainExperience(int experience)
    {
        this.experience += experience;
        if (this.experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        if (level < 100) // Check if the slime's level is less than 100
        {
            level++;
            maxHP += 25;
            currentHP = maxHP;
            experience -=            experienceToNextLevel;
            experienceToNextLevel = (int)(experienceToNextLevel * 1.05f);
            // Increase attack damage by 5 per level
            attackDamage += 5;
            attackInterval += 0.01f;

            // Increase attack range by 0.025 per level
            attackRange += 0.025f;

            UpdateAttackRange();

            // Play level up sound
            if (levelUpSound != null)
            {
                levelUpSound.Play();
            }

            Debug.Log("Slime leveled up to level " + level);
        }
        else
        {
            Debug.Log("Slime reached maximum level (100).");
        }
    }

    private void GainExperienceFromHumanDeath(int expValue)
    {
        GainExperience(expValue);
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

    private void InitializeLevelUpSound()
    {
        // Create an AudioSource component for level up sound
        levelUpSound = gameObject.AddComponent<AudioSource>();
        levelUpSound.clip = levelUpClip;
        levelUpSound.volume = levelUpVolume;
    }

    private void InitializeDeathSound()
    {
        // Create an AudioSource component for death sound
        deathSound = gameObject.AddComponent<AudioSource>();
        deathSound.clip = deathClip;
        deathSound.volume = deathVolume;
    }

    public void SetSlimeName(string newName)
    {
        slimeName = newName;

        // Increase player stats by 20% if the name is "Remu"
        if (newName.ToLower() == "remu")
        {
            maxHP = Mathf.RoundToInt(maxHP * 1.2f);
            attackDamage = Mathf.RoundToInt(attackDamage * 1.2f);
            Debug.Log("Slime stats permanently increased by 20% after getting the name Remu.");
        }
        else
        {
            Debug.Log("Slime named " + newName + ". No special effects applied.");
        }
    }
}

