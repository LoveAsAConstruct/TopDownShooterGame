using UnityEngine;
using UnityEngine.Events;

public enum WeaponType
{
    Ranged,
    Melee
}

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public WeaponType weaponType;
    public Vector2 muzzlePosition;

    // Common properties
    public string weaponName;
    public float damage;
    public Sprite weaponSprite;
    public AudioClip shotSound; // Optional sound to be played on shooting
    public UnityEvent onShoot; // Optional event to be triggered on shooting

    // Ranged weapon properties
    [Header("Ranged Properties")]
    public float fireRate;
    public float range;
    public int maxAmmo;
    public int bulletSpeed;
    public float recoilStrength;
    public float accuracySpread; // Combined property for accuracy and spread
    public int bulletsPerShot; // Number of bullets fired per shot
    public GameObject projectilePrefab;

    // Melee weapon properties
    [Header("Melee Properties")]
    public float attackSpeed;
    public float attackRange;
    public int durability; // or any other relevant property for melee weapons


    public void Shoot(Transform firePoint)
    {
        if (weaponSprite != null)
        {
            float ppu = weaponSprite.pixelsPerUnit; // Pixels Per Unit of the sprite
            Vector3 spriteSize = new Vector3(weaponSprite.bounds.size.x / ppu, weaponSprite.bounds.size.y / ppu, 1f);

            // Convert muzzlePosition from local space (relative to the center of the sprite)
            // to world space, considering the sprite size and the firePoint's scale
            Vector3 localMuzzlePosition = new Vector3(
                muzzlePosition.x * 100 * spriteSize.x * firePoint.localScale.x,
                muzzlePosition.y * 100 * spriteSize.y * firePoint.localScale.y,
                0);

            Vector3 worldMuzzlePosition = firePoint.TransformPoint(localMuzzlePosition);
            Debug.DrawRay(Vector2.zero, worldMuzzlePosition, Color.red,1);
            // Draw a debug dot at the muzzle position
            Debug.DrawRay(worldMuzzlePosition, firePoint.forward * 0.1f, Color.red, 0.1f);
        }   
        Debug.DrawRay(Vector2.zero, Vector2.one * 0.1f, Color.red, 1);
        if (weaponType == WeaponType.Ranged)
        {
            // Ranged weapon shooting logic
            if (maxAmmo <= 0)
            {
                Debug.Log("Out of ammo!");
                return;
            }

            for (int i = 0; i < bulletsPerShot; i++)
            {
                float randomSpreadAngle = Random.Range(-accuracySpread / 2f, accuracySpread / 2f);
                Quaternion spreadRotation = Quaternion.Euler(0f, 0f, randomSpreadAngle);
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
                        Debug.Log("Raycast hit: " + hit.collider.name); // Debug log for raycast hit
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

            maxAmmo -= bulletsPerShot;
        }
        else if (weaponType == WeaponType.Melee)
        {
            // Melee attack logic
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, attackRange);
            if (hit.collider != null)
            {
                HealthComponent healthComponent = hit.collider.GetComponent<HealthComponent>();
                if (healthComponent != null)
                {
                    healthComponent.TakeDamage(damage);
                    // Additional melee hit logic
                }
                Debug.Log("Melee hit: " + hit.collider.name); // Debug log for melee hit
            }

            // Additional logic for melee attack, e.g., animation, sound, etc.
        }
        PlaySound();
        onShoot.Invoke();
        // Additional shared logic, if any
    }

    private void PlaySound()
    {
        if (shotSound != null)
        {
            AudioSource.PlayClipAtPoint(shotSound, Camera.main.transform.position);
        }
    }
}
