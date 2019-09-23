using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CCSequenceAction : SSAction, ISSActionCallback
{
    public List<SSAction> sequence;
    public int repeat = -1;
    public int currentIndex = 0;

    public static CCSequenceAction GetSSAction(int repeat, int currentIndex, List<SSAction> sequence)
    {
        CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
        action.repeat = repeat;
        action.currentIndex = currentIndex;
        action.sequence = sequence;
        return action;
    }

    // 执行动作前，为每个动作注入当前动作游戏对象，并将自己作为动作事件的接收者
    public override void Start()
    {
        foreach(SSAction action in sequence)
        {
            action.gameObject = this.gameObject;
            action.transform = this.transform;
            action.callback = this;
            action.Start();
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (sequence.Count == 0)
            return;
        if(currentIndex < sequence.Count)
        {
            sequence[currentIndex].Update();
        }
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Compeleted, int intParam = 0, string strParam = null, Object objectParam = null)
    { 
        source.destory = false;
        this.currentIndex++;
        if(this.currentIndex >= sequence.Count)
        {
            this.currentIndex = 0;
            if (repeat > 0)
                repeat--;
            if(repeat == 0)
            {
                this.destory = true;
                this.callback.SSActionEvent(this);
            }
        }
    }

    private void OnDestroy()
    {
        foreach(SSAction action in sequence)
        {
            Object.Destroy(action);
        }
    }
}
