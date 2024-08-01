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
        // Reflection�� ����Ͽ� AttributeTest�� ����

        // 1. attTest�� Type�� Ȯ��
        //Type attTestType = typeof(AttributeTest);

        // GetType���� Ÿ���� Ȯ���ϸ�
        // �ڽ��� �Ǵ��� �ڽ̵Ǳ� ���� ������ Ÿ������ ��ȯ�Ѵ�.
        // ���� Ŭ������ boxing�� �ص� ���� ��ü�� type�� ��ȯ
        Type attTestType = attTest.GetType();



        // 2. AttributeTest��� Ŭ������ �����͸� Ȯ���غ���

        //BindingFlags bind = BindingFlags.NonPublic | BindingFlags.Static;
        BindingFlags bind = BindingFlags.Public | BindingFlags.Instance;
        FieldInfo[] fis = attTestType.GetFields(bind); // �ʵ�(�������)

        //print(fis.Length);
        foreach(FieldInfo fi in fis)
        {
            if(fi.GetCustomAttribute<MyCustomAttribute>() == null)
            {
                // FieldInfo�� MyCustomAttribute ��Ʈ����Ʈ�� �����Ǿ����� ������ �ǳʶ�
                continue;
            }

            MyCustomAttribute customAtt = fi.GetCustomAttribute<MyCustomAttribute>();

            //print($"Name : {fi.Name}, Type : {fi.FieldType}, AttName : {customAtt.name}, AttValue : {customAtt.value}");
        }


        // TestMethod�� MethodInfo �Ǵ� MemeberInfo�� Ž���ؼ�
        // MethodMessageAttribute.msg�� ����غ�����.
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


        // ���1
        // SendMessage�� ����� ��� - ������� �߻�
        //MethodInfo testMethod = attTestType.GetMethod("TestMethod");

        // ���2
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
            mi.Invoke(attTest, null); // private������ �̷��� ȣ���� �����ϴ�.
        }
    }
}
