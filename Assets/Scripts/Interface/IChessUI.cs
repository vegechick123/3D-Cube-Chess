using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChessUI
{
    void Refresh();
    void Release();

    void Hide();
    void Show();
}
