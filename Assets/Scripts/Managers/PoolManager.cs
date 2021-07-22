using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PoolManager�� ȥ�� ��ü�� �����ϴ� ���� �ƴ϶�, �ٸ� ������ ����ؼ� �ڽ��� ���簨�� �˳���.
//ResourceManager�� �����ϴ� ������ �� ���̴�.

// Pop : ����ϰ� �ִ� Pooling �� ������Ʈ�� �ִ��� Ȯ���Ͽ�, ������ �ٷ� ����ϰڴٴ� �� 
// Push : Pooling �� ������Ʈ�� �� ����� ������, ��ȯ�ϴ� �۾�

//2021-07-21
public class PoolManager    //MonoBehaviour ����
{
    //���⼭ �������� �ϳ��ϳ� ����,2021-07-22
    class Pool
    {
        //���� ������
        public GameObject Original { get; private set; }

        //���̾��Űâ�� ������(�ڽ�)�� ���� ���ӿ�����Ʈ(�θ�)
        public Transform Root { get; set; }

        //_poolStack�� ������ ��
        Stack<Poolable> _poolStack = new Stack<Poolable>();

        //count : �ʱ�ȭ �� ��, ���̾��Űâ�� ������ ������Ʈ�� �� ���� �غ��صѲ���
        public void Init(GameObject original, int count = 5)
        {
            //���� �������� Original�� ����
            Original = original;

            //���̾��Űâ�� ������(�ڽ�)�� ���� ���ӿ�����Ʈ(�θ�) ����
            Root = new GameObject().transform;
            
            //���� �� ���ӿ�����Ʈ(�θ�)�� �̸��� ����
            Root.name = $"{original.name}_Root";


            for (int i = 0; i < count; i++)
            {
                //�������� �ٷ�� �׼��� 2������ ������(�����(Create()/�ֱ�(Push())
                //�������� �����ڸ��� �־��ֱ�
                Push(Create());
            }
        }

        //���ο� ��ü�� ������ְ� ��ȯ�� Poolable�� ����
        Poolable Create()
        {
            //���� ������(Original)�� ���纻(go)�� ����
            GameObject go = Object.Instantiate<GameObject>(Original);

            //���纻�� (Clone)�̶�� ��������� ���� ���ϱ����� ������ �̸� �ٲٱ�
            go.name = Original.name;

            //Poolable ��ȯ
            return go.GetOrAddComponent<Poolable>();
        }

        //_poolStack ���ٰ� �ٷ� �����յ��� �ִ°� �ȵǰ�, �������� ���� ��ġ��,
        //���ӿ�����Ʈ ���� �� �������� �� �������� Ȯ���ϰ��� _poolStack���ٰ� �����յ��� �ֱ� ���� �Լ�
        public void Push(Poolable poolable)
        {
            //poolable�� null�̸� ���� ������ �ִ� ��
            if (poolable == null)
            {
                return;
            }

            //Root ���ӿ�����Ʈ�� �����յ��� �θ�� 
            poolable.transform.parent = Root;

            //���̾��Űâ�� ���ӿ�����Ʈ�� �� ���·� ����
            poolable.gameObject.SetActive(false);

            //Poolable.cs�� bool��
            poolable.IsUsing = false;

            //_poolStack���ٰ� �������� �־��ֱ�
            _poolStack.Push(poolable);
        }

        //_poolStack�� �̸� �־���� �������� ��������
        //Original�� Init()���� ������ ���̱� ������, parent�� �޴´�
        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            //_poolStack�� �־���� �������� 1�� �̻� �����Ѵٸ�
            if (_poolStack.Count > 0)
            {
                //�������� �ϳ� ������ poolable ������ ����
                poolable = _poolStack.Pop();
            }

            //_poolStack�� �־���� �������� 0�����
            else
            {
                //�������� �ϳ� ���� ��, poolable ������ ����
                poolable = Create();
            }

            //DontDestroyOnLoad ���� �뵵
            if (parent == null)
            {
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;
            }

            //���ӿ�����Ʈ �� �ѱ�
            poolable.gameObject.SetActive(true);

            //�Ű����� parent�� �������� ��� �θ�� ����
            poolable.transform.parent = parent;

            //Poolable.cs�� bool��
            poolable.IsUsing = true;

            //��ȯ
            return poolable;
        }
    }

    //PoolManager�� �������� _pool�� ���� �ִµ�, ������ Class Pool����, string�̶�� Key�� �̿��ؼ� ������ ��,2021-07-22
    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();

    Transform _root;

    //2021-07-22
    public void Init()
    {
        //�ʱ�ȭ �� ��, _root�� ���� ���¶��,
        if (_root == null)
        {
            //_root�� ������ش�. �̸��� "@Pool_Root"
            _root = new GameObject { name = "@Pool_Root" }.transform;

            Object.DontDestroyOnLoad(_root);
        }
    }

    //�������� ���� �� ���ӿ�����Ʈ�� Pool ����
    public void CreatePool(GameObject original, int count = 5)
    {
        //�����յ��� ���� �� ���ӿ�����Ʈ
        Pool pool = new Pool();

        //�� ���ӿ�����Ʈ�� �����ϰ� �̸��� �ٲٴ� Init�Լ� ����
        pool.Init(original, count);

        //�� ���ӿ�����Ʈ���� Pool����, ���� �ֻ����� ��ġ�� �� ���ӿ�����Ʈ�� @Pool_Root�� �θ�� ����
        pool.Root.parent = _root;

        //Dictionary�� �������� ���� �� ���ӿ�����Ʈ �־��ֱ�
        //�Ű����� : key�� �̿��ϱ�� �� original.name�� �����ͷδ� �������� ���� �� ���ӿ�����Ʈ�� pool
        _pool.Add(original.name, pool);

    }

    //���� Pooling�ϴ� Manager�� Push�� Pop�� ���̵� ����.
    //Push : ��ȯ. Pool�� �־�ֶ�, 2021-07-22
    public void Push(Poolable poolable)
    {
        // Pop : ����ϰ� �ִ� Pooling �� ������Ʈ�� �ִ��� Ȯ���Ͽ�, ������ �ٷ� ����ϰڴٴ� �� 
        // Push : Pooling �� ������Ʈ�� �� ����� ������, ��ȯ�ϴ� �۾�

        //�̸��� �����ϰ� ������, poolable�� ������Ʈ�̴ϱ� .�� ���� �翬�� gameObject�� ���� �� �ְ�,
        //gameObject ���Ͽ��� name�� ������ name�� ���� �� ����. �׷��Ƿ� ���ͼ� name������ ����.
        string name = poolable.gameObject.name;

        //Dictionary�� _pool�� �ƹ��͵� ������
        if (_pool.ContainsKey(name) == false)
        {
            //���� Pool�� ���� �ʰ� poolable�� ����������
            GameObject.Destroy(poolable.gameObject);

            return;
        }
        //_pool�� name�� �����ؼ� Push�� �ϰ��� �ϴ� �ĺ�(poolable)�� ������
        _pool[name].Push(poolable);
    }

    //ResourceManager.cs ���� 
    //GameObject go = Object.Instatiate(prefab, parent)�� �����ϴ� �ڵ带 �ۼ�����.
    //���⼭ prefab�� ��������, parent�� ���� ��ġ��ų����(������ ����� parent ������ ��ġ)
    //�׷��Ƿ� ���� ���� �� �Լ��� �Ű������� ������ �ڵ�� �Բ� �Ȱ��� �����Ų��, 2021-07-22
    public Poolable Pop(GameObject original, Transform parent = null)
    {
        //�� ó���� Pop�� �����ϸ� �ƹ��͵� ���� ������ ������ �� �� �ִ�. Dictionary�� _pool�� original�� �ִ��� Ȯ��
        if (_pool.ContainsKey(original.name) == false)
        {
            //CreatePool �Լ�on
            CreatePool(original);
        }

        //Dictionary�� _pool�� �����Ͽ� ������ �̸��� �����Ͽ� �����ͼ�(= Class Pool�� ����), Pop�Լ��� ���.
        return _pool[original.name].Pop(parent);
    }

    //2021-07-22
    public GameObject GetOriginal(string name)
    {
        //�� ó���� GetOriginal �����ϸ� Pop()�� ���ѻ��¶�,
        //�� _pool�� ������ ���� ���¶� _pool�� ���� ������ ������ �� �� �ִ�.
        //Dictionary�� _pool�� name�� �ִ��� Ȯ��
        if (_pool.ContainsKey(name) == false)
        {
            return null;
        }

        return _pool[name].Original;
    }

    //������ �ٸ� ������ �Ѿ ��, �츮�� �ֽἭ �� ĳ�ø� ����������, �ƴϸ� �״�� �����ؾ� ����, ���Ӹ��� �ٸ���.
    public void Clear()
    {
        //�ϴ� �����ִ� �ڵ�� ������.
        //�̶����� �ٷ� ���ӿ�����Ʈ���� ���� @Pool_Root ���ӿ�����Ʈ�� ���Ͽ� �� �ֱ� ������,
        //@Pool_Root�� �����ؼ� ��� �ڽĵ��� �� Destroy �ϸ� �� ��.
        foreach(Transform child in _root)
        {
            //@Pool_Root�� �ڽĵ��� ���� �ı�
            GameObject.Destroy(child.gameObject);
        }

        //_pool�� �ʱ�ȭ
        _pool.Clear();

    }
}