using System.Collections;
using System.Collections.Generic;
using AlmenaraGames;
using Unity.Netcode;
using UnityEngine;

public class Network_GunEffects : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private ParticleSystem[] shootParticles;
    [SerializeField] private GameObject muzzleFlash;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // animation should be called automatically, not needed
    public void EquipEffect()
    {
        animator.SetTrigger("Equip");
    }

    public void ShootEffect()
    {
        animator.SetTrigger("Shoot");
        muzzleFlash.SetActive(true);
        foreach(ParticleSystem i in shootParticles) {
            i.Play();
        }
    }

    public void ReloadEffect()
    {
        animator.SetTrigger("Reload");
    }


}
