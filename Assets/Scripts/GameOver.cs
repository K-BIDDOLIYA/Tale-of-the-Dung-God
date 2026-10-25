using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene("FirstLevel");
    }

    public void Quit()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
