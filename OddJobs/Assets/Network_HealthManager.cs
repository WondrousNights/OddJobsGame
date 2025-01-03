using RootMotion.Dynamics;
using Unity.Netcode;
using UnityEngine;

public class Network_HealthManager : NetworkBehaviour
{

    int health = 100;

    [SerializeField] PuppetMaster puppetMaster;

    public void DoDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            puppetMaster.state = PuppetMaster.State.Dead;
        }
    }

}
