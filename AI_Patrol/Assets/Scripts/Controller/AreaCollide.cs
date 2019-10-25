using UnityEngine;
using System.Collections;

public class AreaCollide : MonoBehaviour
{
    public int sign = 0;
    SceneController sceneController;
    private void Start()
    {
        sceneController = SSDirector.GetInstance().CurrentSceneController as SceneController;
    }
    void OnTriggerEnter(Collider collider)
    {
        //标记玩家进入自己的区域
        if (collider.gameObject.name == "player")
        {
            Debug.Log("player enter floor " + sign);
            sceneController.floorNumber = sign;
        }
    }
}