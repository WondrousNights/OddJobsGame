using UnityEngine;

public class Weapon : MonoBehaviour
{

   public float ragdollForceMagnitude;
   public bool isAttacking;
   public void BaseAttack()
    {
        Attack();
    }

    protected virtual void Attack()
    {

    }
}
