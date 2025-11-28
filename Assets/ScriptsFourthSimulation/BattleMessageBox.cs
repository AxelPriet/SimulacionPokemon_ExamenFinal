using System.Collections;
using UnityEngine;
using TMPro; // ← IMPORTANTE

public class BattleMessageBox : MonoBehaviour
{
    public TMP_Text messageText;   // ← Ahora es TMP_Text
    public float displayTime = 1.2f;
    Coroutine current;

    public void ShowMessage(string msg)
    {
        if (current != null)
            StopCoroutine(current);

        current = StartCoroutine(ShowCoroutine(msg));
    }

    IEnumerator ShowCoroutine(string msg)
    {
        messageText.text = msg;
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        messageText.gameObject.SetActive(false);

        current = null;
    }

    public bool IsShowing()
    {
        return current != null;
    }
}

