using TMPro;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    public GameObject lockIcon; // the lock icon
    public TextMeshProUGUI LevelText; // for the text
    public void Unlock()
    {
        if (lockIcon != null)
        {
            lockIcon.SetActive(false); // make it disabled (meaning the player unlocked this stage)
            LevelText.enabled = true; // show the level
        }
    }
}