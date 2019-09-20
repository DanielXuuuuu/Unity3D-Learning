using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame

    void Update()
    {
    }


    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 70, 50), "停止"))
        {
            Time.timeScale = 0;
        }
        if (GUI.Button(new Rect(70, 0, 70, 50), "开始"))
        {
            Time.timeScale = 1;
        }
    }
}
