using TMPro;
using UnityEngine;

public class Local_PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
