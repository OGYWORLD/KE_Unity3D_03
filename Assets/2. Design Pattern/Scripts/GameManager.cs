using MyProject;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 어떤 걸 싱글톤 패턴으로 만들면 좋은가?
    // 한 가지만 고민해보면 된다 : 단일 책임 원칙에 부합하는 녀석인가?
    public static GameManager Instance { get; private set; }
    public Light light;

    public float dayLength = 5; // 밤 낮의 길이
    public bool isDay = true;

    // Observer Pattern
    // 특정 임무를 수행하는 객체(옵저버)에게 상태 변화
    // 또는 특정 이벤트의 호출 조건이 발생할 시
    // 해당 이벤트 호출이 필요한 객체들이
    // "나도 상태 변하면 알려주세요." 라고 등록(구독) 해놓는 형태의
    // 디자인 패턴이다.

    private List<Monster> monsters = new List<Monster>(); // 구독자들

    // C#의 event는 observer pattern 구현에 최적화된 구조로 만들어져 있으므로
    // event를 활용하는 것 만으로도 옵저버 패턴을 적용했다고 볼 수 있음
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
        // 5초마다 밤 낮이 바뀌도록 연산
        if(Time.time - dayTemp > dayLength)
        {
            dayTemp = Time.time;
            isDay = !isDay;
            light.gameObject.SetActive(isDay);

            // 옵저버가 구독자들에게 메시지를 보냄
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
