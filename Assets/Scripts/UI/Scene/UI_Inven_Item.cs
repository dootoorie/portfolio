using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base        //���������� �ߴ� �˾��� �ƴϰ�, �ϳ��� �����ϴ� ���䵵 �ƴϴϱ� UI_Base�� ��������, 2021-07-19
{
    enum GameObjects
    {
        ItemIcon,
        ItemNameText,
    }

    string _name;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        //���� ������Ʈ�� ã�Ƽ� ���°� �ƴ϶�, ������Ʈ�� ������ �ִ� ���ӿ�����Ʈ ��ü�� ���ε�, 2021-07-19 
        Bind<GameObject>(typeof(GameObjects));

        //ItemNameText�̶� �̸��� ���ӿ�����Ʈ ���� + Text������Ʈ�� �������鼭, �츮�� ���ϴ� �ؽ�Ʈ�� �������� �ٲ� �� �ִ�, 2021-07-19
        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<Text>().text = _name;

        //ItemIcon�̶� �̸��� ���ӿ�����Ʈ ���� + Image ������Ʈ�� �����ͼ�, �������� Ŭ�� ���� �� {  } �ӿ� �ִ� ���� ����, 2021-07-19
        Get<GameObject>((int)GameObjects.ItemIcon).AddUIEvent((PointerEventData) => { Debug.Log($"������ Ŭ��! {_name}"); });
    }

    //Text�� ������ �ٲٴ� �Լ�(= ������ �̸�), 2021-07-19
    public void SetInfo(string name)
    {
        _name = name;
    }
}
