using UnityEngine;

public class HideAtStart : MonoBehaviour
{
    private void Start() {
        gameObject.SetActive(false);
    }
}
