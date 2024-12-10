using System.Collections;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField] GameObject muzzleFlash;

    bool muzzleFlashShowing = false;

    float timeToShow = 0.1f;
    float count = 0;

    void Awake()
    {
        muzzleFlashShowing = false;
    }
    public void Play()
    {
        muzzleFlashShowing = true;
        count = 0;
    }


    void Update()
    {
        count += Time.deltaTime;
        if(count <= timeToShow)
        {
            muzzleFlashShowing = true;
        }
        else{
            muzzleFlashShowing = false;
        }
        

        if(muzzleFlashShowing)
        {
            muzzleFlash.SetActive(true);
        }
        else
        {
            muzzleFlash.SetActive(false);
        }


    }

    
    
}
