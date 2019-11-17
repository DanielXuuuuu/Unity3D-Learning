using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    // UGUI的Slider对象
    public Slider mainSlider;

    // 血量
    private float currentBlood;
    private float resultBlood;

    // 碰撞检测
    private bool flag;

    // Start is called before the first frame update
    void Start()
    {
        // 初始血量
        mainSlider.value = 80;
        currentBlood = mainSlider.value;
        resultBlood = currentBlood;
    }

    // Update is called once per frame
    void Update()
    {
        // 如果发生碰撞，扣血
        if (flag)
        {
            resultBlood = mainSlider.value - 20.0f < 0.1f ? 0.0f : mainSlider.value - 20.0f;
            flag = false;
        }
        // 平滑减少血量
        currentBlood = Mathf.Lerp(currentBlood, resultBlood, 0.1f);
        mainSlider.value = currentBlood;
    }

    public void collision()
    {
        flag = true;
    }
}
