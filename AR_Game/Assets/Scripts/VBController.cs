using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VBController : MonoBehaviour, IVirtualButtonEventHandler
{
    public GameObject dragon;
    public Vector3 target = new Vector3(-0.2f, 0, -0.2f);
    public float speed = 0.1f;
    public bool pressed = false;

    void Start()
    {
        //在所有子物体类中找到所有VirtualButtonBehaviour组件
        VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        //在虚拟按钮中注册TrackableBehaviour事件
        for (int i = 0; i < vbs.Length; ++i)
        {
            vbs[i].RegisterEventHandler(this);
        }

        dragon = transform.Find("dragon").gameObject;
    }
    
    void Update()
    {
        dragon.transform.localPosition = Vector3.MoveTowards(dragon.transform.localPosition, target, speed * Time.deltaTime);
        if (dragon.transform.localPosition == target)
        {
            target = new Vector3(-0.2f, 0, -0.2f);
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log(vb.VirtualButtonName + " btn pressed");
        if(pressed == false)
        {
            pressed = true;
            target = new Vector3(-0.2f, 0.4f, -0.2f);
        }
        
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        Debug.Log(vb.VirtualButtonName + " btn released");
        pressed = false;
    }
}