using UnityEngine;

public class ActiveRagdollController : MonoBehaviour
{

    [SerializeField] Rigidbody hips;
    [SerializeField] Rigidbody leftShin;
    [SerializeField] Rigidbody rightShin;
    [SerializeField] float speed;
    [SerializeField] float constantUpwardForce;
    [SerializeField] float constantDownwardForce;


    void Update()
    {
       hips.AddForce(hips.transform.up * constantUpwardForce);

       hips.AddForce(hips.transform.forward * speed);
       

       leftShin.AddForce(leftShin.transform.up * constantDownwardForce);
       rightShin.AddForce(rightShin.transform.up * constantDownwardForce);
        
    }
    


  
}
