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
            // Apply a friction-like effect for stopping quickly
            rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, stopSpeed * Time.fixedDeltaTime);
        }
    }

    System.Collections.IEnumerator Dash()
    {
        isDashing = true;
        float startTime = Time.time;
        Vector2 initialDashDirection = moveDirection.normalized;

        // Sprite and Particle Effects
        //characterSprite.sprite = dashSprite;
        dashEffect.Play();
        //dashSound.Play();

        while (Time.time < startTime + dashDuration)
        {
            rb.velocity = initialDashDirection * dashSpeed;
            yield return null;
        }

        // Revert to normal sprite
        //characterSprite.sprite = /* your normal sprite */;
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
}
