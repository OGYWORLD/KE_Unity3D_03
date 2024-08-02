using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyProject.Skill;

namespace MyProject.State{

    public class IdleState : BaseState
    {
        public override void Enter()
        {
            player.stateStay = 1;
        }

        public override void Exit()
        {
            Debug.Log("��� ���� ����");
        }

        public override void Update()
        {
            base.Update();
            //player.text.text = $"{GetType().Name} : {player.stateStay:n0}";
        }
    }
}
