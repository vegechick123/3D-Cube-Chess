using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Messages :MonoBehaviour
{
    bool bReleased;
    public Text title;
    public Text info;
   // public  GameObject messageBoxObject;
    IGetInfo information;
    public void Init(IGetInfo target)
    {
        //GameObject.SetActive(false);
        //messageBoxObject = UIManager.instance.CreateMessage();
        title.text=target.GetTitle();
        info.text = target.GetInfo();
        //messageBoxObject
    }

}
