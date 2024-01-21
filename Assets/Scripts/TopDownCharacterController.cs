using UnityEngine;
using UnityEngine.UI;

public class TopDownCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float stopSpeed = 10f;
    public float dashCost = 10f;
    public float staminaRegen = 25f;
    public Slider staminaSlider;

    public ParticleSystem dashEffect; // Assign in Unity Editor
    public SpriteRenderer characterSprite; // Assign your character's sprite renderer
    public Sprite dashSprite; // Your dash sprite
    public AudioSource dashSound; // Dash sound effect

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float lastDashTime;
    private bool isDashing;
    private float maxStamina = 100f;
    private float currentStamina;

    // For bloody footprints
    public GameObject leftFootprintPrefab;  // Assign in Unity Editor
    public GameObject rightFootprintPrefab; // Assign in Unity Editor
    private float lastTimeOnBlood;          // Time when last stepped on blood
    private bool isLeavingFootprints = false;
    private bool isLeftFoot = true;         // To alternate between left and right foot
    private float footprintSpacing = 0.75f;  // Space between footprints
    private Vector2 lastFootprintPosition;
    private const float footprintDuration = 5f; // Duration to leave footprints after leaving blood


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentStamina = maxStamina; // Initialize stamina
        UpdateStaminaUI(); // Update UI at the start
    }

    void Update()
    {
        ProcessInputs();

        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time > lastDashTime + dashCooldown && currentStamina >= dashCost)
        {
            StartCoroutine(Dash());
            ConsumeStamina(dashCost);
        }

        // Optional: Regenerate stamina over time
        RegenerateStamina(Time.deltaTime * staminaRegen); // Regenerate at a rate of 5 units per second

        if (isLeavingFootprints && Time.time <= lastTimeOnBlood + footprintDuration)
        {
            TryPlaceFootprint();
        }
        else if (Time.time > lastTimeOnBlood + footprintDuration)
        {
            isLeavingFootprints = false;
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            Move();
        }
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        if (moveDirection != Vector2.zero)
        {
            rb.velocity = moveDirection * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, stopSpeed * Time.fixedDeltaTime);
        }
    }

    System.Collections.IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;
        Vector2 initialDashDirection = moveDirection.normalized;

        dashEffect.Play();

        while (Time.time < startTime + dashDuration)
        {
            rb.velocity = initialDashDirection * dashSpeed;
            yield return null;
        }

        if (isLeavingFootprints)
        {
            lastTimeOnBlood = Mathf.Max(lastTimeOnBlood, Time.time - 3f); // Reduce the footprint time by 3 seconds
        }

        isDashing = false;
        lastDashTime = Time.time;
    }

    void ConsumeStamina(float amount)
    {
        currentStamina = Mathf.Max(currentStamina - amount, 0);
        UpdateStaminaUI();
    }

    void RegenerateStamina(float amount)
    {
        if (currentStamina < maxStamina)
        {
            currentStamina = Mathf.Min(currentStamina + amount, maxStamina);
            UpdateStaminaUI();
        }
    }

    void UpdateStaminaUI()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina / maxStamina;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BloodSurface"))
        {
            isLeavingFootprints = true;
            lastFootprintPosition = transform.position;
            lastTimeOnBlood = Time.time;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("BloodSurface"))
        {
            lastTimeOnBlood = Time.time;
        }
    }

    private void TryPlaceFootprint()
    {
        if (Vector2.Distance(transform.position, lastFootprintPosition) >= footprintSpacing)
        {
            PlaceFootprint();
            lastFootprintPosition = transform.position;
        }
    }

    private void PlaceFootprint()
    {
        GameObject footprintPrefab = isLeftFoot ? leftFootprintPrefab : rightFootprintPrefab;
        Quaternion footprintRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, moveDirection));

        // Calculate the offset position for left and right footprints
        Vector2 offset = (isLeftFoot ? Vector2.Perpendicular(rb.velocity).normalized : -Vector2.Perpendicular(rb.velocity).normalized) * 0.3f;
        Vector2 footprintPosition = (Vector2)transform.position + offset;

        Instantiate(footprintPrefab, footprintPosition, footprintRotation);
        isLeftFoot = !isLeftFoot;
    }

    public void ApplyRecoil(Vector2 direction, float strength)
    {
        // Apply recoil force in the opposite direction of firing
        rb.AddForce(-direction * strength, ForceMode2D.Impulse);
    }
}
