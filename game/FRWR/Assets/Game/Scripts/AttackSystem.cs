using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    public Texture2D cursorTex;
    public CursorMode cursorMode = CursorMode.Auto;

    // Start is called before the first frame update
    void Start()
    {
        // Cursor.SetCursor(cursorTex, Vector2.zero, cursorMode);
    }
}
