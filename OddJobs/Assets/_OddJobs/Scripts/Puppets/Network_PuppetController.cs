using UnityEngine;
using RootMotion.Dynamics;
using Unity.Netcode;
using System.Collections;

public class Network_PuppetController : NetworkBehaviour
{
    private PuppetMaster puppetMaster;
    private Collider[] puppetColliders = new Collider[0];
    [SerializeField] int playerPuppetLayer;
    [SerializeField] bool isPlayerPuppet;

    [SerializeField] Transform puppetRootBone;
    [SerializeField] Transform animatorRootBone;

    bool checkPuppet = true;

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

    void Update()
    {
        if(IsOwner) return;
        if(!checkPuppet) return;
        if(Vector3.Distance(puppetRootBone.position, animatorRootBone.position) >= 5f)
        {
            puppetMaster.state = PuppetMaster.State.Dead;
            Invoke("ResetPuppet", 1f);
            checkPuppet = false;

        }
    }

    void ResetPuppet()
    {
        puppetMaster.state = PuppetMaster.State.Alive;
        checkPuppet = true;
    }
}
