using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    public override void Init()
    {
        //sorting�� �ʿ��Ͽ�, SetCanvas�Լ��� �����Ͽ� true�� ������ Sorting ��û
        Managers.UI.SetCanvas(gameObject, false);
    }
}