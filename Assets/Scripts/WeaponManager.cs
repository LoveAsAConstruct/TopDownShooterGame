using UnityEngine;

public class WeaponManager : MonoBehaviour
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
        if (currentWeaponData != null)
        {
            GameObject thrownWeapon = Instantiate(weaponPickupPrefab, firePoint.position, Quaternion.identity);

            // Pass the currentWeaponData to the WeaponPickup script
            WeaponPickup pickupScript = thrownWeapon.GetComponent<WeaponPickup>();
            if (pickupScript != null)
            {
                pickupScript.weaponData = currentWeaponData;
            }

            Rigidbody2D rb = thrownWeapon.GetComponent<Rigidbody2D>();
            Vector3 throwDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            throwDirection.z = 0;
            rb.AddForce(throwDirection * throwForce);
            rb.angularVelocity = 720;

            // Clear current weapon data and sprite
            weaponImage.sprite = null;
            currentWeaponData = null;
        }
    }

}
