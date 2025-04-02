using UnityEngine;

public class CursorTest : MonoBehaviour
{
    public Texture2D cursor;

    void Start()
    {
        Cursor.SetCursor(cursor, new Vector2(64, 64), CursorMode.ForceSoftware);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("Custom cursor set!");
    }
}
