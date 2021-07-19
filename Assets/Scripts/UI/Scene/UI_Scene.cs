using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    public override void Init()
    {
        //sorting이 필요하여, SetCanvas함수를 실행하여 true로 설정해 Sorting 요청
        Managers.UI.SetCanvas(gameObject, false);
    }
}