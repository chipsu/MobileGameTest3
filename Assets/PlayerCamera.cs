using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float Speed = 0.25f;
    public float DistanceBehind = 5.0f;
    public float DistanceAbove = 1.0f;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        var newPosition = target.transform.position - target.transform.forward * DistanceBehind + (target.transform.up * DistanceAbove);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, Speed);
        transform.rotation = target.transform.rotation;
    }
}
