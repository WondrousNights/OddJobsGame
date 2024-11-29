using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkEnemyShooting : NetworkBehaviour
{
    private EnemyReferences references;

    [SerializeField] Transform shootPoint;
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject shotVFX;
    [SerializeField] AudioClip shotSFX;

    [Header("Stats")]
    public float fireRate;
    public int damage;
    public Vector3 spread = new Vector3(0.06f, 0.06f, 0.06f);

    private float fireRateCounter;

    private void Awake()
    {
        references = GetComponent<EnemyReferences>();
        fireRateCounter = fireRate;
    }

    private void FixedUpdate()
    {
        if (!IsHost || !IsServer) return;
        if (references.enemyMovement.inRange.Value == true)
        {
            //Start shooting
            fireRateCounter -= Time.deltaTime;

            if (fireRateCounter <= 0)
            {
                fireRateCounter = fireRate;
                //Shot fired
                Shoot();
                
            }
        
        }

    }


    void Shoot()
    {

        if (references.enemyMovement.fleeing) return;
        ShootServerRpc();
       
    }

    [ServerRpc(RequireOwnership = false)]
    void ShootServerRpc()
    {
        Vector3 direction = GetDirection();
        ShootClientRpc(direction);
    }

    [ClientRpc]
    void ShootClientRpc(Vector3 direction)
    {
        references.audioSource.clip = shotSFX;
        references.audioSource.Play();

        if (Physics.Raycast(shootPoint.position, direction, out RaycastHit hit, float.MaxValue, layerMask))
        {
            Debug.DrawLine(shootPoint.position, shootPoint.position + direction * 10f, Color.red, 1f);

            if (hit.transform.gameObject.tag == "Player")
            {
                Debug.Log("Enemy just shot the player");
                hit.transform.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
                StartCoroutine("MuzzleFlash", 0.1f);
                
            }
            
        }
    }
    IEnumerator MuzzleFlash(float duration)
    {
        shotVFX.SetActive(true);

        yield return new WaitForSeconds(duration);

        shotVFX.SetActive(false);
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;

        direction += new Vector3(
            Random.Range(-spread.x, spread.x),
            Random.Range(-spread.y, spread.y),
            Random.Range(-spread.z, spread.z)
            );

        direction.Normalize();
        return direction;
    }
}
