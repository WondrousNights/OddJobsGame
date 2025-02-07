using UnityEngine;

public class Network_AmmoPickup : Network_ItemPickup
{
    [SerializeField] AmmoType ammoType;
    [SerializeField] int ammount;


     protected override void Interact(PlayerManager playerManager)
    {
        playerManager.ammoHandler.AddAmmo(ammoType, ammount);
      
        base.Interact(playerManager);
        DestoryItemRpc();
    }

}
