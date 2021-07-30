using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager //�� ��ũ��Ʈ�� ������Ʈ�� ������ ���� �����̹Ƿ� �Ϲ� C# ��ũ��Ʈ�� �Ѵ�.
{
    //where T : Object : �θ� Ŭ������ Object�� Ÿ�Ը� ���� �� �ֵ��� ������ ����
   public T Load<T>(string path) where T : Object
    {
        //�������� ���, original�� ã�ƺ��� �ٷ� ��ȯ����


        //���� T�� GameObject�� ��ġ�ϸ� �������� Ȯ���� ������ �����״� ã�ƺ���
        if(typeof(T) == typeof(GameObject))
        {
            //�������� ����Ǿ� �ִ� ��θ� name���� ����
            string name = path;

            //��� ���� ���, abc/Knight �� ������(/)�� �Ǿ� �ִµ�, �� ������ ������(/)�� ã�Ƽ� �� �޺κи� �����ϸ� �ȴ�.
            //�� ������ ������(/) ã�� ��� : LastIndexOf()�� �̿�
            int index = name.LastIndexOf('/');

            //���� index�� �����ϸ�,
            if (index >= 0)
            {
                //Substring(int startIndex) : ���ڿ� �ڸ���.
                //Substring�� �̿��Ͽ� name ����.
                //index + 1�� �� ������, index������ ������(/)�̹Ƿ�, ������(/) �� �κк��� �߶� �����ϱ� ����
                name = name.Substring(index + 1);
            }

            //������ GetOriginal�� ã������
            GameObject go = Managers.Pool.GetOriginal(name);

            //��ȯ
            if (go != null)
            {
                return go as T;
            }
        }

        //Resources ������ ���� ��ġ�� �� "path"�� �ش��ϴ� T Ÿ���� ���� ������ �ҷ����� �̸� �����Ѵ�.
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        // 1. original �̹� ��� ������ �ٷ� ���
        //Load�� ����� prefab�� path�� �ش��ϴ� GameObejct Ÿ���� ������ �Ҵ��Ѵ�
        GameObject original = Load<GameObject>($"Prefabs/{path}");

        //���� ���� �������� ������
        if (original == null)
        {
            //���� �޽��� ��Ÿ����({path} : �������)
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        //���� Instantiate�� �� �տ� �� �ٿ��µ�,
        //Object�� ���� ������ Instantiate�� ������ 12��° ���� ��������� ȣ���Ϸ��� �ϴϱ�
        //Object�� �ִ� Instantiate�� �϶�� ��Ȯ�� �ڵ�
        //return Object.Instantiate(prefab, parent);

        // 2. Ȥ�� Pooling �� �ְ� ������?

        //���� Poolable ������Ʈ�� ������ �ִ� ���� �������� ������ , (Poolable ������Ʈ�� ������ Pooling �ϴ� ����� �ƴϴϱ�)
        if (original.GetComponent<Poolable>() != null)
        {
            // Pop : ����ϰ� �ִ� Pooling �� ������Ʈ�� �ִ��� Ȯ���Ͽ�, ������ �ٷ� ����ϰڴٴ� �� 
            return Managers.Pool.Pop(original, parent).gameObject;
        }
        //���̾��Ű�� ������ ����. �׸��� ���ӿ�����Ʈ go�� ����, 2021-07-19
        GameObject go = Object.Instantiate(original, parent);

        //������ ������ ����, 201-07-21
        go.name = original.name;

        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        // ���࿡ Pooling�� �ʿ��� ���̶�� -> PoolManager ���� ��Ź

        //�ϴ� Pooling ������� Ȯ������(= Poolable ������Ʈ�� ������ �ִ��� Ȯ��)
        Poolable poolable = go.GetComponent<Poolable>();

        //Destroy �ϱ� ���� ����
        //���� Pooling����̸�,(= Poolable ������Ʈ�� ������ �ִٸ�)
        if (poolable != null)
        {
            // Push : Pooling �� ������Ʈ�� �� ����� ������, ��ȯ�ϴ� �۾�
            Managers.Pool.Push(poolable);

            return;
        }

        //���� Pooling ����� �ƴ϶��, Destroy
        Object.Destroy(go);
    }

}