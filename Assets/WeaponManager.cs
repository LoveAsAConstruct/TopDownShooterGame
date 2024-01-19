using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapons")]
    public WeaponData startingWeapon; // The initial weapon the player starts with
    private WeaponData currentWeaponData; // Reference to the currently equipped weapon data
    private GameObject currentWeaponInstance; // Reference to the currently equipped weapon instance

    [Header("References")]
    public Transform firePoint; // Transform where projectiles are spawned
    public LayerMask hitLayer; // Layer mask for raycasting hits (if using raycast-based weapons)
    public Transform weaponRoot; // Transform representing the weapon's root (where the weapon pivots)

    private bool isReloading = false;

    private void Start()
    {
        // Equip the starting weapon when the game starts
        EquipWeapon(startingWeapon);
    }

    private void Update()
    {
        if (!isReloading)
        {
            // Rotate the weapon to point at the mouse
            RotateWeaponTowardsMouse();

            // Handle shooting input
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
    }

    private void RotateWeaponTowardsMouse()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure that the z-coordinate is the same as the character's position

        // Calculate the direction from the weaponRoot to the mouse position
        Vector3 directionToMouse = mousePosition - weaponRoot.position;

        // Rotate the weaponRoot to look at the mouse---
        weaponRoot.right = directionToMouse.normalized;
    }

    public void Shoot()
    {
        if (currentWeaponData != null)
        {
            currentWeaponData.Shoot(firePoint); // Pass the player's transform
        }
    }

    public void EquipWeapon(WeaponData weaponToEquip)
    {
        if (weaponToEquip != null)
        {
            // Destroy the currently equipped weapon instance (if any)
            if (currentWeaponInstance != null)
            {
                Destroy(currentWeaponInstance);
            }

            // Create an instance of the new weapon
            //currentWeaponInstance = Instantiate(weaponToEquip.weaponPrefab, firePoint.position, Quaternion.identity);

            // Set the current weapon data to the new weapon
            currentWeaponData = weaponToEquip;

            // Parent the new weapon instance to the weaponRoot
            //currentWeaponInstance.transform.parent = weaponRoot;
            //currentWeaponInstance.transform.localPosition = Vector3.zero;
            //currentWeaponInstance.transform.localRotation = Quaternion.identity;
        }
    }
}
