using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour
{
    private static FloatingText popupText;
    static GameObject canvas;

    public static void InitializeFloatingText()
    {
        canvas = GameObject.Find("Canvas");
        if (!popupText)
        {
            popupText = Resources.Load<FloatingText>("Prefabs/PopupTextParent");
        }
    }
    public static void CreateFloatingText(string text, Transform location)
    {
        FloatingText instance = Instantiate(popupText);

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(location.position);

        instance.transform.SetParent(canvas.transform, false);

        instance.transform.position = screenPosition;

        instance.SetText(text);
    }
}
