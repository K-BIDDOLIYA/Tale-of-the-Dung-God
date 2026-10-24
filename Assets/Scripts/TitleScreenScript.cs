using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenScript : MonoBehaviour
{
  public void StartScreen()
  {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

  public void Settings()
  {
      GameObject.Find("PauseScreen").GetComponent<Canvas>().enabled = true;
  }
  
  public void QuitGame()
  {
    Application.Quit();
  }
}
