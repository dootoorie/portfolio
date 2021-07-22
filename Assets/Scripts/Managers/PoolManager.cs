using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PoolManager는 혼자 자체로 존재하는 것이 아니라, 다른 애한테 기생해서 자신의 존재감을 뽐낸다.
//ResourceManager를 보조하는 역할을 할 것이다.

// Pop : 대기하고 있는 Pooling 된 오브젝트가 있는지 확인하여, 있으면 바로 사용하겠다는 뜻 
// Push : Pooling 된 오브젝트를 다 사용한 다음에, 반환하는 작업

//2021-07-21
public class PoolManager    //MonoBehaviour 제거
{
    //여기서 프리팹을 하나하나 관리,2021-07-22
    class Pool
    {
        //원본 프리팹
        public GameObject Original { get; private set; }

        //하이어라키창에 프리팹(자식)을 담을 게임오브젝트(부모)
        public Transform Root { get; set; }

        //_poolStack에 저장할 것
        Stack<Poolable> _poolStack = new Stack<Poolable>();

        //count : 초기화 할 때, 하이어라키창의 프리팹 오브젝트를 몇 개를 준비해둘껀지
        public void Init(GameObject original, int count = 5)
        {
            //원본 프리팹을 Original에 저장
            Original = original;

            //하이어라키창에 프리팹(자식)을 담을 게임오브젝트(부모) 생성
            Root = new GameObject().transform;
            
            //생성 된 게임오브젝트(부모)의 이름을 변경
            Root.name = $"{original.name}_Root";


            for (int i = 0; i < count; i++)
            {
                //프리팹을 다루는 액션은 2가지로 나뉜다(만들기(Create()/넣기(Push())
                //프리팹을 만들자마자 넣어주기
                Push(Create());
            }
        }

        //새로운 객체를 만들어주고 반환을 Poolable로 해줌
        Poolable Create()
        {
            //원본 프리팹(Original)을 복사본(go)에 저장
            GameObject go = Object.Instantiate<GameObject>(Original);

            //복사본이 (Clone)이라고 만들어지는 것을 피하기위해 강제로 이름 바꾸기
            go.name = Original.name;

            //Poolable 반환
            return go.GetOrAddComponent<Poolable>();
        }

        //_poolStack 에다가 바로 프리팹들을 넣는건 안되고, 프리팹을 넣을 위치와,
        //게임오브젝트 불을 켠 상태인지 끈 상태인지 확인하고나서 _poolStack에다가 프리팹들을 넣기 위한 함수
        public void Push(Poolable poolable)
        {
            //poolable이 null이면 뭔가 문제가 있는 것
            if (poolable == null)
            {
                return;
            }

            //Root 게임오브젝트가 프리팹들의 부모님 
            poolable.transform.parent = Root;

            //하이어라키창에 게임오브젝트를 끈 상태로 만듬
            poolable.gameObject.SetActive(false);

            //Poolable.cs의 bool값
            poolable.IsUsing = false;

            //_poolStack에다가 프리팹을 넣어주기
            _poolStack.Push(poolable);
        }

        //_poolStack에 미리 넣어놨던 프리팹을 꺼내오기
        //Original은 Init()에서 세팅할 것이기 때문에, parent만 받는다
        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            //_poolStack에 넣어놨던 프리팹이 1개 이상 존재한다면
            if (_poolStack.Count > 0)
            {
                //프리팹을 하나 꺼내서 poolable 변수에 저장
                poolable = _poolStack.Pop();
            }

            //_poolStack에 넣어놨던 프리팹이 0개라면
            else
            {
                //프리팹을 하나 만든 후, poolable 변수에 저장
                poolable = Create();
            }

            //DontDestroyOnLoad 해제 용도
            if (parent == null)
            {
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;
            }

            //게임오브젝트 불 켜기
            poolable.gameObject.SetActive(true);

            //매개변수 parent를 프리팹이 담길 부모로 정함
            poolable.transform.parent = parent;

            //Poolable.cs의 bool값
            poolable.IsUsing = true;

            //반환
            return poolable;
        }
    }

    //PoolManager는 여러개의 _pool을 갖고 있는데, 각각의 Class Pool들은, string이라는 Key를 이용해서 관리할 것,2021-07-22
    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();

    Transform _root;

    //2021-07-22
    public void Init()
    {
        //초기화 할 때, _root가 없는 상태라면,
        if (_root == null)
        {
            //_root를 만들어준다. 이름은 "@Pool_Root"
            _root = new GameObject { name = "@Pool_Root" }.transform;

            Object.DontDestroyOnLoad(_root);
        }
    }

    //프리팹을 담을 빈 게임오브젝트인 Pool 생성
    public void CreatePool(GameObject original, int count = 5)
    {
        //프리팹들을 담을 빈 게임오브젝트
        Pool pool = new Pool();

        //빈 게임오브젝트를 생성하고 이름을 바꾸는 Init함수 실행
        pool.Init(original, count);

        //빈 게임오브젝트들인 Pool들을, 가장 최상위에 위치한 빈 게임오브젝트인 @Pool_Root를 부모로 지정
        pool.Root.parent = _root;

        //Dictionary에 프리팹을 담을 빈 게임오브젝트 넣어주기
        //매개변수 : key로 이용하기로 한 original.name과 데이터로는 프리팹을 담을 빈 게임오브젝트인 pool
        _pool.Add(original.name, pool);

    }

    //보통 Pooling하는 Manager는 Push와 Pop을 많이들 쓴다.
    //Push : 반환. Pool에 넣어둬라, 2021-07-22
    public void Push(Poolable poolable)
    {
        // Pop : 대기하고 있는 Pooling 된 오브젝트가 있는지 확인하여, 있으면 바로 사용하겠다는 뜻 
        // Push : Pooling 된 오브젝트를 다 사용한 다음에, 반환하는 작업

        //이름을 추출하고 싶은데, poolable도 컴포넌트이니까 .을 찍어보면 당연히 gameObject를 들고올 수 있고,
        //gameObject 산하에는 name이 있으니 name을 들고올 수 있음. 그러므로 들고와서 name변수에 저장.
        string name = poolable.gameObject.name;

        //Dictionary인 _pool에 아무것도 없으면
        if (_pool.ContainsKey(name) == false)
        {
            //직접 Pool에 넣지 않고 poolable을 날려버리기
            GameObject.Destroy(poolable.gameObject);

            return;
        }
        //_pool의 name에 접근해서 Push를 하고자 하는 후보(poolable)를 적어줌
        _pool[name].Push(poolable);
    }

    //ResourceManager.cs 에서 
    //GameObject go = Object.Instatiate(prefab, parent)을 개선하는 코드를 작성하자.
    //여기서 prefab은 오리지날, parent는 어디다 위치시킬건지(원본을 만들고 parent 밑으로 위치)
    //그러므로 지금 여기 이 함수의 매개변수도 개선할 코드와 함께 똑같이 적용시킨다, 2021-07-22
    public Poolable Pop(GameObject original, Transform parent = null)
    {
        //맨 처음에 Pop에 접근하면 아무것도 없기 때문에 오류가 날 수 있다. Dictionary인 _pool에 original이 있는지 확인
        if (_pool.ContainsKey(original.name) == false)
        {
            //CreatePool 함수on
            CreatePool(original);
        }

        //Dictionary인 _pool에 접근하여 프리팹 이름에 접근하여 가져와서(= Class Pool이 리턴), Pop함수를 사용.
        return _pool[original.name].Pop(parent);
    }

    //2021-07-22
    public GameObject GetOriginal(string name)
    {
        //맨 처음에 GetOriginal 접근하면 Pop()을 안한상태라서,
        //즉 _pool을 만들지 않은 상태라서 _pool이 없기 때문에 오류가 날 수 있다.
        //Dictionary인 _pool에 name이 있는지 확인
        if (_pool.ContainsKey(name) == false)
        {
            return null;
        }

        return _pool[name].Original;
    }

    //씬에서 다른 씬으로 넘어갈 때, 우리가 애써서 한 캐시를 날려야할지, 아니면 그대로 유지해야 할지, 게임마다 다르다.
    public void Clear()
    {
        //일단 날려주는 코드로 정하자.
        //이때까지 다룬 게임오브젝트들은 전부 @Pool_Root 게임오브젝트의 산하에 들어가 있기 때문에,
        //@Pool_Root에 접근해서 모든 자식들을 다 Destroy 하면 될 것.
        foreach(Transform child in _root)
        {
            //@Pool_Root의 자식들을 전부 파괴
            GameObject.Destroy(child.gameObject);
        }

        //_pool도 초기화
        _pool.Clear();

    }
}