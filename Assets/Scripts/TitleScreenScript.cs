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
      //TODO: Write a settings thing.
  }
  
  public void QuitGame()
  {
    Application.Quit();
  }
}
