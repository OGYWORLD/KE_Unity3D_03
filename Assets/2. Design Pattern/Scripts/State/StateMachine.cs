using MyProject.Skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.State{
    
    // 플레이어와 같은 오브젝트에 있으면서 플레이어의 상태를 제어해주는 컴포넌트
    [RequireComponent(typeof(Player))]
    public class StateMachine : MonoBehaviour
    {
        public MoveState moveState;
        public IdleState idleState;
        public Player player;

        public IState currentState;

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        private void Start()
        {
            moveState = new MoveState();
            idleState = new IdleState();

            moveState.Initialize(player);
            idleState.Initialize(player);

            currentState = idleState;
            idleState.Enter();
        }

        public void Transition(IState state)
        {
            if(currentState == state)
            {
                return;
            }

            currentState.Exit();
            currentState = state;
            currentState.Enter();
        }

        private void Update()
        {
            currentState.Update();
        }
    }
}
