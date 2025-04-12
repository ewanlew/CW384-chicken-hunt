using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    void Update()
    {
        if (Time.timeScale == 0f) return; // don’t allow shooting while paused
        
        if (Input.GetMouseButtonDown(0)){ // left click
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // get click position in world

            Chicken[] chickens = Object.FindObjectsByType<Chicken>(FindObjectsSortMode.None); // get all chickens
            bool hit = false;

            foreach (Chicken c in chickens){
                float dist = Vector2.Distance(c.transform.position, worldPos); // check distance to each chicken

                if (dist <= c.killRadius * 2f){ // hit or near-hit range
                    c.TryHit(worldPos); // try to hit it
                    hit = true;
                    break;
                }
            }

            if (!hit){
                Debug.Log("missed everything..."); // nothing hit
            }
        }
    }
}
