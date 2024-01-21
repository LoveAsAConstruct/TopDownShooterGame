using UnityEngine;
using Mirror;

public class WeaponPickup : NetworkBehaviour
{
    public WeaponData weaponData;
    private WeaponData clonedWeaponData;
    private SpriteRenderer spriteRenderer;

    private GameObject playerInTriggerWith = null;

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
        if (playerInTriggerWith != null && Input.GetKeyDown(KeyCode.E))
        {
            AttemptPickup();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            playerInTriggerWith = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == playerInTriggerWith)
        {
            playerInTriggerWith = null;
        }
    }

    private void AttemptPickup()
    {
        if (playerInTriggerWith != null)
        {
            print("calling weapon pickup");
            playerInTriggerWith.GetComponent<WeaponManager>().CmdPickupWeapon(gameObject);
        }
    }
}
