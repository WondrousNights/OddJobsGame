using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour
{
    [HideInInspector] public Image Sprite;
    [HideInInspector] public GameObject Model;
    [HideInInspector] public GameObject OtherModel;
    public Network_GunProperties weaponProperties;
    public abstract void UseWeapon(Ray ray);
    protected abstract void UpdateUI();
}
