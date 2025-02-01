using UnityEngine;

public class Player_MenuHandler : MonoBehaviour
{
    public bool MenuOpen = false;
    [SerializeField] GameObject MenuCanvas;

    void Start()
    {
        MenuCanvas.SetActive(false);
    }
    public void OnMenuButtonClicked()
    {
        if(!MenuOpen)
        {
            MenuCanvas.SetActive(true);
            MenuOpen = true;
            Cursor.lockState = CursorLockMode.None;
            
        }
        else
        {
            MenuCanvas.SetActive(false);
            MenuOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void OnQuitClicked()
    {
        LobbyManager.Instance.LeaveLobby();
    }

    
}
