using UnityEngine;
using System.Collections;

public class ActionManagerAdapter : MonoBehaviour, IActionManager
{
    public CCActionManager CCAction;
    public PhysisActionManager PhysisAction;

    public void playDisk(GameObject disk, float angle, float power, bool isPhysis)
    {
        if (!isPhysis)
        {
            CCAction.playDisk(disk, angle, power);
        }
        else
        {
            PhysisAction.playDisk(disk, angle, power);
        }
    }

    void Start()
    {
        CCAction = gameObject.AddComponent<CCActionManager>() as CCActionManager;
        PhysisAction = gameObject.AddComponent<PhysisActionManager>() as PhysisActionManager;
    }
}
