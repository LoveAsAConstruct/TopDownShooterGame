using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float damage;
    public float fireRate;
    public float range;
    public int maxAmmo;
    public int bulletSpeed;
    public float recoilStrength;
    public float accuracySpread; // Combined property for accuracy and spread
    public int bulletsPerShot; // Number of bullets fired per shot
    public Sprite weaponSprite;
    public GameObject projectilePrefab;

    // Create a method for shooting using this weapon
    public void Shoot(Transform firePoint)
    {
        // Check if there is ammunition remaining
        if (maxAmmo <= 0)
        {
            Debug.Log("Out of ammo!");
            return;
        }

        for (int i = 0; i < bulletsPerShot; i++)
        {
            // Calculate a random angle for bullet spread based on accuracySpread
            float randomSpreadAngle = Random.Range(-accuracySpread / 2f, accuracySpread / 2f);
            Quaternion spreadRotation = Quaternion.Euler(0f, 0f, randomSpreadAngle);

            // Apply spread to the fire direction
            Vector3 fireDirection = spreadRotation * firePoint.right;

            if (bulletSpeed == 0) // Raycast-based weapon
            {
                RaycastHit2D hit = Physics2D.Raycast(firePoint.position, fireDirection, range);
                if (hit.collider != null)
                {
                    HealthComponent healthComponent = hit.collider.GetComponent<HealthComponent>();
                    if (healthComponent != null)
                    {
                        healthComponent.TakeDamage(damage);
                    }
                    // Add hit effects
                }
            }
            else // Projectile-based weapon
            {
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                Vector3 spreadVelocity = spreadRotation * (fireDirection * bulletSpeed);
                rb.velocity = spreadVelocity;

                ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();
                if (projectileScript != null)
                {
                    projectileScript.SetDamage(damage);
                }
            }
        }

        // Reduce ammunition
        maxAmmo -= bulletsPerShot;

        // Additional logic
    }
}
