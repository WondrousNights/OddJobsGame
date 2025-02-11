using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_UIManager : MonoBehaviour
{
    Network_InventoryUI inventoryUI;
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] Image healthImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryUI = GetComponent<Network_InventoryUI>();
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
        healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, Mathf.Clamp01(currentHealth / maxHealth), Time.deltaTime * 10);
    }
}
