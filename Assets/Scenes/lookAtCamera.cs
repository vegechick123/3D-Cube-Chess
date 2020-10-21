using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// 始终面向摄像机
/// </summary>
public class LookAtCamera : MonoBehaviour
{
    public bool IsStopInSceneView = false;
    public bool reverse = false;
    void Update()
    {
        Rot(Camera.main.GetComponent<Camera>().transform);
    }

    void Rot(Transform target)
    {
        Plane plane = new Plane(target.forward, target.position);
        float dis;
        Vector3 tar = target.position;
        if (plane.Raycast(new Ray(this.transform.position, -target.forward), out dis) == true||reverse)
        {
            tar = this.transform.position + (-target.forward * dis);
        }
        this.transform.LookAt(tar, Vector3.up);
    }
}

