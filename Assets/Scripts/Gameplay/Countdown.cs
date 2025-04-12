using UnityEngine;
using TMPro;
using System.Collections;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText; // text shown on screen
    [SerializeField] private GameObject spawner; // object that spawns chickens

    public int countdownTime = 3; // start value shown before game begins

    void Start()
    {
        StartCoroutine(CountdownRoutine()); // begin countdown on load
    }

    IEnumerator CountdownRoutine()
    {
        countdownTime = 3;
        while (countdownTime > 0)
        {
            countdownText.text = "Starting in " + countdownTime.ToString() + "..."; // update UI
            yield return new WaitForSeconds(1f); // wait a second
            countdownTime--; // tick down
        }

        countdownText.text = "Begin! :D"; // flash start message
        yield return new WaitForSeconds(1f); // hold for a moment
        countdownText.gameObject.SetActive(false); // hide text

        spawner.SetActive(true); // begin spawning
    }
}
