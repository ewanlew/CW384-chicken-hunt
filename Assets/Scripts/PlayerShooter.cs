using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    void Update()
    {
        if (Time.timeScale == 0f) return;
        
        if (Input.GetMouseButtonDown(0)){
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Chicken[] chickens = Object.FindObjectsByType<Chicken>(FindObjectsSortMode.None);
            bool hit = false;

            foreach (Chicken c in chickens){
                float dist = Vector2.Distance(c.transform.position, worldPos);

                if (dist <= c.killRadius * 2f){
                    c.TryHit(worldPos);
                    hit = true;
                    break;
                }
            }

            if (!hit){
                Debug.Log("missed everything...");
            }
        }
    }
}
