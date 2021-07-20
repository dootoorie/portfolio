using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//2021-07-08
public class UI_Button : UI_Popup
{
    //< UI�ڵ�ȭ >
    //���÷��� : C#���� ���÷����̶�� ���� ����� ����(=������ �������� ���)

    //enum�� ��� �ٸ����� �Ѱ��� �� ������? -> reflection(���÷���)
    enum Buttons
    {
        PointButton
    }

    //enum�� ��� �ٸ����� �Ѱ��� �� ������? -> reflection(���÷���)
    enum Texts
    {
        PointText,
        ScoreText,
    }

    enum GameObjects
    {
        TestObject,
    }

    enum Images
    {
        ItemIcon,
    }

    
    private void Start()
    {
        Init();
    }


    public override void Init()
    {
        //�θ��� Init()�� �����´�.
        base.Init();

        //Generic <T> : Button������Ʈ�� �����ִ� ���ӿ�����Ʈ�� ã�� ���̴�.
        Bind<Button>(typeof(Buttons));

        //Generic <T> : Text������Ʈ�� �����ִ� ���ӿ�����Ʈ�� ã�� ���̴�.
        Bind<Text>(typeof(Texts));

        Bind<GameObject>(typeof(GameObjects));

        //T Get<T>���� T�� �����ϸ鼭 Text�� Ƣ���, 2021-07-16
        //Get<Text>((int)Texts.ScoreText).text = "Bind Test";

        Bind<Image>(typeof(Images));


        //���̾��Ű â�� �ִ� ItemIcon ���ӿ�����Ʈ�� �����´�.
        GameObject go = GetImage((int)Images.ItemIcon).gameObject;

        //2021-07-17
        BindEvent(go, (PointerEventData data) => { go.gameObject.transform.position = data.position; }, Define.UIEvent.Drag);

        //��ư Ŭ���� ���� ���. Extension �޼ҵ带 ����Ͽ� �������(Extension.cs) ,2021-07-17 
        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);
    }


    int score = 0;

    //�� public���� ���ش�. UI�� ������.
    public void OnButtonClicked(PointerEventData data)
    {
        Debug.Log("ButtonClicked");

        score++;

        //��ư ������ ���� ����ϴ� ���� ���, 2021-07-17
        GetText((int)Texts.ScoreText).text = $"���� : {score}";
    }
}