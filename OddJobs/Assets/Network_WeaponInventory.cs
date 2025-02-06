using UnityEngine;

public class Network_WeaponInventory : MonoBehaviour
{
    [SerializeField] Weapon[] weapons;
    private int currentWeaponIndex = 0;

    public Weapon GetCurrentWeapon()
    {
        return weapons[currentWeaponIndex];
    }
}
