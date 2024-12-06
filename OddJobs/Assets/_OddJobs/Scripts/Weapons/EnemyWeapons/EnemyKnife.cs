using System.Collections;
using UnityEngine;

public class EnemyKnife : EnemyWeapon
{
    
    protected override void Attack()
    {
        Debug.Log("Attacking!");
        StartCoroutine("ProccessAttackBool", 1f);
    }



    IEnumerator ProccessAttackBool(float duration)
    {
       isAttacking = true;
       
       yield return new WaitForSeconds(duration);

        isAttacking = false;

    }

}
