using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;
    void Start()
    {
        Instance = this;
    }

    public void BackToLevelMenu() 
    {
        SceneManager.LoadScene(0); // open level menu
    }

    public void PickLevel(GameObject Button)
    {
        try
        {
            int levelSelected = int.Parse(Button.name);

            if (levelSelected <= PlayerPrefs.GetInt("MaxLevel", 0)) // making sure it's a level that the player actually unlocked, 0 is just a safe fall back
            {
                PlayerPrefs.GetInt("PlayerLevel", 0); // set the level 
                SceneManager.LoadScene(1); // load the Main Game scene
            }
        }
        catch (System.FormatException Message) // if for some reason the button isn't a number
        {
            Debug.Log(Message);
        }
    }
}
