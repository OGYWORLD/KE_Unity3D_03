using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeControllerTest : MonoBehaviour
{
    [Color(0, 1, 0, 1)]
    public new Renderer renderer;

    //[SerializeField, Color] �̷��Ե� ����
    [SerializeField]
    [Color(r:1, b: 0.5f)]
    private Graphic graphic;

    [Color]
    public float notRendererOrGraphic;

    // !����!
    [Size(3, 3, 3, Category.LocalScale)]
    public Transform localScale;

    [Size(2, 2, 2, Category.LocalScale)]
    public Transform worldScale;

    [Size(1, 1, 1, Category.RectScale)]
    public RectTransform rectScale;
}