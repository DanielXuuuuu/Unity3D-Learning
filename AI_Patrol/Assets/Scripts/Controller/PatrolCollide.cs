using UnityEngine;
using System.Collections;

public class PatrolCollide : MonoBehaviour
{
    void OnCollisionStay(Collision other)
    {
        //当侦察兵与玩家相撞
        if (other.gameObject.name == "player")
        {
            other.gameObject.GetComponent<Animator>().SetBool("death", true);
            this.GetComponent<Animator>().SetBool("shoot", true);
            Singleton<GameEventManager>.Instance.PlayerArrested();
        }
    }
}