using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMGUI : MonoBehaviour
{
    public float blood;
    private float resultBlood;

    private Rect bloodBar;      // 血条
    private Rect addButton;     // 加血按钮
    private Rect reduceButton;  // 扣血按钮

    // Start is called before the first frame update
    void Start()
    {
        // 初始值
        blood = 50;
        resultBlood = 50;

        // 固定位置
        bloodBar = new Rect(Screen.width / 2 -100, Screen.height / 4 - 10, 200, 20);
        reduceButton = new Rect(Screen.width / 2 - 160, Screen.height / 4 - 10, 40, 20);
        addButton = new Rect(Screen.width / 2 + 120, Screen.height / 4 - 10, 40, 20);
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    private void OnGUI()
    {
        if(GUI.Button(addButton, "+"))
        {
            resultBlood = resultBlood + 10.0f > 100.0f ? 100.0f : resultBlood + 10.0f;
        }
        if(GUI.Button(reduceButton, "-"))
        {
            resultBlood = resultBlood - 10.0f < 0.1f ? 0.0f : resultBlood - 10.0f;
        }

        GUI.color = new Color(255f / 255f, 0f / 255f, 0f / 255f, 1f);

        //插值计算HP值
        blood = Mathf.Lerp(blood, resultBlood, 0.05f);

        GUI.HorizontalScrollbar(bloodBar, 0.0f, blood, 0.0f, 100.0f);
    }
}
