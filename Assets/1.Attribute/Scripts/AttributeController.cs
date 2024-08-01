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
        // ColorAttribute를 가진 필드를 찾는다.
        // BindingFlags : public 이거나 private 상관 없이 static이 아닌 동적 할당 멤버만 탐색
        BindingFlags bind = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        MonoBehaviour[] monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        /*
        foreach(MonoBehaviour monoBehaviour in monoBehaviours)
        {
            Type type = monoBehaviour.GetType();

            // 콜렉션(배열, 리스트 등) 특정 조건에 부합하는 요소만 가져오라 할 경우,
            // Foreach 또는 list.Find 등에서 다소 복잡한 절차를 거쳐야 함
            //List<FieldInfo> fields = new List<FieldInfo>(type.GetFields(bind));
            //fields.FindAll(null);

            // Linq 문법을 잘 사용하여 이를 간소화 할 수 있음.
            // 1. Linq에 정의된 확장 메서드 이용하는 방법
            IEnumerable<FieldInfo> colorAttachedFields = type.GetFields(bind).Where(
                field => field.GetCustomAttribute<ColorAttribute>() != null);

            // 2. Linq를 통해 쿼리문과 비슷한 문법을 활용하는 방법
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
                    throw new Exception("저런...");
                    //Debug.LogError("저런...");
                }
            }
        }
        */

        // !과제!
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
                    throw new Exception("저런...");
                    //Debug.LogError("저런...");
                }
            }
        }
    }
}

// Color를 조절할 수 있는 컴포넌트 또는 오브젝트에 [Color]라는 어트리뷰트를 붙여서
// 색을 설정하고 싶음

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ColorAttribute : Attribute
{
    public Color color;

    // Attribute의 생성자에서는 리터럴 타입의 매개변수만 할당이 가능
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
    // 특정 attribute를 가지고 있는 여부만 확인하고 싶을 때 쓸 확장 메서드
    public static bool HasAttribute<T>(this MemberInfo info) where T : Attribute
    {
        return info.GetCustomAttributes(typeof(T), true).Length > 0;
    }
}