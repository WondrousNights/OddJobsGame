using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{

   public float ragdollForceMagnitude;
   public bool isAttacking;

   public float damage;

   public void BaseAttack()
    {
        Attack();
    }

    protected virtual void Attack()
    {

    }
}
