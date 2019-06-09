using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float Speed = 15.0f;
    public float DistanceBehind = 5.0f;
    public float DistanceAbove = 2.0f;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        var newPosition = target.transform.position - (target.transform.forward * DistanceBehind) + (target.transform.up * DistanceAbove);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * Speed);
        transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
    }
}
