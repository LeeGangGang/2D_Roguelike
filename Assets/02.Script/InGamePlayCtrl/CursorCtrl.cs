using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCtrl : MonoBehaviour
{
    [SerializeField] Texture2D CursorImg;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(CursorImg, new Vector2(CursorImg.width / 2, CursorImg.height / 2), CursorMode.Auto);
    }
}
