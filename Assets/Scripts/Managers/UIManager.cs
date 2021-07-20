using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager //MonoBehaviour 지워준다
{
    //UIManager 만든 이유 : UI_Button의 Sort Order 관리를 위해, 2021-07-18

    int order = 10;

    //어떤 자료구조로 들고있으면 좋을까? -> Stack 구조
    //이유 : 가장 마지막에 띄워진 팝업이 먼저 삭제되야 하니까 -> Stack으로 관리하자

    //GameObject 대신 컴포넌트를 가져오는 것이 훨씬 깔끔하다.
    //UI_Button은 UI_Popup을 상속받고 있으니 UI_Popup을 쓰자.
    Stack<UI_Popup> popupStack = new Stack<UI_Popup>();

    UI_Scene _sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");

            //Find를 했는데 @UI_Root를 못찾아서, root가 null이면, 2021-07-18
            if (root == null)
            {
                //하이어라키창에서 우클릭하여 Create 해주던 과정을 코드로 실행하여 게임오브젝트를 만들어주면 된다.
                //빈 게임오브젝트에 이름이 @UI_Root 인 것을 만들자., 2021-07-18
                root = new GameObject { name = "@UI_Root" };
            }

            //만약 찾았으면 root를 리턴
            return root;
        }
    }

    //SetCanvas 함수의 역할 : 외부에서 팝업 같은 UI가 켜질 때,
    //역으로 UIManager한테 SetCanvas를 요청해서 자기 Canvas에 있는 order를 채워달라고 부탁
    public void SetCanvas(GameObject go, bool sort = true)
    {
        //Util에 가서 Canvas 컴포넌트를 추출
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        //canvas안에 canvas가 중첩해서 있을 때, 부모가 어떤 값을 가지던, 나는 무조건 내 sortingOrder를 가진다는 의미
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = order;
            
            order++;
        }

        //sorting을 요청 안했다는 것은, 팝업과 연관 없는 일반UI
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    //아이템 만들기, 2021-07-19
    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        //만약 name을 받지 않았다면,
        if (string.IsNullOrEmpty(name))
        {
            //<T>와 똑같은 이름을 사용하겠다는 뜻
            name = typeof(T).Name;
        }

        //프리팹 만들기 : UI폴더/Popup폴더/
        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
        {
            go.transform.SetParent(parent);
        }

        return Util.GetOrAddComponent<T>(go);
    }

    //씬을 여는 함수
    //string name : 프리팹과 연관 -> Prefabs폴더/UI폴더/Popup폴더/UI_Button(프리팹파일 이름)을 건내주는 것임
    //public T : 스크립트와 연관 -> Scripts폴더/UI폴더/Popup폴더/UI_Button(스크립트파일 이름)을 건내주는 것임
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene  //T는 아무나 받는것이 아니라, UI_Scene을 상속받는 애
    {
        //만약 name을 받지 않았다면,
        if (string.IsNullOrEmpty(name))
        {
            //<T>와 똑같은 이름을 사용하겠다는 뜻
            name = typeof(T).Name;
        }

        //프리팹 만들기 : UI폴더/Popup폴더/
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");

        //게임 오브젝트를 보면 각각 중요한 기능들은 컴포넌트로 물고있다.        
        //프리팹 만들었으니(게임오브젝트), 기능을 부여 할려면, 컴포넌트도 물어줘야한다.
        //컴포넌트를 빼오는 방법은, Util.cs에다가 기능을 하나 넣어놓았다.
        T sceneUI = Util.GetOrAddComponent<T>(go);

        _sceneUI = sceneUI;                

        //go의 위치(= 하이어라키창의 UI 게임오브젝트의 위치)를 root의 위치로 정한다(부모님으로 설정하여), 2021-07-18
        go.transform.SetParent(Root.transform);

        return sceneUI;
    }



    //팝업을 여는 함수
    //string name : 프리팹과 연관 -> Prefabs폴더/UI폴더/Popup폴더/UI_Button(프리팹파일 이름)을 건내주는 것임
    //public T : 스크립트와 연관 -> Scripts폴더/UI폴더/Popup폴더/UI_Button(스크립트파일 이름)을 건내주는 것임
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup  //T는 아무나 받는것이 아니라, UI_Popup을 상속받는 애
    {
        //만약 name을 받지 않았다면,
        if(string.IsNullOrEmpty(name))
        {
            //<T>와 똑같은 이름을 사용하겠다는 뜻
            name = typeof(T).Name;
        }

        //프리팹 만들기 : UI폴더/Popup폴더/
        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");

        //게임 오브젝트를 보면 각각 중요한 기능들은 컴포넌트로 물고있다.        
        //프리팹 만들었으니(게임오브젝트), 기능을 부여 할려면, 컴포넌트도 물어줘야한다.
        //컴포넌트를 빼오는 방법은, Util.cs에다가 기능을 하나 넣어놓았다.
        T popup = Util.GetOrAddComponent<T>(go);

        popupStack.Push(popup);

        //여기서 order++ 하면 안되는 이유 : 하이어라키에 UI_Button 게임 오브젝트를 드래그 앤 드롭으로 만들어놓은 시작할때부터 만들어놓은 경우 처리가 안되는 문제가 있음 
        //order++;
                

        //go의 위치(= 하이어라키창의 UI 게임오브젝트의 위치)를 root의 위치로 정한다(부모님으로 설정하여), 2021-07-18
        go.transform.SetParent(Root.transform);

        return popup;
    }

    //팝업을 닫는 함수(안전 버전)
    //UI를 닫을 때, 마지막에 생성된 UI를 닫는 것이 아닌 중간부분 UI를 끌 수도 있는것을 미연에 방지하기 위해
    public void ClosePopupUI(UI_Popup popup)
    {
        //없으면
        if (popupStack.Count == 0)
        {
            //아무것도 안 한다.
            return;
        }

        //삭제가 되야 하는거면,
        //Peek() : 스택의 가장 위에 있는 항목을 반환 
        if (popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            
            return;
        }

        ClosePopupUI();
    }

    //팝업을 닫는 함수(일반 버전)
    public void ClosePopupUI()
    {
        //stack을 건드릴 때는, 항상 항상 Count를 체크하는 것을 습관화 하자
        //stack에 아무것도 없으면
        if (popupStack.Count == 0)
        {
            return;
        }

        //만일 stack에 하나라도 있으면, Pop() 해서 popup컴포넌트에 넣기
        UI_Popup popup = popupStack.Pop();

        //뽑아왔으면 지워줘야 하기 때문에 Destroy
        //Destroy(popup.gameObject) : popup 컴포넌트를 물고있는 게임 오브젝트를 삭제
        Managers.Resource.Destroy(popup.gameObject);

        //Destroy한 다음 더 이상 popup에 접근하면 안됨. 그러므로 null을 넣어주자.
        popup = null;

        order--;
    }

    //모든 UI를 닫는 함수
    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }
}
