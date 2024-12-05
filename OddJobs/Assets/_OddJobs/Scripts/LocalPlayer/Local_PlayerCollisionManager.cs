using UnityEngine;

public class Local_PlayerCollisionManager : MonoBehaviour
{

    Local_PlayerInputController playerInputController;
    Local_PlayerHealthManager playerHealthManager;

    void Start()
    {
        playerHealthManager = GetComponent<Local_PlayerHealthManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "MeleeWeapon")
        {
            Weapon weapon = other.gameObject.GetComponent<Weapon>();

            if(weapon.isAttacking)
            {

                //Ragdoll physics hit information
                Vector3 forceDirection = this.transform.position - weapon.transform.position;
                forceDirection.y = 1;
                forceDirection.Normalize();

                Vector3 collisionPoint = other.transform.position;

                Vector3 force = weapon.ragdollForceMagnitude * forceDirection;

                playerHealthManager.TakeDamage(force, collisionPoint);
            }
        }
    }
}
