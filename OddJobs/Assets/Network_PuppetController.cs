using UnityEngine;
using RootMotion.Dynamics;
using Unity.Netcode;

public class Network_PuppetController : NetworkBehaviour
{
    private PuppetMaster puppetMaster;
    private Collider[] puppetColliders = new Collider[0];

    [SerializeField] int playerPuppetLayer;

    [SerializeField] bool isPlayerPuppet;

    void Awake()
    {
        puppetMaster = GetComponentInChildren<PuppetMaster>();
        puppetColliders = puppetMaster.GetComponentsInChildren<Collider>();
    }

    void Start()
    {
        if(isPlayerPuppet)
        {
            if(IsOwner)
             {
            for (int i = 0; i < puppetColliders.Length; i++)
            {
                puppetColliders[i].gameObject.layer = playerPuppetLayer;
            }

            puppetMaster.name = "My Puppet";
            }
            else
            {
            puppetMaster.name = "Other Puppet";
            }
        }
        else
        {
            puppetMaster.name = "Enemy Puppet";
        }
        
    }
}
