using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    //static�̴� s�� �ٿ��� ��, 2021-06-30
    static Managers s_instance;

    //Managers.cs�� Instance��� �̸����� �ҷ��´�
    //Instance�� �ܺο��� �����ϱ⸦ ��ġ �����ϱ� public ����, 2021-06-30
    static Managers Instance 
    { 
        get 
        { 
            Init(); 
            
            return s_instance; 
        } 
    }

    //Contents����

    //���� ���� GameManager.cs �߰��Ͽ� �ν��Ͻ�ȭ, 2021-07-29
    GameManager _game = new GameManager();

    public static GameManager Game
    {
        get
        {
            return Instance._game;
        }
    }

    //Core����

    //���� ���� DataManager.cs �߰��Ͽ� �ν��Ͻ�ȭ, 2021-07-23
    DataManager _data = new DataManager();

    //���� ���� InpuManager.cs �߰��Ͽ� �ν��Ͻ�ȭ, 2021-06-30
    InputManager _input = new InputManager();

    //���� ���� PoolManager.cs �߰��Ͽ� �ν��Ͻ�ȭ, 2021-07-21
    PoolManager _pool = new PoolManager();

    //���� ���� Resourcemanager.cs �߰��Ͽ� �ν��Ͻ�ȭ, 2021-06-30
    ResourceManager _resource = new ResourceManager();

    //���� ���� SceneManagerEx.cs �߰��Ͽ� �ν��Ͻ�ȭ, 2021-07-20
    SceneManagerEx _scene = new SceneManagerEx();

    //���� ���� SoundManager.cs �߰��Ͽ� �ν��Ͻ�ȭ, 2021-07-20
    SoundManager _sound = new SoundManager();

    //���� ���� UIManager.cs �߰��Ͽ� �ν��Ͻ�ȭ, 2021-07-18
    UIManager _ui = new UIManager();


    //DataManager.cs�� Data��� �̸����� �ҷ��´�
    //�������� ���� DataManager.cs�� ����ϰ� ������, Manager.Date�� ���� �ҷ����� �ȴ�, 2021-07-23
    public static DataManager Data
    {
        get
        {
            return Instance._data;
        }
    }

    //InputManager.cs�� Input�̶�� �̸����� �ҷ��´�
    //�������� ���� InputManager.cs�� ����ϰ� ������, Managers.Input�� ���� �ҷ����� �ȴ�, 2021-06-30
    public static InputManager Input
    { 
        get 
        { 
            return Instance._input; 
        }
    }

    //PoolManager.cs�� Resource��� �̸����� �ҷ��´�.
    //�������� ���� PoolManager.cs�� ����ϰ� ������, Managers.Pool�� ���� �ҷ����� �ȴ�, 2021-07-21
    public static PoolManager Pool
    {
        get
        {
            return Instance._pool;
        }
    }

    //ResourceManager.cs�� Resource��� �̸����� �ҷ��´�.
    //�������� ���� ResourceManager.cs�� ����ϰ� ������, Managers.Resource�� ���� �ҷ����� �ȴ�, 2021-06-30
    public static ResourceManager Resource 
    { 
        get 
        { 
            return Instance._resource; 
        } 
    }

    //SceneManagerEx.cs�� Scene�̶�� �̸����� �ҷ��´�.
    //�������� ���� SceneManagerEx.cs�� ����ϰ� ������, SceneManagerEx Scene�� ���� �ҷ����� �ȴ�, 2021-07-20
    public static SceneManagerEx Scene
    {
        get
        {
            return Instance._scene;
        }
    }

    //SoundManager.cs�� Sound��� �̸����� �ҷ��´�.
    //�������� ���� SoundManager.cs�� ����ϰ� ������, SoundManager Sound�� ���� �ҷ����� �ȴ�, 2021-07-20
    public static SoundManager Sound
    {
        get
        {
            return Instance._sound;
        }
    }

    //UIManager.cs�� UI��� �̸����� �ҷ��´�.
    //�������� ���� UIManager.cs�� ����ϰ� ������, Managers.UI�� ���� �ҷ����� �ȴ�, 2021-07-18
    public static UIManager UI 
    { 
        get 
        { 
            return Instance._ui;
        } 
    }


    void Start()
    {
        //�� ���ٺ��ʹ� ������5 �κ����� �̵� ��Ŵ
        //GameObject gameobject : GameObject�� ������ gameobject�� �ν��Ͻ�ȭ �� ��,
        //GameObject.Find("@Managers") : ���� ������Ʈ ã��(@Manager�� �̸��� ����)
        //= :  ������ gameobject�� �ν��Ͻ�ȭ �� �ڵ忡 �ֱ�, 2021-06-30
        GameObject gameobject = GameObject.Find("@Managers");

        //"@Managers"�� ���ӿ�����Ʈ�� ������ ������Ʈ�� Managers��ũ��Ʈ�� �������� 
        //Managers mg : Managers ��ũ��Ʈ�� ManagersŬ������ ������ mg�� �ν��Ͻ�ȭ ��
        //gameobject.GetComponent<Managers>() : ������ gameobject�� ������ ������Ʈ�� Managers��ũ��Ʈ�� �������� 
        //= : ������ �ڵ带 ���� �ڵ忡 �ֱ�
        //Instance = gameobject.GetComponent<Managers>();        
        
        //, 2021-06-30
        Init();        
    }

    void Update()
    {
        //�ᱹ, ���콺 Ű �Է��ϴ� �κ��� Managers.cs�� _input.OnUpdate()���� ���ְ� �ȴ�, 2021-06-30
        _input.OnUpdate();
    }

    
    static void Init()
    {
        //Instance�� null���� üũ, 2021-06-30
        if (s_instance == null)
        {
            //GameObject go : GameObject�� ������ go �ν��Ͻ�ȭ �� ��,
            //GameObject.Find("@Managers") : ���� ������Ʈ ã��(@Manager�� �̸��� ����)
            //= :  ������ gameobject�� �ν��Ͻ�ȭ �� �ڵ忡 �ֱ�, 2021-06-30
            GameObject go = GameObject.Find("@Managers");

            //Find�� �ߴµ� @Managers�� ��ã�Ƽ�, go�� null�̸�, 2021-06-30
            if (go == null)
            {
                //���̾��Űâ���� ��Ŭ���Ͽ� Create���ִ� ������ �ڵ�� �����Ͽ� ���ӿ�����Ʈ�� ������ָ� �ȴ�.
                //�� ���ӿ�����Ʈ�� �̸��� @Managers�� ���� ������., 2021-06-30
                go = new GameObject { name = "@Managers" };

                //�׸��� ������Ʈ�� �ٿ�����Ѵ�. Managers ��ũ��Ʈ�� �ٿ�����
                go.AddComponent<Managers>();
            }

            //�ٷ� ������ ������ġ�� ����� ������, �������� ������ġ�� �ϳ� �� �߰�����.
            //���� �ı��ϰ� �� �� ���� DontDestroyOnLoad�� �߰�����., 2021-06-30
            DontDestroyOnLoad(go);
            //Instance�� ���𰡸� �ݵ�� �־���� �Ѵ�. Managers ��ũ��Ʈ �ֱ�, 2021-06-30
            s_instance = go.GetComponent<Managers>();
            //����Ƽ�� �����غ��� @Managers�� ���� �Ϳ� ��Ȳ���� ����. DontDestroyOnLoad ���� ȭ��ǥ�� ������ �Ʒ��� @Managers�� ����� ���� Ȯ���� �� �ִ�. , 2021-06-30       

            //DataManager.cs�� Init()�Լ��� ȣ��, 2021-07-23
            s_instance._data.Init();

            //Instance�� �����ϸ� �ȵ�. �ֳ��ϸ� Instance�ȿ� �̹� Init()�� �ֱ� ������ �ߺ��Ǿ� ���ѷ��� ��.
            //SoundManager.cs�� Init()�Լ��� ȣ��, 2021-07-20
            s_instance._sound.Init();

            //PoolManager.cs�� Init()�Լ��� ȣ��, 2021-07-22
            s_instance._pool.Init();
        }
    }

    //���� �ٲ� ��, �޸� ������ ����, �����ϴ� audio ���� �����͸� �����ֱ�, 2021-07-21
    public static void Clear()
    {
        //InputManager.cs �� Clear()�Լ�
        Input.Clear();

        //SoundManager.cs �� Clear()�Լ�
        Sound.Clear();

        //SceneManagerEx.cs �� Clear()�Լ�
        Scene.Clear();

        //UIManager.cs �� Clear()�Լ�
        UI.Clear();

        //PoolManager.cs�� Clear()�Լ� - Ư���ϰ� �ٸ� Clear()���� �ڿ� ������Ѵ�(�ٸ� ������ Pooling �� �������� ����ϰ� �������� �����Ƿ�)
        Pool.Clear();
    }
}
