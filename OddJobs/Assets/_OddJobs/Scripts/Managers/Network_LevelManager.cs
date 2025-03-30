using Unity.Netcode;
using UnityEngine;

public class Network_LevelManager : NetworkBehaviour
{
    int waterBarrelCount = 0;

    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void AddWaterRpc()
    {
        waterBarrelCount += 1;
        Debug.Log(waterBarrelCount);
    }

    public void Extract()
    {
        if(IsHost)
        {
            PlayerPrefs.SetInt("WaterFromExtraction", waterBarrelCount);
        }
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void CheckIfAllPlayersDeadRpc()
    {
       var totalPlayer = NetworkManager.Singleton.ConnectedClientsList.Count;
       var totalDead = 0;
       for(var i = 0; i < totalPlayer; i++)
       {
            Network_HealthManager playerHealth = NetworkManager.Singleton.ConnectedClientsList[i].PlayerObject.GetComponentInChildren<Network_HealthManager>();
            if(playerHealth.isDead)
            {
                totalDead += 1;
            }
       }

        if(totalDead >= totalPlayer)
        {
            waterBarrelCount = 0;
            PlayerPrefs.SetInt("WaterFromExtraction", waterBarrelCount);
            Loader.LoadNetwork(Loader.Scene.Base);
        }

    }
}
