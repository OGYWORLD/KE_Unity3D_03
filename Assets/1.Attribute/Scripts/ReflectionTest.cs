using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class ReflectionTest : MonoBehaviour
{
    private AttributeTest attTest;

    private void Awake()
    {
        attTest = GetComponent<AttributeTest>();
    }

    private void Start()
    {
        // Reflection을 사용하여 AttributeTest에 접근

        // 1. attTest의 Type을 확인
        //Type attTestType = typeof(AttributeTest);

        // GetType으로 타입을 확인하면
        // 박싱이 되더라도 박싱되기 전의 데이터 타입으로 반환한다.
        // 상위 클래스로 boxing을 해도 원래 객체의 type을 반환
        Type attTestType = attTest.GetType();



        // 2. AttributeTest라는 클래스의 데이터를 확인해보자

        //BindingFlags bind = BindingFlags.NonPublic | BindingFlags.Static;
        BindingFlags bind = BindingFlags.Public | BindingFlags.Instance;
        FieldInfo[] fis = attTestType.GetFields(bind); // 필드(멤버변수)

        //print(fis.Length);
        foreach(FieldInfo fi in fis)
        {
            if(fi.GetCustomAttribute<MyCustomAttribute>() == null)
            {
                // FieldInfo에 MyCustomAttribute 어트리뷰트가 부착되어있지 않으면 건너뜀
                continue;
            }

            MyCustomAttribute customAtt = fi.GetCustomAttribute<MyCustomAttribute>();

            //print($"Name : {fi.Name}, Type : {fi.FieldType}, AttName : {customAtt.name}, AttValue : {customAtt.value}");
        }


        // TestMethod의 MethodInfo 또는 MemeberInfo를 탐색해서
        // MethodMessageAttribute.msg를 출력해보세요.
        /*
        BindingFlags bind2 = BindingFlags.NonPublic;
        MethodInfo[] fis2 = attTestType.GetMethods(bind2);
        print(fis2.Length);
        foreach (MethodInfo fi in fis2)
        {
            if (fi.GetCustomAttribute<MethodMessageAttribute>() == null)
            {
                continue;
            }

            MethodMessageAttribute customAtt = fi.GetCustomAttribute<MethodMessageAttribute>();

            print($"msg: {customAtt.msg}");
        }
        */


        // 방법1
        // SendMessage랑 비슷한 용법 - 오버헤드 발생
        //MethodInfo testMethod = attTestType.GetMethod("TestMethod");

        // 방법2
        bind = BindingFlags.NonPublic | BindingFlags.Instance;
        MethodInfo[] mis = attTestType.GetMethods(bind);
        print(mis.Length);

        foreach(MethodInfo mi in mis)
        {
            print(mi.Name);
            if(mi.GetCustomAttribute<MethodMessageAttribute>() == null)
            {
                continue;
            }

            var Att = mi.GetCustomAttribute<MethodMessageAttribute>();

            print(Att.msg);
            mi.Invoke(attTest, null); // private이지만 이렇게 호출은 가능하다.
        }
    }
}
