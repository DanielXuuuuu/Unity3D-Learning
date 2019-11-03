using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private OuterRing[] outerRing;
    private InnerRing[] innerRing;

    private void Start()
    {
        outerRing = GetComponentsInChildren<OuterRing>() as OuterRing[];
        innerRing = GetComponentsInChildren<InnerRing>() as InnerRing[];
    }
    private void OnGUI()
    {
        GUIStyle button_style;
        button_style = new GUIStyle("button")
        {
            fontSize = 15
        };
        if(GUI.Button(new Rect(Screen.width - 150, Screen.height - 100, 100, 30), "收", button_style))
        {
            outerRing[0].isCollected = true;
            innerRing[0].isCollected = true;
        }
        if(GUI.Button(new Rect(Screen.width - 150, Screen.height - 50, 100, 30), "散", button_style))
        {
            outerRing[0].isCollected = false;
            innerRing[0].isCollected = false;
        }
    }
}
