using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class FloatText : MonoBehaviour
{
   
    private void DestroySelf()
    {
        Destroy(transform.parent.gameObject);
    }

}
