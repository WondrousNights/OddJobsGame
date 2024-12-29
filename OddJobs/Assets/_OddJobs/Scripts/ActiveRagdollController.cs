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

    public bool forward;

    public bool backward;

    public bool right;

    public bool left;

    void Update()
    {
        //Puppet Stablizing Force
       head.AddForce(up * constantUpwardForce);
       hips.AddForce(up * constantUpwardForce);

       leftShin.AddForce(-up * constantDownwardForce);
       rightShin.AddForce(-up * constantDownwardForce);

        if(forward)
        {
            hips.AddForce(hips.transform.forward * speed);
        }
        if(backward)
        {
            hips.AddForce((hips.transform.forward * speed) * -1);
        }
        if(left)
        {
            hips.AddForce(hips.transform.right * speed * 2);
        }
        if(right)
        {
            hips.AddForce(hips.transform.right * speed * 2 * -1);
        }


    }
    


  
}
