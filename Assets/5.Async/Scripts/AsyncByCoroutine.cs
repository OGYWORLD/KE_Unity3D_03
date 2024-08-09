using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject{

    public class AsyncByCoroutine : MonoBehaviour
    {
        // 햄버거를 만들고 싶다.
        
        int bread = 0; // 빵 개수
        int patty = 0; // 패티 개수
        int pickle = 0; // 피클 개수
        int lettuce = 0; // 양상추

        public Text text;

        private FoodMakerThread breadMaker = new FoodMakerThread();
        private FoodMakerThread patMaker = new FoodMakerThread();
        private FoodMakerThread pickleMaker = new FoodMakerThread();
        private FoodMakerThread lettuceMaker = new FoodMakerThread();

        private void Start()
        {
            breadMaker.StartCook();
            patMaker.StartCook();
            pickleMaker.StartCook();
            lettuceMaker.StartCook();

            StartCoroutine(CheckHamberger());
        }

        private void Update()
        {
            bread = breadMaker.amount;
            patty = patMaker.amount;
            pickle = pickleMaker.amount;
            lettuce = lettuceMaker.amount;

            text.text = $"빵 개수 : {bread}, 패티 개수 : {patty}, 피클 개수 : {pickle}, 양상추 개수 : {lettuce}";
        }

        IEnumerator CheckHamberger()
        {
            yield return new WaitUntil(HambergerReady);

            MakeHamberger();
        }

        private bool HambergerReady()
        {
            return bread >= 2 & patty >= 2 && pickle >= 8 && lettuce >= 4;
        }

        private void MakeHamberger()
        {
            bread -= 2;
            patty -= 2;
            pickle -= 8;
            lettuce -= 4;

            print($"햄버거가 만들어 졌습니다. 소요시간 : {Time.time}");
        }

    }

    public class FoodMakerThread
    {
        public int amount = 0; // 식재료 양

        private System.Random rand = new System.Random();

        public void StartCook()
        {
            Thread cookThread = new Thread(Cook);
            cookThread.IsBackground = true;
            cookThread.Start();
        }

        private void Cook()
        {
            while(true)
            {
                int time = rand.Next(1000, 3000); // 1초에서 3초 사이 랜덤 대기
                Thread.Sleep(time);

                amount++;
            }
        }
    }
}
