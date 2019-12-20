using UnityEngine;
using System.Collections;

public class birdGenerator : MonoBehaviour
{
    private GameObject bird;
    // Use this for initialization
    void Start()
    {
        bird = Object.Instantiate(Resources.Load<GameObject>("bird"),
                    Vector3.zero, Quaternion.identity, null);
        bird.transform.parent = this.transform;
        bird.transform.localPosition = new Vector3(-0.2f, 0.1f, 0.5f);
        bird.transform.localScale *= 0.07f;
        bird.transform.Rotate(0, 180, 0, Space.Self); 
    }

    // Update is called once per frame
    void Update()
    {
        bird.transform.localPosition -= new Vector3(0, 0, 0.015f);
        if (bird.transform.localPosition.z < -0.4)
        {
            bird.transform.localPosition = new Vector3(-0.2f, 0, 0.5f);
        }   
    }
}
