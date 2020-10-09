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

    public void Refresh()
    {
       
    }
    public void Release()
    {
        bReleased = true;
        UIManager.instance.eRefreshFloorHUD.RemoveListener(Refresh);
        //GameObject.Destroy(Message);
        //Message2.DestoryAll();
       
    }
    public void Hide()
    {
    }
    public void Show()
    {

    }

}
