using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

public class OddDebug : EditorWindow
{
    [MenuItem("Window/Odd Jobs/Debug Window")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(OddDebug), false, "Odd Jobs Debug");
    }

    void OnGUI()
    {
        GUILayout.Label("Player Management", EditorStyles.boldLabel);

        if (GUILayout.Button("Add New Player"))
        {
            var localPlayerManager = GameObject.Find("LocalPlayerManager");
            if (localPlayerManager != null)
            {
                localPlayerManager.GetComponent<PlayerInputManager>().JoinPlayer();
            }
            else
            {
                Debug.LogError("No LocalPlayerManager found in scene. Please ensure one exists before adding players.");
            }
        }
    }
}
