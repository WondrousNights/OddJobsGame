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
        if (GUILayout.Button("Play from Lobby"))
        {
            string scenePath = "Assets/_OddJobs/Scenes/Network_Scenes/Lobby.unity";
            if (System.IO.File.Exists(scenePath))
            {
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath);
                EditorApplication.isPlaying = true;
            }
            else
            {
                Debug.LogError("Test scene not found at: " + scenePath);
            }
        }
        
        // if (GUILayout.Button("Add New Player"))
        // {
        //     var localPlayerManager = GameObject.Find("LocalPlayerManager");
        //     if (localPlayerManager != null)
        //     {
        //         localPlayerManager.GetComponent<PlayerInputManager>().JoinPlayer();
        //     }
        //     else
        //     {
        //         Debug.LogError("No LocalPlayerManager found in scene. Please ensure one exists before adding players.");
        //     }
        // }
    }
}
