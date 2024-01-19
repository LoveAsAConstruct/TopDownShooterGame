using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponData weaponData; // Reference to the original weapon data (asset)
    private WeaponData clonedWeaponData; // Reference to the cloned weapon data
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    private void Start()
    {
        // Get a reference to the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Clone the weaponData to create a unique instance
        clonedWeaponData = Instantiate(weaponData);

        // Check if a valid weaponData is assigned
        if (clonedWeaponData != null)
        {
            // Set the sprite of the pickup to the sprite from the cloned weaponData
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = clonedWeaponData.weaponSprite;
            }
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        print("colliderstay");
        if (other.CompareTag("Player")  && Input.GetKeyDown(KeyCode.E))
        {
            print("pickup");
            // If the player picks up this weapon, equip it using the cloned weaponData
            other.GetComponent<WeaponManager>().EquipWeapon(clonedWeaponData);
            Destroy(gameObject); // Remove the pickup object from the scene
        }
    }
}
