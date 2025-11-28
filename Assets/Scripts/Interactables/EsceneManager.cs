using UnityEngine;
using UnityEngine.SceneManagement;

public class EsceneManager : MonoBehaviour
{
    [Header("Escena a cargar")]
    [Tooltip("Índice de la escena en Build Settings")]
    public int sceneIndex;

    private bool playerInside = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
