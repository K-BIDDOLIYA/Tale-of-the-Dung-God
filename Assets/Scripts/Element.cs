using UnityEngine;

public class Element : MonoBehaviour
{
    private bool collected = false;
    private AudioSource soundEffect;

    private void Start()
    {
        soundEffect = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (ElementManager.Instance == null)
        {
            Debug.LogWarning("ElementManager is missing from the scene!");
            return;
        }
        soundEffect.Play(); 
        collected = true;

        ElementManager.Instance.CollectElement();

        Destroy(gameObject);
    }
}

