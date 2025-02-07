using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Network_InventoryUI : MonoBehaviour
{
    [SerializeField] private bool debug = false;
    [SerializeField] private float spacing = 100f;
    [SerializeField] private float showDuration = 2.2f;
    [SerializeField] private GameObject inventorySlotsParent;
    [SerializeField] private GameObject[] itemSlots;
    [Tooltip("if we ever increase inventory size we have to manually update this")]

    private void Start()
    {
        //HideInventorySlots();
    }

    public void UpdateInventoryUI(Weapon[] inventory, int currentGunIndex)
    {
        if (debug) Debug.Log("Updating inventory UI");

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (debug) Debug.Log(inventory[i]);
            if (inventory[i] == null) {
                itemSlots[i].SetActive(false);
            }
            else {
                itemSlots[i].SetActive(true);

                Image slotImage = itemSlots[i].GetComponentInChildren<Image>();
                if (slotImage) slotImage.sprite = inventory[i].weaponProperties.Sprite;
                
                UpdateAmmoText(inventory, currentGunIndex);
            }
        }

        //StartCoroutine(LerpInventoryPosition());

        //CancelInvoke(nameof(HideInventorySlots));
        //ShowInventorySlots();
        //Invoke(nameof(HideInventorySlots), showDuration);
    }

    public void UpdateAmmoText(Weapon[] inventory, int currentGunIndex)
    {
        TMP_Text ammoText = itemSlots[currentGunIndex].GetComponentInChildren<TMP_Text>();

        ammoText.text = inventory[currentGunIndex].ammoInClip + "/" + inventory[currentGunIndex].weaponProperties.ClipSize;
    }

    private IEnumerator LerpInventoryPosition()
    {
        RectTransform rectTransform = inventorySlotsParent.GetComponent<RectTransform>();
        float startX = rectTransform.anchoredPosition.x;
        float targetX = -spacing /* * playerGunHandler.currentGunIndex */;
        float elapsedTime = 0f;
        float lerpDuration = 0.15f;

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lerpDuration;
            float newX = Mathf.Lerp(startX, targetX, t);
            rectTransform.anchoredPosition = new Vector2(newX, rectTransform.anchoredPosition.y);
            yield return null;
        }

        // Ensure we end up exactly at the target
        rectTransform.anchoredPosition = new Vector2(targetX, rectTransform.anchoredPosition.y);

        yield return null;
    }

    private void ShowInventorySlots() {
        inventorySlotsParent.SetActive(true);
    }
    private void HideInventorySlots() {
        inventorySlotsParent.SetActive(false);
    }
    
}
