using UnityEngine;
using Mirror;
public class WeaponManager : NetworkBehaviour
{
    [Header("Weapons")]
    public WeaponData startingWeapon;
    private WeaponData currentWeaponData;

    [Header("References")]
    public Transform firePoint;
    public Transform weaponRoot;
    public SpriteRenderer weaponImage;

    [Header("Weapon Throwing")]
    public GameObject weaponPickupPrefab; // Assign your weapon pickup prefab in the inspector
    public float throwForce = 500f;

    private bool isReloading = false;
    private float fireTimer = 0; // Timer to control firing rate

    private void Start()
    {
        EquipWeapon(startingWeapon);
    }

    private void Update()
    {
        if (!isReloading)
        {
            RotateWeaponTowardsMouse();

            // Firing logic
            HandleFiring();

            // Throwing logic
            if (Input.GetKeyDown(KeyCode.T)) // Replace 'KeyCode.T' with your desired throw key
            {
                ThrowWeapon();
            }
        }
    }
    [Command]
    public void CmdPickupWeapon(GameObject weaponPickup)
    {
        WeaponPickup pickup = weaponPickup.GetComponent<WeaponPickup>();
        if (pickup != null)
        {
            EquipWeapon(pickup.weaponData);
            NetworkServer.Destroy(weaponPickup);
            print("picked up");
        }
    }
    private void RotateWeaponTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector3 directionToMouse = mousePosition - weaponRoot.position;
        weaponRoot.right = directionToMouse.normalized;
    }

    private void HandleFiring()
    {
        if (currentWeaponData != null)
        {
            if (currentWeaponData.fireRate > 0 && Input.GetMouseButton(0) && fireTimer <= 0)
            {
                Shoot();
                fireTimer = 1 / currentWeaponData.fireRate;
            }
            else if (currentWeaponData.fireRate < 0 && Input.GetMouseButtonDown(0) && fireTimer <= 0)
            {
                Shoot();
                fireTimer = -1 / currentWeaponData.fireRate;
            }

            if (fireTimer > 0)
            {
                fireTimer -= Time.deltaTime;
            }
        }
    }

    public void Shoot()
    {
        if (currentWeaponData != null && currentWeaponData.maxAmmo > 0)
        {
            currentWeaponData.Shoot(firePoint);
            Vector2 recoilDirection = firePoint.right;
            GetComponent<TopDownCharacterController>().ApplyRecoil(recoilDirection, currentWeaponData.recoilStrength);
        }
    }

    public void EquipWeapon(WeaponData weaponToEquip)
    {
        if (weaponToEquip != null)
        {
            weaponImage.sprite = weaponToEquip.weaponSprite;
            if (currentWeaponData != null)
            {
                Destroy(currentWeaponData);
            }
            currentWeaponData = weaponToEquip;
        }
    }

    private void ThrowWeapon()
    {
        if (!isLocalPlayer || currentWeaponData == null)
            return;

        CmdThrowWeapon(firePoint.position, CalculateThrowDirection());
    }

    [Command]
    private void CmdThrowWeapon(Vector3 position, Vector3 direction)
    {
        GameObject thrownWeapon = Instantiate(weaponPickupPrefab, position, Quaternion.identity);
        NetworkServer.Spawn(thrownWeapon, connectionToClient); // Spawning with player's authority

        WeaponPickup pickupScript = thrownWeapon.GetComponent<WeaponPickup>();
        if (pickupScript != null)
        {
            pickupScript.weaponData = currentWeaponData;
        }

        Rigidbody2D rb = thrownWeapon.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * throwForce);
        rb.angularVelocity = 720;

        RpcClearCurrentWeaponData();
    }

    [ClientRpc]
    private void RpcClearCurrentWeaponData()
    {
        weaponImage.sprite = null;
        currentWeaponData = null;
    }

    private Vector3 CalculateThrowDirection()
    {
        Vector3 throwDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        throwDirection.z = 0;
        return throwDirection;
    }


}
