using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D customCursor;
    public Vector2 hotspot = new Vector2(64, 64);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(customCursor, hotspot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
