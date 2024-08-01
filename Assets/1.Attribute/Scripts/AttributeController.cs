using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using System.Linq;

public class AttributeController : MonoBehaviour
{
    private void Start()
    {
        // ColorAttribute�� ���� �ʵ带 ã�´�.
        // BindingFlags : public �̰ų� private ��� ���� static�� �ƴ� ���� �Ҵ� ����� Ž��
        BindingFlags bind = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        MonoBehaviour[] monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        /*
        foreach(MonoBehaviour monoBehaviour in monoBehaviours)
        {
            Type type = monoBehaviour.GetType();

            // �ݷ���(�迭, ����Ʈ ��) Ư�� ���ǿ� �����ϴ� ��Ҹ� �������� �� ���,
            // Foreach �Ǵ� list.Find ��� �ټ� ������ ������ ���ľ� ��
            //List<FieldInfo> fields = new List<FieldInfo>(type.GetFields(bind));
            //fields.FindAll(null);

            // Linq ������ �� ����Ͽ� �̸� ����ȭ �� �� ����.
            // 1. Linq�� ���ǵ� Ȯ�� �޼��� �̿��ϴ� ���
            IEnumerable<FieldInfo> colorAttachedFields = type.GetFields(bind).Where(
                field => field.GetCustomAttribute<ColorAttribute>() != null);

            // 2. Linq�� ���� �������� ����� ������ Ȱ���ϴ� ���
            colorAttachedFields = from field in type.GetFields(bind)
                     where field.HasAttribute<ColorAttribute>()
                     select field;

            foreach (FieldInfo field in colorAttachedFields)
            {
                ColorAttribute att = field.GetCustomAttribute<ColorAttribute>();

                object value = field.GetValue(monoBehaviour);

                if(value is Renderer rend)
                {
                    rend.material.color = att.color;
                }
                else if(value is Graphic graph)
                {
                    graph.color = att.color;
                }
                else
                {
                    throw new Exception("����...");
                    //Debug.LogError("����...");
                }
            }
        }
        */

        // !����!
        foreach (MonoBehaviour monoBehaviour in monoBehaviours)
        {
            Type type = monoBehaviour.GetType();

            IEnumerable<FieldInfo> sizeAttachedFields = type.GetFields(bind).Where(
                field => field.GetCustomAttribute<SizeAttribute>() != null);

            foreach (FieldInfo field in sizeAttachedFields)
            {
                SizeAttribute att = field.GetCustomAttribute<SizeAttribute>();

                object value = field.GetValue(monoBehaviour);

                if (att.category != Category.RectScale)
                {
                    if (value is Transform trans)
                    {
                        if (att.category == Category.LocalScale)
                        {
                            trans.localScale = att.scale;
                        }
                    }
                }
                else if(att.category == Category.RectScale)
                {
                    if (value is RectTransform rectTrans)
                    {
                        rectTrans.localScale = att.scale;
                    }
                }
                else
                {
                    throw new Exception("����...");
                    //Debug.LogError("����...");
                }
            }
        }
    }
}

// Color�� ������ �� �ִ� ������Ʈ �Ǵ� ������Ʈ�� [Color]��� ��Ʈ����Ʈ�� �ٿ���
// ���� �����ϰ� ����

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ColorAttribute : Attribute
{
    public Color color;

    // Attribute�� �����ڿ����� ���ͷ� Ÿ���� �Ű������� �Ҵ��� ����
    public ColorAttribute(float r = 0f, float g = 0f, float b = 0f, float a = 1f)
    {
        color = new Color(r, g, b, a);
    }

    public ColorAttribute()
    {
        color = Color.black;
    }
}

public enum Category
{
    LocalScale,
    WorldScale,
    RectScale
}

[AttributeUsage(AttributeTargets.Field)]
public class SizeAttribute : Attribute
{
    public Vector3 scale;

    public Category category;

    public SizeAttribute(float x, float y, float z, Category category)
    {
        scale = new Vector3(x, y, z);
        this.category = category;
    }
}

public static class AttributeHelper
{
    // Ư�� attribute�� ������ �ִ� ���θ� Ȯ���ϰ� ���� �� �� Ȯ�� �޼���
    public static bool HasAttribute<T>(this MemberInfo info) where T : Attribute
    {
        return info.GetCustomAttributes(typeof(T), true).Length > 0;
    }
}