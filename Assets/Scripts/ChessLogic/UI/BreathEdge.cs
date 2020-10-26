using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathEdge : MonoBehaviour
{
    public GameObject prefabEdge;
    public GameObject target;
    private GameObject instansiateObject;
    private void OnEnable()
    {
        instansiateObject = Instantiate(prefabEdge, target.transform);
    }
    private void OnDisable()
    {
        Destroy(instansiateObject);
    }
}
