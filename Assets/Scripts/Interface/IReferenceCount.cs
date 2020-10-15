using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReferenceCount
{
    void AddReference();
    void RemoveReference();
}
