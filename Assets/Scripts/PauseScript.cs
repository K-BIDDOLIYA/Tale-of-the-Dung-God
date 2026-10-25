using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public void PauseGame()
    {
        Time.timeScale = 0f;
        GameObject.Find("PauseScreen").GetComponent<Canvas>().enabled = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        GameObject.Find("PauseScreen").GetComponent<Canvas>().enabled = false;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
