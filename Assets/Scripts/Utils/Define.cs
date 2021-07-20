using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
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

    // ���콺, 2021-07-05
    public enum MouseEvent
    {
        Press,              //���콺�� ������ �ִ� ����, 2021-07-05
        Click,              //���콺�� Ŭ���ϰ� ���� ���� ��, 2021-07-05
    }

    // ī�޶���, 2021-07-04
    public enum CameraMode
    {
        QuaterView,         //���ͺ�, 2021-07-04
    }
}