using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GuildPrestigeManager : NetworkBehaviour
{
    public int guildPrestige;

    [SerializeField] private TMP_Text guildPrestigeText;

    void Start()
    {
        if(IsHost)
        {
            
            

            //waterCount += PlayerPrefs.GetInt("WaterFromExtraction");
            //PlayerPrefs.SetInt("WaterFromExtraction", 0);
            //waterCount -= 1;

            //PlayerPrefs.SetInt("WaterCount", waterCount);

            guildPrestige = PlayerPrefs.GetInt("GuildPrestige");

            SetWaterCountTextRpc(0);
            
        }
    }

    [Rpc(SendTo.Everyone)]
    void SetWaterCountTextRpc(int count)
    {
        guildPrestigeText.text = "Guild Presitge " + count;
    }
}
