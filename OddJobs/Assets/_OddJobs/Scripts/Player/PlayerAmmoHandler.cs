using UnityEngine;

public class PlayerAmmoHandler : MonoBehaviour
{
    public int lightAmmo;
    public int mediumAmmo;
    public int heavyAmmo;

    public int currentAmmo;


    //HOW DO I MAKE THIS BETTER?
    public void ReloadAmmo(int clipSize, AmmoType ammoType)
    {
        Debug.Log("Reloading Ammo");

        if(ammoType == AmmoType.Light)
        {
            int MaxReloadAmount = Mathf.Min(clipSize, lightAmmo);
            int availableBulletsInCurrentClip = clipSize - currentAmmo;
            int reloadAmount = Mathf.Min(MaxReloadAmount, availableBulletsInCurrentClip);

            currentAmmo = currentAmmo + reloadAmount;
            lightAmmo -= reloadAmount;
        }
        if(ammoType == AmmoType.Medium)
        {
            int MaxReloadAmount = Mathf.Min(clipSize, mediumAmmo);
            int availableBulletsInCurrentClip = clipSize - currentAmmo;
            int reloadAmount = Mathf.Min(MaxReloadAmount, availableBulletsInCurrentClip);

            currentAmmo = currentAmmo + reloadAmount;
            mediumAmmo -= reloadAmount;
        }
        if(ammoType == AmmoType.Heavy)
        {
            int MaxReloadAmount = Mathf.Min(clipSize, heavyAmmo);
            int availableBulletsInCurrentClip = clipSize - currentAmmo;
            int reloadAmount = Mathf.Min(MaxReloadAmount, availableBulletsInCurrentClip);

            currentAmmo = currentAmmo + reloadAmount;
            heavyAmmo -= reloadAmount;
        }
    }

    public bool HasAmmoToReload(AmmoType ammoType)
    {
        if(ammoType == AmmoType.Light)
        {
            if(lightAmmo > 0)
            {return true;}
            else
            {return false;}
        }
        else if(ammoType == AmmoType.Medium)
        {
            if(mediumAmmo > 0)
            {return true;}
            else
            {return false;}
        }
        else if(ammoType == AmmoType.Heavy)
        {
            if(heavyAmmo > 0)
            {return true;}
            else
            {return false;}
        }
        else
        {
            return false;
        }
        
    }


    public void AddAmmo(AmmoType ammoType, int amount)
    {
        if(ammoType == AmmoType.Light)
        {
            lightAmmo += amount;
        }
        if(ammoType == AmmoType.Medium)
        {
            mediumAmmo += amount;
        }
        if(ammoType == AmmoType.Heavy)
        {
            heavyAmmo += amount;
        }
    }
    
   
}
