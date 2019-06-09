using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        transform.Rotate(Vector3.right * 50.0f * Time.deltaTime);
    }
}
