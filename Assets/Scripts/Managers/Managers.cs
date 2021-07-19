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

    //���� ���� InpuManager.cs �߰��Ͽ� �ν��Ͻ�ȭ, 2021-06-30
    InputManager input = new InputManager();

    //���� ���� Resourcemanager.cs �߰��Ͽ� �ν��Ͻ�ȭ, 2021-06-30
    ResourceManager resource = new ResourceManager();

    //���� ���� UIManager.cs �߰��Ͽ� �ν��Ͻ�ȭ, 2021-07-18
    UIManager ui = new UIManager();

    //InputManager.cs�� Input�̶�� �̸����� �ҷ��´�
    //�������� ���� InputManager.cs�� ����ϰ� ������, Managers.Input�� ���� �ҷ����� �ȴ�., 2021-06-30
    public static InputManager Input
    { 
        get 
        { 
            return Instance.input; 
        }
    }

    //ResourceManager.cs�� Resource��� �̸����� �ҷ��´�.
    //�������� ���� ResourceManager.cs�� ����ϰ� ������, Managers.Resource�� ���� �ҷ����� �ȴ�., 2021-06-30
    public static ResourceManager Resource 
    { 
        get 
        { 
            return Instance.resource; 
        } 
    }

    //UIManager.cs�� UI��� �̸����� �ҷ��´�.
    //�������� ���� UIManager.cs�� ����ϰ� ������, Managers.UI�� ���� �ҷ����� �ȴ�, 2021-07-18
    public static UIManager UI 
    { 
        get 
        { 
            return Instance.ui;
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
        
        //, 2021 - 06 - 30
        Init();        
    }

    void Update()
    {
        //�ᱹ, ���콺 Ű �Է��ϴ� �κ��� Managers.cs�� _input.OnUpdate()���� ���ְ� �ȴ�., 2021-06-30
        input.OnUpdate();
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
        }
    }
}
