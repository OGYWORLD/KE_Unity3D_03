using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MyProject{

    public class AsyncTest : MonoBehaviour
    {
        //async void Start()
        //{
        //await WaitRandom(); �Ұ���
        //WaitRandom();
        //print("WaitRandom ȣ����");

        //await Wait3Seconds();
        //print("wait3Seconds ȣ����");

        //int a = await WaitRandomAndReturn();
        //print($"{a} �� WaitRandomAndReturn ȣ��");
        //}

        private void Start()
        {
            //async�� �ƴѵ��� Wait3Seconds()�� ���� �Ŀ� ���� �ؾ� �� ���.
            Wait3Seconds().ContinueWith((Task result) => { 
                if(result.IsCanceled || result.IsFaulted)
                {
                    print("task ����");
                }
                else if(result.IsCompleted)
                {
                    print("task ����");
                }
                print("3�� ��"); 
            });
        }

        //1. void�� ��ȯ�ϴ� async �Լ� : �Լ� ��ü�� ��� �����̳�,
        // �ٸ� �Լ����� ��� ���� �������� ȣ���� �Ұ����ϴ�.
        async void WaitRandom()
        {
            print($"������ {Time.time}");
            await Task.Delay(Random.Range(1000, 2000));
            print($"������� {Time.time}");
        }

        //2. Task�� ��ȯ�ϴ� async �Լ�: �Լ� ��ü�� ��� �����̸�,
        // �ٸ� ��� ���� �Լ����� ����������� ȣ���� ����
        // return�� ��� �˾Ƽ� ���μ����� Task�� ���� ��ȯ��.
        async Task Wait3Seconds()
        {
            print($"3�� ��� ���� {Time.time}");
            await Task.Delay(3000);
            print($"3�� ��� ���� {Time.time}");
        }

        //3. Task<T>�� ��ȯ�ϴ� async �Լ�: ��� ���� �Լ��� �� Task�� ��ȯ�ϴ� �Լ��� ������,
        // return�� �־�߸� ��
        async Task<int> WaitRandomAndReturn()
        {
            int delay = Random.Range(1000, 2000);
            print($"{(float)delay / 1000} �� ��� ���� {Time.time}");

            await Task.Delay(delay);

            print($"{(float)delay / 1000} �� ��� ���� {Time.time}");

            return delay;
        }

    }
}
