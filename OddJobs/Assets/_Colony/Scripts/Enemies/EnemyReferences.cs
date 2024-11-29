using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class EnemyReferences : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public NetworkEnemyTargetSelector targetSelector;
    [HideInInspector] public NetworkEnemyMovement enemyMovement;
    [HideInInspector] public AudioSource audioSource;

    [Header("Stats")]
    public float pathUpdateDelay = 0.2f;
    public float targetCheckDelay = 0.2f;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        targetSelector = GetComponent<NetworkEnemyTargetSelector>();
        enemyMovement = GetComponent<NetworkEnemyMovement>();
        audioSource = GetComponent<AudioSource>();
    }
}
