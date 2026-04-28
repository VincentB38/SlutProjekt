using TMPro;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    public GameObject lockIcon; // the lock icon
    public TextMeshProUGUI LevelText; // for the text

    private void Start()
    {
        LevelText.enabled = false; // disable the text in the start
    }
    public void Unlock()
    {
        if (lockIcon != null) 
            lockIcon.SetActive(false); // make it disabled (meaning the player unlocked this stage)
            LevelText.enabled = true; // show the level

    }
}