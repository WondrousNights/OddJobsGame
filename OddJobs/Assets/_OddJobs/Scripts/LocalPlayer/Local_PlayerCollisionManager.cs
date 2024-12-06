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
        // Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "MeleeWeapon")
        {
            EnemyWeapon weapon = other.gameObject.GetComponent<EnemyWeapon>();

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

    float pushPower = 2.0f;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.linearVelocity = pushDir * pushPower;
    }
}
