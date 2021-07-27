using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    // Layer��ȣ, 2021-07-26
    public enum Layer
    {
        Ground = 7,
        Monster = 8,
        Wall = 9,
        Block = 10,
    }

    // Scene, 2021-07-20
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    // Sound, 2021-07-20
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,           //���� �������� �߰�, 2021-07-20
    }

    // UI, 2021-07-17
    public enum UIEvent
    {
        Click,
        Drag,
    }

    // ���콺, 2021-07-05, 2021-07-26
    public enum MouseEvent
    {
        Press,              //���콺�� ������ �ִ� ����, 2021-07-05
        PointerDown,        //�� ó�� ���콺�� �� ���¿��� ���콺�� ������ ��, 2021-07-26
        PointerUp,          //���콺�� �� ���̶� ���� ���¿��� ������ �ִٰ� ���콺�� ���� ��, 2021-07-26 
        Click,              //���콺�� Ŭ���ϰ� ���� ���� ��, 2021-07-05
        
    }

    // ī�޶���, 2021-07-04
    public enum CameraMode
    {
        QuaterView,         //���ͺ�, 2021-07-04       
    }
}