using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterRing : MonoBehaviour
{
    public ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particles;
    private int particleNum = 4500;
    private float[] particleAngle;
    private float[] particleRadius;
    private float[] nonCollectRadius;
    private float[] collectRadius;
    private float minRadius = 3.0f, maxRadius = 4.8f;

    public float speed = 0.4f;
    public float collectSpeed = 2.5f;
    public bool isCollected = false;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = this.GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[particleNum];
        particleAngle = new float[particleNum];
        particleRadius = new float[particleNum];
        nonCollectRadius = new float[particleNum];
        collectRadius = new float[particleNum];
        particleSystem.maxParticles = particleNum;
        particleSystem.Emit(particleNum);
        particleSystem.GetParticles(particles);
        for(int i = 0; i < particleNum; i++)
        {
            float midR = (maxRadius + minRadius) / 2;
            //最小半径随机扩大
            float rate1 = Random.Range(1.0f, midR / minRadius);
            //最大半径随机缩小
            float rate2 = Random.Range(midR / maxRadius, 1.0f);
            float radius = Random.Range(minRadius * rate1, maxRadius * rate2);

            float angle = Random.Range(0.0f, 360.0f);
            float rad = angle / 180 * Mathf.PI;
            particles[i].position = new Vector3(radius * Mathf.Cos(rad), radius * Mathf.Sin(rad), 0.0f);

            particleAngle[i] = angle;
            particleRadius[i] = radius;

            nonCollectRadius[i] = radius;
            collectRadius[i] = radius - 1.5f * (radius / minRadius);
        }
        particleSystem.SetParticles(particles, particleNum);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < particleNum; i++)
        {
            if (isCollected)
            {
                if (particleRadius[i] > collectRadius[i])
                {
                    particleRadius[i] -= collectSpeed * (particleRadius[i] / collectRadius[i]) * Time.deltaTime;
                }
            }
            else
            {
                if (particleRadius[i] < nonCollectRadius[i])
                {
                    particleRadius[i] += collectSpeed * (nonCollectRadius[i] / particleRadius[i]) * Time.deltaTime;
                }
                else if (particleRadius[i] > nonCollectRadius[i])
                {
                    particleRadius[i] = particleRadius[i];
                }
            }
            particleAngle[i] -= Random.Range(0, speed);
            float rad = particleAngle[i] / 180 * Mathf.PI;
            particles[i].position = new Vector3(particleRadius[i] * Mathf.Cos(rad), particleRadius[i] * Mathf.Sin(rad), 0.0f);
        }
        particleSystem.SetParticles(particles, particleNum);
    }
}
