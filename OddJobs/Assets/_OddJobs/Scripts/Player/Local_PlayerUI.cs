using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Local_PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;

    [SerializeField]
    private Image healthImage;

    [SerializeField] private TextMeshProUGUI currentItemText;
    [SerializeField] private TextMeshProUGUI ammoInClipText;
    [SerializeField] private TextMeshProUGUI lightAmmoSupplyText;
    [SerializeField] private TextMeshProUGUI mediumAmmoSupplyText;
    [SerializeField] private TextMeshProUGUI heavyAmmoSupplyText;

    private void Awake()
    {
        UpdateAmmoText(null);
    }   

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }

    public void UpdateAmmoText(GunScriptableObject gun, int inClip = -1, int lightAmmoSupply = -1, int mediumAmmoSupply = -1, int heavyAmmoSupply = -1)
    {
        if (gun)
        {
            currentItemText.text = gun.name;

            if (gun.AmmoType == AmmoType.Light)
            {
                ammoInClipText.text = "Clip: " + inClip.ToString() + " / " + lightAmmoSupply;
            }
            else if (gun.AmmoType == AmmoType.Medium)
            {
                ammoInClipText.text = "Clip: " + inClip.ToString() + " / " + mediumAmmoSupply;
            }
            else if (gun.AmmoType == AmmoType.Heavy)
            {
                ammoInClipText.text = "Clip: " + inClip.ToString() + " / " + heavyAmmoSupply;
            }

            lightAmmoSupplyText.text = "Light: " + lightAmmoSupply.ToString();
            mediumAmmoSupplyText.text = "Medium: " + mediumAmmoSupply.ToString();
            heavyAmmoSupplyText.text = "Heavy: " + heavyAmmoSupply.ToString();
        }
        else
        {
            currentItemText.text = "";
            ammoInClipText.text = "";
            lightAmmoSupplyText.text = "";
            mediumAmmoSupplyText.text = "";
            heavyAmmoSupplyText.text = "";
        }
    }

    public void UpdateHealthImage(float currentHealth, float MaxHealth)
    {
        healthImage.fillAmount = currentHealth / MaxHealth;
    }
}
