using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponData weaponData;
    private WeaponData clonedWeaponData;
    private SpriteRenderer spriteRenderer;

    private bool playerInTrigger = false; // Flag to indicate player is in trigger area

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        clonedWeaponData = Instantiate(weaponData);

        if (clonedWeaponData != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = clonedWeaponData.weaponSprite;
        }
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            // Implement pickup logic here
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.GetComponent<WeaponManager>().EquipWeapon(clonedWeaponData);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }
}
