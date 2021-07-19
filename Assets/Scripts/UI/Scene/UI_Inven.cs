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

        //GameObjectŸ���� ���ε�
        Bind<GameObject>(typeof(GameObjects));

        //���� : �κ��丮 ������ ������(12�� ����) ������� ���� ������
        //��� : ���̾��Ű â������ GridPanel ���� ������Ʈ�� ���� gridPanel�� �־��ش�.        
        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);

        //���� : ��(=���̾��Űâ�� GridPanel)�� ������ �ִ� �ڽĵ��� ��� ����
        //��� : �� transform�� ��� �ִ� ��� �ڽĵ��� ��ȸ
        foreach (Transform child in gridPanel.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        //���� �κ��丮 ������ �����ؼ�
        for (int i = 0; i < 8; i++)
        {
            //������ ���� (��ġ : UI����/Scene����/UI_Inven_Item ������)
            GameObject item = Managers.Resource.Instantiate("UI/Scene/UI_Inven_Item");

            //����(= �θ�� ����)
            item.transform.SetParent(gridPanel.transform);

            //UI_Inven_Item.cs ������Ʈ�� UI_Inven_Item �����տ� ���̱�, ���� ����, �ν��Ͻ�ȭ�� invenItem������ ���̱�
            UI_Inven_Item invenItem = Util.GetOrAddComponent<UI_Inven_Item>(item);

            //UI_Inven_Item.cs�� SetInfo�Լ��� �̿��Ͽ� Text ���� ����(= ������ �̸�)
            invenItem.SetInfo($"�����{i}��");
        }
    }
}
