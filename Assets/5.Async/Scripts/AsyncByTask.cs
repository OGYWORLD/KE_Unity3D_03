using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject{

    public class AsyncByTask : MonoBehaviour
    {
        // �ܹ��Ÿ� ����� �ʹ�.

        int bread = 0; // �� ����
        int patty = 0; // ��Ƽ ����
        int pickle = 0; // ��Ŭ ����
        int lettuce = 0; // �����

        FoodMakerTask breadMaker = new FoodMakerTask();
        FoodMakerTask pattyMaker = new FoodMakerTask();
        FoodMakerTask pickleMaker = new FoodMakerTask();
        FoodMakerTask lettuceMaker = new FoodMakerTask();

        public Text text;

        private void Start()
        {
            breadMaker.StartCook(2);
            pattyMaker.StartCook(2);
            pickleMaker.StartCook(8);
            lettuceMaker.StartCook(4);

            StartCoroutine(CheckHamberger());
        }

        private void Update()
        {
            bread = breadMaker.amount;
            patty = pattyMaker.amount;
            pickle = pickleMaker.amount;
            lettuce = lettuceMaker.amount;

            text.text = $"�� ���� : {bread}, ��Ƽ ���� : {patty}, ��Ŭ ���� : {pickle}, ����� ���� : {lettuce}";
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
            print($"�ܹ��Ű� ����� �����ϴ�. �ҿ�ð� : {Time.time}");
        }
    }

    public class FoodMakerTask
    {
        public int amount = 0;

        public void StartCook(int count)
        {
            Task<int> cookTask = Cook(count);
            //cookTask.ContinueWith((o) => { });
            cookTask.ContinueWith(task => { amount = task.Result; });
        }

        private async Task<int> Cook(int count)
        {
            int result = 0;
            for(int i = 0; i < count; i++)
            {
                int time = Random.Range(1000, 3000);
                await Task.Delay(time);
                result++;
            }

            return result;

            /*
            while(true)
            {
                int time = Random.Range(1000, 3000);
                await Task.Delay(time);
                amount++;
            }
            */
        }
    }
}
