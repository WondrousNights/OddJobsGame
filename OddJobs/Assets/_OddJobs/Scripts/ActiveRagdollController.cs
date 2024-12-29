using UnityEngine;

public class ActiveRagdollController : MonoBehaviour
{

    [SerializeField] Rigidbody hips;
    [SerializeField] Rigidbody head;
    [SerializeField] Rigidbody leftShin;
    [SerializeField] Rigidbody rightShin;
    [SerializeField] float speed;
    [SerializeField] float constantUpwardForce;
    [SerializeField] float constantDownwardForce;



    Vector3 up = new Vector3(0, 1, 0);
    void Update()
    {
       head.AddForce(up * constantUpwardForce);
       hips.AddForce(up * constantUpwardForce);
       hips.AddForce(hips.transform.forward * speed);
       

       leftShin.AddForce(-up * constantDownwardForce);
       rightShin.AddForce(-up * constantDownwardForce);
        
    }
    


  
}
