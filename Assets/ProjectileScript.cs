using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private float damage;

    // Set the damage for this projectile
    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile hits an object with a health component
        HealthComponent healthComponent = other.GetComponent<HealthComponent>();

        if (healthComponent != null)
        {
            // Deal damage to the target
            healthComponent.TakeDamage(damage);
        }

        // Destroy the projectile on impact
        Destroy(gameObject);
    }
}
