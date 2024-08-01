using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttributeTest : MonoBehaviour
{
    //[MyCustom]
    [MyCustomAttribute(name = "MyInteger", value = 1)]
    public int myInt; // !멤버! 변수

    private static int myStaticInt;

    [MyCustom] // MyCustomAttribute의 기본 생성자를 호출하여 Attribute(메타데이터) 생성
    public int myInt2;

    public string myString; // TextArea Attribute가 부착되지 않은 string 멤버 변수
    //[TextArea(1, 10)] 혹은
    [TextArea(minLines: 1, maxLines: 10)]
    public string myTextArea; // TextArea Attribute가 부착된 string 멤버 변수

    [Space(300)]
    public int anotherInt;

    [MethodMessage("이건 private 메소드입니다.")]
    private void TestMethod()
    {
        print($"시크릿또.. 테스토 시마시따...{myInt}");
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

// 메소드에 붙일 Attribute
// AttributeUsage는 Attribute가 붙을 대상의 타입을 제한해주는 Attribute
// Attribute에 제약조건을 설정할 때 클래스 앞에 부착할 Attribute
[AttributeUsage(AttributeTargets.Method)]
public class MethodMessageAttribute : Attribute
{
    public string msg;

    public MethodMessageAttribute(string msg)
    {
        this.msg = msg;
    }
}