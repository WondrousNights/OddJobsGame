using Unity.Netcode;
using UnityEngine;

public class Network_ItemPickup : Network_Interactable
{

    [SerializeField] private bool autoPickup = false;
    public int ammoInClip = -1;
    [Tooltip("Set to -1 to use the gun's default ammo clip size")]
    // [SerializeField] private int timeToRespawn = -1;
    // [Tooltip("Set to -1 to never respawn")]

    bool isPickedUp = false;


    protected override void Interact(PlayerManager playerManager){ }

    [Rpc(SendTo.Everyone, RequireOwnership = false)]
    public void DestoryItemRpc()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (autoPickup && collision.gameObject.GetComponent<PlayerManager>())
            Interact(collision.gameObject.GetComponent<PlayerManager>());
    }

   
}