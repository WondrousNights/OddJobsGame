using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public interface IDamageable
{
    public void TakeDamageRpc(float damage, float hitForce, Ray ray, Vector3 forcePosition);
}
