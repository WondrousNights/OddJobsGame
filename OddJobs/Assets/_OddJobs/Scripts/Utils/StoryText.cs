using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class StoryText : MonoBehaviour
{
    [SerializeField] private float startDelay = 5f;
    [SerializeField] private string startText = "";
    public AudioClip blip;

    private TMP_Text tmp;
    private AudioSource sound;
    [SerializeField] private float letterTimeDefault = 0.04f;
    private bool skip = false;
    private string initialText;
    
    private void Start()
    {
        tmp = GetComponent<TMP_Text>();
        if (!sound) sound = GetComponent<AudioSource>();

        if (startDelay >= 0) 
        {
            string textToShow = "";
            if (startText == "")
            {
                textToShow = tmp.text;
            }
            else
            {
                textToShow = startText;
            }
            tmp.text = "";
            DoStoryText(textToShow, startDelay);
        }
        tmp.text = "";
    }

    public void DoStoryText(string text, float delayTime = 0f, float delayChars = -1f, bool bypassPlayState = false)
    {
        if (delayChars == -1) delayChars = letterTimeDefault;
        StartCoroutine(IDoStoryText(text, delayTime, delayChars, bypassPlayState));
    }

    private IEnumerator IDoStoryText(string text, float delayTime, float delayChars, bool bypassPlayState)
    {
        yield return new WaitForSeconds(delayTime);

        // Don't show any more story text if the game is now playing
        // if (master.state == "playing" && !bypassPlayState) yield break;
        
        List<char> separatedList = new List<char>(text.ToCharArray());
        foreach (char character in separatedList)
        {
            tmp.text = tmp.text + character;
            if (!skip) sound.PlayOneShot(blip);
            if (!skip) yield return new WaitForSeconds(delayChars);
        }
        tmp.text = tmp.text + System.Environment.NewLine;

        skip = false;
    }

    public void ResetText()
    {
        tmp.text = "";
    }

    public void Skip()
    {
        skip = true;
    }
}
