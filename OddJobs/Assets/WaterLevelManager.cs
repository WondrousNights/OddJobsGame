using TMPro;
using Unity.Netcode;
using UnityEngine;

public class WaterLevelManager : NetworkBehaviour
{
    public int waterCount;

    [SerializeField] private TMP_Text waterCountText;

    void Start()
    {
        if(IsHost)
        {
            waterCount = PlayerPrefs.GetInt("WaterCount");

            waterCount += PlayerPrefs.GetInt("WaterFromExtraction");
            PlayerPrefs.SetInt("WaterFromExtraction", 0);
            waterCount -= 1;

            PlayerPrefs.SetInt("WaterCount", waterCount);
            SetWaterCountTextRpc(waterCount);
        }
    }

    [Rpc(SendTo.Everyone)]
    void SetWaterCountTextRpc(int count)
    {
        waterCountText.text = "Water Count: " + count;
    }
}
