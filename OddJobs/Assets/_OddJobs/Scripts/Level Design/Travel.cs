using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Travel : MonoBehaviour
{
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Loader.LoadNetwork(Loader.Scene.Base);
        }
    }
}
