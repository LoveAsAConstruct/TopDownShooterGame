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
    public Sprite weaponSprite;
    public GameObject projectilePrefab; // Prefab for the projectile
    //public Transform firePoint; // Transform where projectiles are spawned

    // Create a method for shooting using this weapon
    public void Shoot(Transform firePoint)
    {
        // Check if there is ammunition remaining
        if (maxAmmo <= 0)
        {
            Debug.Log("Out of ammo!");
            return;
        }

        // Check if it's a raycast-based weapon
        if (bulletSpeed == 0)
        {
            // Perform a 2D raycast
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, range);

            if (hit.collider != null)
            {
                // Check if the ray hits an enemy or object with a health component
                HealthComponent healthComponent = hit.collider.GetComponent<HealthComponent>();
                if (healthComponent != null)
                {
                    // Deal damage to the target
                    healthComponent.TakeDamage(damage);
                }

                // Add visual and audio effects for the hit (e.g., impact effects, gunshot sound)
            }
        }
        else // Projectile-based weapon
        {
            // Create and launch a projectile
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = firePoint.right * bulletSpeed;

            // Set the damage property of the projectile (if needed)
            ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();
            if (projectileScript != null)
            {
                projectileScript.SetDamage(damage);
            }
        }

        // Reduce ammunition
        maxAmmo--;

        // Add additional shooting-related logic as needed
    }
}
