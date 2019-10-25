using UnityEngine;
using System.Collections;

public class CoinCollide : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        //玩家吃到金币事件触发
        if (collider.gameObject.name == "player")
        {
            this.gameObject.SetActive(false);
            Singleton<GameEventManager>.Instance.RecudeCoinNum();
        }
    }
}