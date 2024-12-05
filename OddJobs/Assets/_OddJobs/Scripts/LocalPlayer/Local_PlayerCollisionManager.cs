using UnityEngine;

public class Local_PlayerCollisionManager : MonoBehaviour
{

    Local_PlayerInputController playerInputController;

    void Start()
    {
        playerInputController = GetComponent<Local_PlayerInputController>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "MeleeWeapon")
        {
            Weapon weapon = other.gameObject.GetComponent<Weapon>();

            if(weapon.isAttacking)
            {
                playerInputController.TakeDamage();
            }
        }
    }
}
