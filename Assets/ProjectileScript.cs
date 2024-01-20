using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private float damage;
    public GameObject trail;
    public int bounces;
    public GameObject bloodSplatterPrefab; // Particle system prefab for blood splatter

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

            // Spawn blood splatter particle system
            SpawnBloodSplatter(collision);
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

    private void SpawnBloodSplatter(Collision2D collision)
    {
        if (bloodSplatterPrefab != null)
        {
            // Instantiate the blood splatter particle system
            GameObject splatter = Instantiate(bloodSplatterPrefab, collision.contacts[0].point, Quaternion.identity);

            // Orient the particle system in the direction of the bullet's velocity
            Vector2 bulletDirection = GetComponent<Rigidbody2D>().velocity.normalized;
            float angle = 180+Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
            splatter.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
