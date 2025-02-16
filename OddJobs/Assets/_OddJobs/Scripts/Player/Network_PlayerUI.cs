using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Network_PlayerUI : MonoBehaviour
{
  
    public TextMeshProUGUI promptText;

    [SerializeField]
    private Image healthImage;

    [SerializeField] private TextMeshProUGUI currentItemText;
    [SerializeField] private TextMeshProUGUI ammoInClipText;
    [SerializeField] private TextMeshProUGUI lightAmmoSupplyText;
    [SerializeField] private TextMeshProUGUI mediumAmmoSupplyText;
    [SerializeField] private TextMeshProUGUI heavyAmmoSupplyText;

    private void Awake()
    {
        //UpdateAmmoText(null);
    }   

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }

    public void UpdateAmmoText(Weapon weapon, int inClip = -1, int lightAmmoSupply = -1, int mediumAmmoSupply = -1, int heavyAmmoSupply = -1)
    {
        /*
        if (gun)
        {
            currentItemText.text = gun.name;
            ammoInClipText.text = inClip.ToString() + " / " + gun.ClipSize;
        }
        else
        {
            currentItemText.text = "";
            ammoInClipText.text = "";
        }
        lightAmmoSupplyText.text = "Light ammo: " + lightAmmoSupply.ToString();
        mediumAmmoSupplyText.text = "Medium ammo: " + mediumAmmoSupply.ToString();
        heavyAmmoSupplyText.text = "Heavy ammo: " + heavyAmmoSupply.ToString();
        */
    }

    public void UpdateHealthImage(float currentHealth, float MaxHealth)
    {
        healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, Mathf.Clamp01(currentHealth / 100f), Time.deltaTime * 10);
    }
}
