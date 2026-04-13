using UnityEngine;
using UnityEngine.SceneManagement; // Needed to load the game scene

public class MainMenuManager : MonoBehaviour
{
    // Make sure to spell your game scene's name exactly as it appears in your project!
    // Looking at your previous screenshots, your game scene is named "SampleScene".
    [SerializeField] private string gameSceneName = "SampleScene";
    public GameObject creditPanel;

    public void PlayGame()
    {
        // This resets time just in case you quit to the menu while the game was paused
        Time.timeScale = 1f;

        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        // Application.Quit() does not work inside the Unity Editor!
        // We add a Debug.Log so you can see that the button is actually working.
        Debug.Log("QUITING GAME... (This will close the app in a built game)");

        Application.Quit();
    }

    public void ViewCredits()
    {
        creditPanel.SetActive(true);
    }
     public void HideCredits()
    {
        creditPanel?.SetActive(false);
    }
}