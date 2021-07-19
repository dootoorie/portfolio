using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void Init()
    {
        //sorting이 필요하여, SetCanvas함수를 실행하여 true로 설정해 Sorting 요청, 2021-07-18
        Managers.UI.SetCanvas(gameObject, true);
    }

    //UI_Popup을 상속받은 애들은 이제 ClosePopupUI()를 하면 자동으로 UI를 닫는다, 2021-07-18
    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}