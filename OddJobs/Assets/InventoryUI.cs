using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private bool debug = false;
    [SerializeField] private float spacing = 100f;
    [SerializeField] private PlayerGunHandler playerGunHandler;
    // [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private GameObject[] itemSlots;

    // private void Awake() {
    //     itemSlots = new GameObject[playerGunHandler.Inventory.Length];
    //     for (int i = 0; i < playerGunHandler.Inventory.Length; i++) {
    //         itemSlots[i] = Instantiate(itemSlotPrefab, transform);
    //     }
    // }

    public void UpdateInventoryUI() {
        if (debug) Debug.Log("Updating inventory UI");

        for (int i = 0; i < itemSlots.Length; i++) {
            TMPro.TextMeshProUGUI slotText = itemSlots[i].GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (slotText) {
                // if theres a gun in the slot, set the name in the itemslot
                if (playerGunHandler.Inventory[i]) {
                    itemSlots[i].SetActive(true);
                    slotText.text = playerGunHandler.Inventory[i].GetComponent<GunScriptableObject>().name;
                } else {
                    // if there's no gun in the slot, hide the item slot
                    itemSlots[i].SetActive(false);
                }
            }
        }

        StartCoroutine(LerpInventoryPosition());
    }

    private IEnumerator LerpInventoryPosition()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        float startX = rectTransform.anchoredPosition.x;
        float targetX = -spacing * playerGunHandler.currentGunIndex;
        float elapsedTime = 0f;
        float lerpDuration = 0.2f;

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

    
}
