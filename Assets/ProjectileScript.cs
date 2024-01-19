using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private float damage;
    public GameObject trail;
    public int bounces;
    // Set the damage for this projectile
    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bounces--;
        // Check if the projectile hits an object with a health component
        HealthComponent healthComponent = collision.gameObject.GetComponent<HealthComponent>();

        if (healthComponent != null)
        {
            // Deal damage to the target
            healthComponent.TakeDamage(damage);
        }
        // Destroy the projectile on impact
        if (bounces < 0)
        {
            if (trail != null)
            {
                trail.transform.parent = null;
            }
            Destroy(gameObject);
        }
    }
}
