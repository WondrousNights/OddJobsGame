using TMPro;
using UnityEngine;

public class Player_UIManager : MonoBehaviour
{
    Network_InventoryUI inventoryUI;
    [SerializeField] TextMeshProUGUI interactText;
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
}
