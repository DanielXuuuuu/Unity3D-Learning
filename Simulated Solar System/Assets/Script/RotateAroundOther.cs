using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundOther : MonoBehaviour
{
    public Transform sun;
    public int speed = 50;
    float ry,rz;
    // Start is called before the first frame update
    void Start()
    {
        ry = Random.Range(30, 60);
        rz = Random.Range(-20, 20);
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 axis = new Vector3(0, ry, rz);
        this.transform.RotateAround(sun.position, axis, speed * Time.deltaTime);
    }
}
