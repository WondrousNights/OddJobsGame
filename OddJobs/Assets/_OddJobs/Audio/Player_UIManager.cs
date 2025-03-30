using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_UIManager : MonoBehaviour
{
    Network_InventoryUI inventoryUI;
    Network_HealthManager healthManager;
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] Image healthImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryUI = GetComponent<Network_InventoryUI>();
        healthManager = GetComponentInParent<Network_HealthManager>();
        
        healthManager.OnRespawn += UpdateHealthBar;
    }

    public void UpdateInventoryUI()
    {
        //inventoryUI.
    }

    public void UpdateInteractText(string text)
    {
        interactText.text = text;
    }
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthImage.fillAmount = currentHealth / maxHealth;
    }
}
