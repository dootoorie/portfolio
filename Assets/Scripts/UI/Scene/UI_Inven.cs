using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2021-07-19

public class UI_Inven : UI_Scene
{
    enum GameObjects
    {
        GridPanel
    }
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        //GameObject타입을 바인딩
        Bind<GameObject>(typeof(GameObjects));

        //목적 : 인벤토리 아이콘 여러개(12개 정도) 만들었던 것을 지우자
        //기능 : 하이어라키 창에서의 GridPanel 게임 오브젝트를 변수 gridPanel에 넣어준다.        
        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);

        //목적 : 내(=하이어라키창의 GridPanel)가 가지고 있는 자식들을 모두 삭제
        //기능 : 내 transform을 들고 있는 모든 자식들을 순회
        foreach (Transform child in gridPanel.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        //실제 인벤토리 정보를 참고해서
        for (int i = 0; i < 8; i++)
        {
            //프리팹 생성 (위치 : UI폴더/Scene폴더/UI_Inven_Item 프리팹)
            GameObject item = Managers.Resource.Instantiate("UI/Scene/UI_Inven_Item");

            //연결(= 부모님 지정)
            item.transform.SetParent(gridPanel.transform);

            //UI_Inven_Item.cs 컴포넌트를 UI_Inven_Item 프리팹에 붙이기, 붙인 다음, 인스턴스화한 invenItem변수에 붙이기
            UI_Inven_Item invenItem = Util.GetOrAddComponent<UI_Inven_Item>(item);

            //UI_Inven_Item.cs의 SetInfo함수를 이용하여 Text 내용 적기(= 아이템 이름)
            invenItem.SetInfo($"집행검{i}번");
        }
    }
}
