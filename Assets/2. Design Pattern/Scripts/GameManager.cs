using MyProject;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // � �� �̱��� �������� ����� ������?
    // �� ������ ����غ��� �ȴ� : ���� å�� ��Ģ�� �����ϴ� �༮�ΰ�?
    public static GameManager Instance { get; private set; }
    public Light light;

    public float dayLength = 5; // �� ���� ����
    public bool isDay = true;

    // Observer Pattern
    // Ư�� �ӹ��� �����ϴ� ��ü(������)���� ���� ��ȭ
    // �Ǵ� Ư�� �̺�Ʈ�� ȣ�� ������ �߻��� ��
    // �ش� �̺�Ʈ ȣ���� �ʿ��� ��ü����
    // "���� ���� ���ϸ� �˷��ּ���." ��� ���(����) �س��� ������
    // ������ �����̴�.

    private List<Monster> monsters = new List<Monster>(); // �����ڵ�

    // C#�� event�� observer pattern ������ ����ȭ�� ������ ������� �����Ƿ�
    // event�� Ȱ���ϴ� �� �����ε� ������ ������ �����ߴٰ� �� �� ����
    public event Action<bool> onDayNightChange;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private float dayTemp;
    private void Update()
    {
        // 5�ʸ��� �� ���� �ٲ�� ����
        if(Time.time - dayTemp > dayLength)
        {
            dayTemp = Time.time;
            isDay = !isDay;
            light.gameObject.SetActive(isDay);

            // �������� �����ڵ鿡�� �޽����� ����
            //foreach(Monster monster in monsters)
            //{
            //    monster.OnDayNightChange(isDay);
            //}

            onDayNightChange?.Invoke(isDay);
        }
    }

    public void OnMonsterSpawn(Monster monster)
    {
        monsters.Add(monster);
        monster.OnDayNightChange(isDay);
    }

    public void OnMonsterDespaen(Monster monster)
    {
        monsters.Remove(monster);
    }
}
