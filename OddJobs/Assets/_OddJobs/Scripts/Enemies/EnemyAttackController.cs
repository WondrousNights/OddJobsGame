using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{

    [SerializeField] Weapon weapon;

    public void AttackEvent()
    {
        Debug.Log("Attack event fired!");
        weapon.BaseAttack();
    }
    
   
}
