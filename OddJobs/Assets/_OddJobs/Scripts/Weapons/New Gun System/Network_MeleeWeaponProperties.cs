using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeaponProperties", menuName = "Weapons/MeleeWeaponProperties", order = 0)]
public class Network_MeleeWeaponProperties : Network_WeaponProperties
{
   public float range;
   public float damage;
   public float hitForce;
}
