using UnityEngine;

public class MeleeWeapon : Weapon
{
    public override void DestroyWeapon()
    {
        //throw new System.NotImplementedException();
    }

    public override void HideWeapon()
    {
        //throw new System.NotImplementedException();
    }

    public override void Reload(int AmmoToReload)
    {
        //throw new System.NotImplementedException();
    }

    public override void ReloadEffects()
    {
        //throw new System.NotImplementedException();
    }

    public override void ShootEffects()
    {
        //throw new System.NotImplementedException();
    }

    public override void ShowWeapon()
    {
        //throw new System.NotImplementedException();
    }

    public override void UseWeapon(Ray ray, bool isPlayer)
    {
        LastShootTime = Time.time;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, (int)meleeProperties.range))
        {
            if(hit.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamageRpc(meleeProperties.damage, meleeProperties.hitForce, ray, hit.point);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
