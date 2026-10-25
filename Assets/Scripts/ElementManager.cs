using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ElementManager : MonoBehaviour
{
    public static ElementManager Instance { get; private set; }

    [Header("Element Collection")]
    [SerializeField] private int totalElements = 3;
    private int collectedElements = 0;

    [Header("UI")]
    [SerializeField] private TMP_Text elementText;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        collectedElements = 0;
        UpdateElementText();

    }

    public void CollectElement()
    {
        if (collectedElements >= totalElements)
            return;

        collectedElements++;

        UpdateElementText();

        if (Audio.Instance != null)
            Audio.Instance.PlayCollect();

        if (collectedElements >= totalElements)
            UnlockPortal();
    }

    private void UpdateElementText()
    {
        if (elementText != null)
            elementText.text = collectedElements + "/" + totalElements + " Elements";
    }

    private void UnlockPortal()
    {
        SceneManager.LoadScene("TitleScreen");
        Debug.Log("All elements collected! Portal unlocked.");
    }

    public int GetCollectedElements()
    {
        return collectedElements;
    }
}


