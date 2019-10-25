using UnityEngine;
using System.Collections;

public enum SSActionEventType : int { Started, Compeleted }

public interface ISSActionCallback
{
    void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Compeleted,
        int intParam = 0,
        string strParam = null,
        GameObject objectParam = null);
}