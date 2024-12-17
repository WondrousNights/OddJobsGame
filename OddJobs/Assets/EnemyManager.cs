using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public bool isHostile;
    [SerializeField] bool hasConverstaion;

    NPC_Interactable interactable;

    [SerializeField] GameObject gunGameobject;
    void Start()
    {
        if(hasConverstaion)
        {
            interactable = GetComponent<NPC_Interactable>();
        }

        if(isHostile)
        {
            gunGameobject.SetActive(true);
        }
        else
        {
            gunGameobject.SetActive(false);
        }
    }

    public void SetHostile()
    {
        StartCoroutine(SetHostileBool(2f));

        if(hasConverstaion)
        {
            interactable.canInteract = false;
            Destroy(interactable.conversationManager.gameObject, 2f);
        }
    }


    public void SetNotHostile()
    {
        isHostile = false;
    }


    IEnumerator SetHostileBool(float duration)
    {
        yield return new WaitForSeconds(duration);
        gunGameobject.SetActive(true);
        isHostile = true;
    }
}
