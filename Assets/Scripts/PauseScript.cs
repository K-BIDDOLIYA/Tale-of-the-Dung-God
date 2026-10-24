using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
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
