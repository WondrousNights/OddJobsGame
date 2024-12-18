using UnityEngine;

public class PlayerGrenadeHandler : MonoBehaviour
{
    [SerializeField] float throwForce;
    [SerializeField] Transform grenadeThrowTransform;
    public GameObject grenadePrefab;

    public int grenadeCount;

    public void ThrowGrenade()
    {
        if(grenadeCount > 0)
        {
            GameObject grenade = Instantiate(grenadePrefab, grenadeThrowTransform.position, grenadeThrowTransform.rotation);
            Rigidbody rb = grenade.GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
            grenadeCount -= 1;
        }
    }
}
