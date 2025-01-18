using System;
using Unity.Netcode;
using UnityEngine;

public class Van_LevelLoader : NetworkBehaviour
{
    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void LoadLevelRpc()
    {
        Loader.LoadNetwork(Loader.Scene.Network_Valley);
    }
    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void LoadExtractRpc()
    {
        Loader.LoadNetwork(Loader.Scene.Network_DesertLevel);
    }
}
