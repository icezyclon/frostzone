using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotScript : MonoBehaviour
{
    [SerializeField] public int ttl = 20;

    [SerializeField] public bool freeze = false;

    void Start()
    {
        Destroy(gameObject, ttl);
    }

    void FixedUpdate()
    {
        if (freeze)
        {
            transform.Rotate(5, 0, 0);
        }
        float distanceFromCamera = (transform.position - Camera.main.transform.position).z;
        float rightCameraEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceFromCamera)).x;
        if (transform.position.x > rightCameraEdge + 2)
        {
            Destroy(gameObject);
        }
    }
}
