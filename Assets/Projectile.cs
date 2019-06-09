using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed = 10.0f;
    public GameObject Target;
    Transform particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = transform.Find("Particles");
    }

    // Update is called once per frame
    void Update()
    {
        if (Target)
        {
            if (transform.position == Target.transform.position)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Time.deltaTime * Speed);
                particles.rotation = Quaternion.LookRotation(transform.position - Target.transform.position);
            }
        }
    }
}
