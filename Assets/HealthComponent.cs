using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // Function to take damage
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

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
}
