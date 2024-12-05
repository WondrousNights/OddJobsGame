using UnityEngine;

public class Weapon : MonoBehaviour
{

   public bool isAttacking;
   public void BaseAttack()
    {
        Attack();
    }

    protected virtual void Attack()
    {

    }
}
