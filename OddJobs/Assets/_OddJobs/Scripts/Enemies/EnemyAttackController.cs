using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{

    [SerializeField] EnemyWeapon weapon;

    public void AttackEvent()
    {
        Debug.Log("Attack event fired!");
        weapon.BaseAttack();
    }
    
   
}
