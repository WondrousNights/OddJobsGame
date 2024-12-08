using UnityEngine;

public interface IDamageable
{
    public float CurrentHealth { get; }
    public float MaxHealth { get; }
    public void TakeDamage(Ray ray, Vector3 positionOfAttacker, float Damage, float hitForce, Vector3 collisionPoint);


}
