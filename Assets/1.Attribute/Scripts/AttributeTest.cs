using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttributeTest : MonoBehaviour
{
    //[MyCustom]
    [MyCustomAttribute(name = "MyInteger", value = 1)]
    public int myInt; // !���! ����

    private static int myStaticInt;

    [MyCustom] // MyCustomAttribute�� �⺻ �����ڸ� ȣ���Ͽ� Attribute(��Ÿ������) ����
    public int myInt2;

    public string myString; // TextArea Attribute�� �������� ���� string ��� ����
    //[TextArea(1, 10)] Ȥ��
    [TextArea(minLines: 1, maxLines: 10)]
    public string myTextArea; // TextArea Attribute�� ������ string ��� ����

    [Space(300)]
    public int anotherInt;

    [MethodMessage("�̰� private �޼ҵ��Դϴ�.")]
    private void TestMethod()
    {
        print($"��ũ����.. �׽��� �ø��õ�...{myInt}");
    }
}


public class MyCustomAttribute : Attribute
{
    public string name;
    public float value;

    public MyCustomAttribute()
    {
        name = "No Name";
        value = -1;
    }
}

// �޼ҵ忡 ���� Attribute
// AttributeUsage�� Attribute�� ���� ����� Ÿ���� �������ִ� Attribute
// Attribute�� ���������� ������ �� Ŭ���� �տ� ������ Attribute
[AttributeUsage(AttributeTargets.Method)]
public class MethodMessageAttribute : Attribute
{
    public string msg;

    public MethodMessageAttribute(string msg)
    {
        this.msg = msg;
    }
}