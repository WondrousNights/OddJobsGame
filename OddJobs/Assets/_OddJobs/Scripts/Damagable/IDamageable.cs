using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public interface IDamageable
{
    public float CurrentHealth { get; }
    public float MaxHealth { get; }
    public void TakeDamageFromGun(Ray ray, float damage, float hitForce, Vector3 collisionPoint, GameObject sender);

    public void TakeDamageFromMelee(Vector3 positionOfAttacker, float damage, float hitForce, Vector3 collsionPoint);

}
