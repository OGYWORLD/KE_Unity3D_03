using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.State{

    public class MoveState : BaseState
    {
        public override void Enter()
        {
            player.stateStay = 0;
        }

        public override void Exit()
        {
            Debug.Log("이동 상태 종료");
        }

        public override void Update()
        {
            //base.Update();
            player.text.text = $"{GetType().Name} : {player.sumTotalTime:n1}";
            player.sumTotalTime += Time.deltaTime;
        }
    }
}
