using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public ParticleSystem damageParticleSystem; // Particle system for damage effect
    public AnimationCurve emissionRateCurve; // Curve to control the emission rate based on health
    public AnimationCurve particleSizeCurve; // Curve to control the particle size based on health

    private void Start()
    {
        currentHealth = maxHealth;
        if (damageParticleSystem != null)
        {
            // Initialize particle system parameters
            UpdateParticleSystem();
        }
    }

    // Function to take damage
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        // Update particle system based on new health
        if (damageParticleSystem != null)
        {
            UpdateParticleSystem();
        }

        // Check if the object should be destroyed when its health reaches zero
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // Function to handle the object's death
    private void Die()
    {
        // Implement death behavior here, such as playing death animations, particle effects, or destroying the object
        Destroy(gameObject);
    }

    // Function to update particle system based on current health
    private void UpdateParticleSystem()
    {
        var emission = damageParticleSystem.emission;
        var main = damageParticleSystem.main;

        // Calculate the health percentage
        float healthPercentage = currentHealth / maxHealth;

        // Update emission rate and particle size based on the health percentage
        emission.rateOverTime = emissionRateCurve.Evaluate(1 - healthPercentage); // Assuming the curve's x-axis is normalized
        main.startSize = particleSizeCurve.Evaluate(1 - healthPercentage); // Assuming the curve's x-axis is normalized
    }
}
