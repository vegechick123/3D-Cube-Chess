using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Messages :MonoBehaviour
{
    bool bReleased;
    bool isShowInfo;
    public Text title;
    public Text info;
    GameObject Message;
    IGetInfo information;
    public void Init(IGetInfo target)
    {
        title.text=target.GetTitle();
        info.text = target.GetInfo();
    }
    private void OnMouseEnter()
    {
        isShowInfo = true;
    }
    private void OnMouseExit()
    {
        isShowInfo = false;
    }
    public void Refresh()
    {

        if (isShowInfo)
            Message.SetActive(true);
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
