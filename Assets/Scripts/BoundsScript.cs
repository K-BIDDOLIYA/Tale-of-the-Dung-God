using UnityEngine;
using UnityEngine.SceneManagement;

public class BoundsScript : MonoBehaviour
{
  void onCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
  } 
}
