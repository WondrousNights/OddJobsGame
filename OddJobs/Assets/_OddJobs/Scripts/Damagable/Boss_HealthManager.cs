using System;
using UnityEngine;
using UnityEngine.UI;

public class Boss_HealthManager : Enemy_HealthManager
{
    [SerializeField] GameObject bossCanvas;
    [SerializeField] Image bossHealthUI;

    [SerializeField] Enemy_PerceptionManager perception;

    void Start()
    {
        perception = GetComponent<Enemy_PerceptionManager>();
        OnDamageTaken += UpdateHealthUI;
        perception.RegisterPerceptionEvent<GameObject>("OnTargetSpotted", HandleTargetSpotted);
        bossCanvas.SetActive(false);
    }

    private void HandleTargetSpotted(GameObject @object)
    {
        bossCanvas.SetActive(true);
    }

    private void UpdateHealthUI(Ray ray)
    {
        bossHealthUI.fillAmount = health / MaxHealth;
    }

    public override void TakeDamageRpc(float damage, float hitForce, Ray ray, Vector3 vector3)
    {
        base.TakeDamageRpc(damage, hitForce, ray, vector3);
    }


}
