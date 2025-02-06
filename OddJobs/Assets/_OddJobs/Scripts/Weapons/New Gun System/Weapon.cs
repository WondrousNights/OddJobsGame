using Unity.Netcode;
using UnityEngine;

public abstract class Weapon : NetworkBehaviour
{
    //Spawns the Weapon for the player
    protected abstract void SpawnWeapon(Transform parent, Vector3 position, Vector3 rotation);
    public abstract void UseWeapon(Ray ray);
    protected abstract void UpdateUI();
    protected abstract void DropWeapon();
    protected abstract void PickupWeapon();
}
