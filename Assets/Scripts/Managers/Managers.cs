using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    //static이니 s를 붙여준 것, 2021-06-30
    static Managers s_instance;

    //Managers.cs를 Instance라는 이름으로 불러온다
    //Instance를 외부에서 접근하기를 원치 않으니까 public 삭제, 2021-06-30
    static Managers Instance 
    { 
        get 
        { 
            Init(); 
            
            return s_instance; 
        } 
    }

    //새로 만든 InpuManager.cs 추가하여 인스턴스화, 2021-06-30
    InputManager input = new InputManager();

    //새로 만든 Resourcemanager.cs 추가하여 인스턴스화, 2021-06-30
    ResourceManager resource = new ResourceManager();

    //새로 만든 UIManager.cs 추가하여 인스턴스화, 2021-07-18
    UIManager ui = new UIManager();

    //InputManager.cs를 Input이라는 이름으로 불러온다
    //이제부터 실제 InputManager.cs를 사용하고 싶으면, Managers.Input을 통해 불러오면 된다., 2021-06-30
    public static InputManager Input
    { 
        get 
        { 
            return Instance.input; 
        }
    }

    //ResourceManager.cs를 Resource라는 이름으로 불러온다.
    //이제부터 실제 ResourceManager.cs를 사용하고 싶으면, Managers.Resource를 통해 불러오면 된다., 2021-06-30
    public static ResourceManager Resource 
    { 
        get 
        { 
            return Instance.resource; 
        } 
    }

    //UIManager.cs를 UI라는 이름으로 불러온다.
    //이제부터 실제 UIManager.cs를 사용하고 싶으면, Managers.UI를 통해 불러오면 된다, 2021-07-18
    public static UIManager UI 
    { 
        get 
        { 
            return Instance.ui;
        } 
    }


    void Start()
    {
        //이 밑줄부터는 발자취5 부분으로 이동 시킴
        //GameObject gameobject : GameObject를 변수명 gameobject로 인스턴스화 한 후,
        //GameObject.Find("@Managers") : 게임 오브젝트 찾기(@Manager란 이름을 가진)
        //= :  변수명 gameobject로 인스턴스화 한 코드에 넣기, 2021-06-30
        GameObject gameobject = GameObject.Find("@Managers");

        //"@Managers"란 게임오브젝트에 부착된 컴포넌트인 Managers스크립트를 가져오기 
        //Managers mg : Managers 스크립트의 Managers클래스를 변수명 mg로 인스턴스화 함
        //gameobject.GetComponent<Managers>() : 변수명 gameobject에 부착된 컴포넌트인 Managers스크립트를 가져오기 
        //= : 오른쪽 코드를 왼쪽 코드에 넣기
        //Instance = gameobject.GetComponent<Managers>();        
        
        //, 2021 - 06 - 30
        Init();        
    }

    void Update()
    {
        //결국, 마우스 키 입력하던 부분은 Managers.cs의 _input.OnUpdate()에서 해주게 된다., 2021-06-30
        input.OnUpdate();
    }

    
    static void Init()
    {
        //Instance가 null인지 체크, 2021-06-30
        if (s_instance == null)
        {
            //GameObject go : GameObject를 변수명 go 인스턴스화 한 후,
            //GameObject.Find("@Managers") : 게임 오브젝트 찾기(@Manager란 이름을 가진)
            //= :  변수명 gameobject로 인스턴스화 한 코드에 넣기, 2021-06-30
            GameObject go = GameObject.Find("@Managers");

            //Find를 했는데 @Managers를 못찾아서, go가 null이면, 2021-06-30
            if (go == null)
            {
                //하이어라키창에서 우클릭하여 Create해주던 과정을 코드로 실행하여 게임오브젝트를 만들어주면 된다.
                //빈 게임오브젝트에 이름이 @Managers인 것을 만들자., 2021-06-30
                go = new GameObject { name = "@Managers" };

                //그리고 컴포넌트를 붙여줘야한다. Managers 스크립트를 붙여주자
                go.AddComponent<Managers>();
            }

            //바로 위에서 안전장치를 만들어 줬지만, 절대적인 안정장치를 하나 더 추가하자.
            //절대 파괴하게 할 수 없는 DontDestroyOnLoad를 추가하자., 2021-06-30
            DontDestroyOnLoad(go);
            //Instance에 무언가를 반드시 넣어줘야 한다. Managers 스크립트 넣기, 2021-06-30
            s_instance = go.GetComponent<Managers>();
            //유니티를 실행해보면 @Managers가 없는 것에 당황하지 말자. DontDestroyOnLoad 왼쪽 화살표를 눌리면 아래에 @Managers가 생기는 것을 확인할 수 있다. , 2021-06-30       
        }
    }
}
