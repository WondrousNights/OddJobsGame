using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour
{
    [HideInInspector] public Image Sprite;
    [HideInInspector] public GameObject Model;
    [HideInInspector] public GameObject OtherModel;
    [HideInInspector] public int ammoInClip;
    [HideInInspector] public float LastShootTime;
    public Network_GunProperties gunProperties;
    public Network_MeleeWeaponProperties meleeProperties;
    public abstract void UseWeapon(Ray ray, bool isPlayer);
    public abstract void ShootEffects();
    public abstract void Reload(int AmmoToReload);
    public abstract void DestroyWeapon();
    public abstract void HideWeapon();
    public abstract void ShowWeapon();
    public abstract void ReloadEffects();
}
