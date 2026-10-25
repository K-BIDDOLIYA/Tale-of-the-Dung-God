using UnityEngine;

public class Element : MonoBehaviour
{
    private bool collected = false;

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

        collected = true;

        ElementManager.Instance.CollectElement();

        Destroy(gameObject);
    }
}

