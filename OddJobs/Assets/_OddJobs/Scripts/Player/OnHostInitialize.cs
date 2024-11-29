using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OnHostInitialize : NetworkBehaviour
{

    [SerializeField] GameObject GameManagerToSpawn;
    private void Start()
    {

        if (IsHost && IsOwner)
        {
            Debug.Log("Spawn game manager");
            //GameObject spawnedObjectTransform = Instantiate(GameManagerToSpawn);
           // spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        }

        
     
    }
}
