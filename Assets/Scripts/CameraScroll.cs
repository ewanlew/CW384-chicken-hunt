using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public float scrollSpeed = 5f;  // scroll speed
    public float maxOffset = 3f;    // maximum movement from start pos

    private float baseX;  // init x pos

    void Start()
    {
        baseX = transform.position.x;  // store the starting x position
    }

    void Update()
    {
        float screenWidth = Screen.width;
        float mouseX = Input.mousePosition.x;

        float scrollDirection = 0f;

        // if mouse is near the left edge (10% of screen)
        if (mouseX < screenWidth * 0.1f) 
            scrollDirection = -1f;
        // if mouse is near the right edge (90% of screen)
        else if (mouseX > screenWidth * 0.9f) 
            scrollDirection = 1f;

        // calculate new X position with clamping
        float newX = Mathf.Clamp(transform.position.x + scrollDirection * scrollSpeed * Time.deltaTime, baseX - maxOffset, baseX + maxOffset);

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
