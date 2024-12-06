using System.Collections;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField] GameObject muzzleFlash;
    public void Play()
    {
        StartCoroutine("MuzzleFlashTimer", 0.1f);
    }

    IEnumerator MuzzleFlashTimer(float duration)
    {
        muzzleFlash.SetActive(true);

        yield return new WaitForSeconds(duration);

        muzzleFlash.SetActive(false);
    }

}
