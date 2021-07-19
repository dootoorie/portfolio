using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void Init()
    {
        //sorting�� �ʿ��Ͽ�, SetCanvas�Լ��� �����Ͽ� true�� ������ Sorting ��û, 2021-07-18
        Managers.UI.SetCanvas(gameObject, true);
    }

    //UI_Popup�� ��ӹ��� �ֵ��� ���� ClosePopupUI()�� �ϸ� �ڵ����� UI�� �ݴ´�, 2021-07-18
    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}