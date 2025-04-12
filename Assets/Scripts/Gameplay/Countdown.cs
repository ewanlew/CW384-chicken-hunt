using UnityEngine;
using TMPro;
using System.Collections;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject spawner;

    public int countdownTime = 3;

    void Start()
    {
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        int countdownTime = 3;
        while (countdownTime > 0)
        {
            countdownText.text = "Starting in " + countdownTime.ToString() + "...";
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        countdownText.text = "Begin! :D";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);

        spawner.SetActive(true); // Begin spawning
    }
}