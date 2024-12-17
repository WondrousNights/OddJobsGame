using System;
using System.Collections;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    public bool isHostile;
    [SerializeField] bool reportToSquad;
    [SerializeField] float squadDistance;
    [SerializeField] LayerMask squadMask;
    [SerializeField] bool hasConverstaion;

    NPC_Interactable interactable;

    [SerializeField] GameObject gunGameobject;


    EnemyHealth enemyHealth;

    public UnityEvent OnDamagedEvent;


    bool isDead = false;
    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        enemyHealth.OnDeath += HandleDeath;
        enemyHealth.OnDamaged += OnDamaged;

        if(hasConverstaion)
        {
            interactable = GetComponent<NPC_Interactable>();
        }


        if(gunGameobject == null) return;
        if(isHostile)
        {
            gunGameobject.SetActive(true);
        }
        else
        {
            gunGameobject.SetActive(false);
        }
    }

    private void OnDamaged(object sender, EnemyHealth.OnDamagedEventArgs e)
    {
        if(isDead) return;
        SetHostile(0f);
        OnDamagedEvent.Invoke();
        if(reportToSquad) ReportToSquad(e.Sender);

    }

    public void SetHostile(float duration)
    {
        if(isDead) return;
        StartCoroutine(SetHostileBool(duration));

        if(hasConverstaion)
        {
            interactable.canInteract = false;
            if(interactable.conversationManager != null)
            {
                Destroy(interactable.conversationManager.gameObject, 2f);
            }
            
        }
    }


    public void SetNotHostile()
    {
        if(isDead) return;
        isHostile = false;
    }


    IEnumerator SetHostileBool(float duration)
    {
        yield return new WaitForSeconds(duration);

        if(gunGameobject != null) gunGameobject.SetActive(true);
        isHostile = true;
    }

    void HandleDeath(object sender, EventArgs e)
    {
        isDead = true;
        if(gunGameobject != null)  Destroy(gunGameobject);
        Destroy(this.GetComponent<BehaviorGraphAgent>());

    }

    void ReportToSquad(GameObject target)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, squadDistance, squadMask);

        if (colliders.Length > 0)
        {
            foreach(Collider col in colliders)
            {
                if(col.TryGetComponent<TargetDetector>(out TargetDetector targetDetector))
                {
                    targetDetector.currentTarget = target;
                }
            }

           
        }
    }

}
