using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTest : MonoBehaviour
{
    GameObject tank;

    void Start()
    {
        //�� ���, �������� �̸��� �� Tank���߸� ����ȴ�. �׷��� ������ ������� �ʴ´�.
        tank = Managers.Resource.Instantiate("Tank");

        //���� �ڵ�� ���� �� �̻� ���� �ʴ´�.

        //�ڵ尡 �ƴ� �͵��� ���� Resources�� �־ ����.
        //���� : Resources�������� �ε��Ұǵ� ���ӿ�����Ʈ�� (Prefabs������ Tank������)
        //prefab = Resources.Load<GameObject>("Prefabs/Tank");
        
        //tank = Instantiate(prefab);

        //Destroy(tank, 3.0f);
    }    
}
