using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager //�� ��ũ��Ʈ�� ������Ʈ�� ������ ���� �����̹Ƿ� �Ϲ� C# ��ũ��Ʈ�� �Ѵ�.
{
    //where T : Object : �θ� Ŭ������ Object�� Ÿ�Ը� ���� �� �ֵ��� ������ ����
   public T Load<T>(string path) where T : Object
    {
        //Resources ������ ���� ��ġ�� �� "path"�� �ش��ϴ� T Ÿ���� ���� ������ �ҷ����� �̸� �����Ѵ�.
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        //Load�� ����� prefab�� path�� �ش��ϴ� GameObejct Ÿ���� ������ �Ҵ��Ѵ�
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");

        //�������� ������
        if (prefab == null)
        {
            //���� �޽��� ��Ÿ����({path} : �������)
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        //���� Instantiate�� �� �տ� �� �ٿ��µ�,
        //Object�� ���� ������ Instantiate�� ������ 12��° ���� ��������� ȣ���Ϸ��� �ϴϱ�
        //Object�� �ִ� Instantiate�� �϶�� ��Ȯ�� �ڵ�
        return Object.Instantiate(prefab, parent);
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        Object.Destroy(go);
    }

}
