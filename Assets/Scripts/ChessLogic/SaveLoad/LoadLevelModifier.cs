using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LoadLevelModifier : MonoBehaviour
{
    public abstract void OnNextLevel(SaveData data);
}
