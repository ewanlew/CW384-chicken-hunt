using UnityEngine;

public class Chicken : MonoBehaviour
{

    public float killRadius = 0.5f;

    public void TryHit(Vector2 clickPosition) { 
        float distance = Vector2.Distance(transform.position, clickPosition);

        if (distance <= killRadius) {
            GameManager.Instance.AddScore(1);
            Debug.Log("hit!");
        } else {
            GameManager.Instance.Miss();
            Debug.Log("close but missed...");
        }

        Destroy(gameObject);
    }

}
