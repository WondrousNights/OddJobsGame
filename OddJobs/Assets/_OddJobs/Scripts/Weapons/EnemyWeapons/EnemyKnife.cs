using System.Collections;
using UnityEngine;

public class EnemyKnife : EnemyWeapon
{
    float count;
    float countToStopConsiderAttacking = 1f;

    protected override void Attack()
    {
        count += countToStopConsiderAttacking;
    }

    void Update()
    {

        if(count <= 0)
        {
            isAttacking = false;
        }
        else
        {
            count -= Time.deltaTime;
            isAttacking = true;
        }
    }

}
