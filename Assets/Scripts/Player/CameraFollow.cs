using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform head;
    Vector3 offset;
    private void Start()
    {
        offset = transform.position- head.position  ;
    }
    void Update()
    {
        transform.position = head.position+ head.forward*0.3f+Vector3.up*0.2f;
    }
}
