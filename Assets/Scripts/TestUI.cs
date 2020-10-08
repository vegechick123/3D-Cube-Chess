using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        new HealthBar(GetComponent<GChess>());
    }

    // Update is called once per frame
}
