using System;
using UnityEngine;

public class Explodable : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float _MaxHealth = 1;
    [SerializeField]
    private float _Health = 1;
    public float CurrentHealth {get => _Health; private set => _Health = value;}

    public float MaxHealth {get => _MaxHealth; private set => _MaxHealth = value;}

    public GameObject explosionEffect;


    [SerializeField] bool explosionTimer;
    [SerializeField] float timeToExplode;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionForce;

    [SerializeField] float damage;

    [SerializeField] float recoveryTime;


    public bool explodeOnHit = true;





    float count;

    public bool hasExploded = false;
    

    public void TakeDamage()
    {
        /*
        if(explodeOnHit)
        {
            if(_Health <= 0) return;
            if(hasExploded) return;
            _Health -= damage;

            if(_Health <= 0)
            {
            if(hasExploded) return;
            Explode();
            hasExploded = true;
            }
        }
        else
        {
            explosionTimer = true;
        }
        */
    }

    private void Explode()
    {
        if(hasExploded) return;
        Instantiate(explosionEffect, gameObject.transform.position, gameObject.transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach(Collider nearbyObject in colliders)
        {
            if(nearbyObject.gameObject != this.gameObject)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if(rb!= null)
                {
                    rb.AddExplosionForce(explosionForce,transform.position, explosionRadius);
                }
                

                IDamageable damageable = nearbyObject.GetComponent<IDamageable>();
                if(damageable != null)
                {      
                    Explodable explodable = nearbyObject.GetComponent<Explodable>();
                    if(explodable != null)
                    {
                        if(explodable.hasExploded)
                        {
                            return;
                        }
                        else
                        {
                            explodable.TakeDamageFromMelee(transform.position, damage, explosionForce, gameObject.transform.position, recoveryTime);
                        }
                    }
                    else
                    {
                        //damageable.TakeDamageFromMelee(transform.position, damage, explosionForce, gameObject.transform.position, recoveryTime);
                    }

                }
                }
           
        }
        hasExploded = true;
        Destroy(gameObject);
    }

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(explosionTimer)
        {
            count += Time.deltaTime;
            if(count >= timeToExplode)
            {
                if(!hasExploded)
                {
                    Explode();
                    hasExploded = true;
                }
               
            }
        }
    }






    public void TakeDamageFromMelee(Vector3 positionOfAttacker, float damage, float hitForce, Vector3 collsionPoint, float recoveryTime)
    {
        if(explodeOnHit)
        {
if(_Health <= 0) return;
        if(hasExploded) return;
        _Health -= damage;

       if(_Health <= 0)
       {
          if(hasExploded) return;
            Explode();
            hasExploded = true;
       }
        }
        

       else
        {
            explosionTimer = true;
        }
        
    }
}
