using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Local_PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;

    [SerializeField]
    private Image healthImage;

    [SerializeField]
    private TextMeshProUGUI ammoText;

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }

    public void UpdateAmmoText(int currentAmmo, int maxAmmo)
    {
        ammoText.text = currentAmmo + " / " + maxAmmo;
    }

    public void UpdateHealthImage(float currentHealth, float MaxHealth)
    {
        healthImage.fillAmount = currentHealth / MaxHealth;
    }
}
