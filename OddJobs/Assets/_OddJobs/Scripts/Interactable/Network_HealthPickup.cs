using UnityEngine;

public class Network_HealthPickup : Network_ItemPickup
{
    [SerializeField] float amountToHeal;


    protected override void Interact(PlayerManager playerManager)
    {
        playerManager.healthManager.IncreaseHealthRpc(amountToHeal);
      
        base.Interact(playerManager);
        DestoryItemRpc();
    }
}
