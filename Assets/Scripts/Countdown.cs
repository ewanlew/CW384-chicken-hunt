using UnityEngine;
using TMPro;
using System.Collections;

public class Countdown : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    // public GameObject spawner;

    void Start()
    {
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        int count = 3;
        while (count > 0)
        {
            countdownText.text = "Starting in " + count.ToString() + "...";
            yield return new WaitForSeconds(1f);
            count--;
        }

        countdownText.text = "Begin! :D";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);

        // spawner.SetActive(true); // Begin spawning
    }
}