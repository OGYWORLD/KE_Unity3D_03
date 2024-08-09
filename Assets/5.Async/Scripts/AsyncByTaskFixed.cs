using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MyProject{

    public class AsyncByTaskFixed : MonoBehaviour
    {
        // async: 함수 앞에 붙는 키워드로, 해당 함수가 대기 가능(비동기) 함수라는 의미가 됨.
        // await: async(비동기) 함수 내에서 사용되며, 함수 내의 대기 가능 요소(Task 등)이 완료 될 때까지 대기하도록 함.
        // 멀티 쓰레딩을 수행할 때처럼 새로운 쓰레드를 생성하는 것이 아닌, Task를 생성하여 수행하므로 단일 쓰레드에서 수행이 가능함.
        async private void Start()
        {
            print("햄버거 제작이 시작됨");

            Task breadTask = GetFood("빵", 2);
            Task pickleTask = GetFood("피클", 8);
            Task pattyTask = GetFood("패티", 2);
            Task lettuceTask = GetFood("양상추", 4);

            await Task.WhenAll(breadTask, pattyTask, pickleTask, lettuceTask);
            //.ContinueWith(result => { });

            print("햄버거가 완성됨");
        }

        async Task GetFood(string name, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                await Task.Delay(Random.Range(1000, 3000));
                print($"{name} {i}개 완성됨");
            }
        }


    }
}
