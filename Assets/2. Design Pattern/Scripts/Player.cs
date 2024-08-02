using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyProject.State;

namespace MyProject.Skill
{
    // 상태 패턴 구현을 위해, 현재 움직이는 중인지 아니면 멈춰있는지 판단하고 싶음
    public class Player : MonoBehaviour
    {
        public enum State
        {
            Idle,
            Move,
            //Jump,
            //Attack
        }

        private float mouseX = 0f;

        private CharacterController cc;
        public float moveSpeed = 10;
        public TextMeshPro text;

        public State currentState; // 0이면 멈춰있는 상태, 1이면 움직이는 상태.
                                   // 추후 확장이 되면 2면 공격. 3이면 점프 등등...

        public float stateStay; // 현재 상태에 머문 시간.
        public float sumTotalTime; // 누적 이동시간.

        public Transform shotPoint; // 스킬 시전 시, 투사체가 생성될 자리
        private SkillContext skillContext;

        private StateMachine stateMachine;

        private void Awake()
        {
            cc = GetComponent<CharacterController>();

            stateMachine = GetComponent<StateMachine>();
            skillContext = GetComponentInChildren<SkillContext>();
            SkillBehaviour[] skills = skillContext.GetComponentsInChildren<SkillBehaviour>();
        
            foreach(SkillBehaviour sk in skills)
            {
                skillContext.AddSkill(sk);
            }

            skillContext.SetCurrentSkill(0);
        }

        private void Start()
        {
            //currentState = State.Idle;
        }

        private void Update()
        {
            Move();
            //StateUpdate();

            // 오른쪽 버튼 누르면 스킬 사용
            if(Input.GetButtonDown("Fire1"))
            {
                skillContext.UseSkill();
            }

            // 스킬 바꾸기
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                skillContext.SetCurrentSkill(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                skillContext.SetCurrentSkill(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                skillContext.SetCurrentSkill(2);
            }
        }

        public void Move()
        {
            // 캐릭터 이동
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 moveDir = new Vector3(x, 0, z);
            //cc.Move(moveDir * moveSpeed * Time.deltaTime);
            cc.transform.localPosition += new Vector3(x * moveSpeed * Time.deltaTime, 0f, z * moveSpeed * Time.deltaTime);

            // 상태 전이
            if (moveDir.magnitude < 0.1f)
            {
                stateMachine.Transition(stateMachine.idleState);
            }
            else
            {
                stateMachine.Transition(stateMachine.moveState);
            }
            /*
            if (moveDir.magnitude < 0.1f)
            {
                TransitionState(State.Idle);
            }
            else
            {
                TransitionState(State.Move);
            }
            */


            // 마우스 회전
            mouseX += Input.GetAxis("Mouse X") * Time.deltaTime * 1000f;
            gameObject.transform.rotation = Quaternion.Euler(0, mouseX, 0);
        }

        // 상태 전이
        public void TransitionState(State nextState)
        {
            if (currentState != nextState)
            {
                // exit
                switch (currentState)
                {
                    case State.Idle:
                        print("대기 상태 종료");
                        break;
                    case State.Move:
                        print("이동 상태 종료");
                        break;
                    default:
                        break;
                }

                // enter
                switch (nextState)
                {
                    case State.Idle:
                        print("대기 상태 시작");
                        break;
                    case State.Move:
                        print("이동 상태 시작");
                        break;
                    default:
                        break;
                }

                currentState = nextState;

                stateStay = 0;
            }
        }

        public void StateUpdate()
        {
            // 현재 상태에 따른 행동 정의
            switch (currentState)
            {
                case State.Idle:
                    text.text = $"{State.Idle} state: {stateStay.ToString("n0")}";
                    break;
                case State.Move:
                    text.text = $"{State.Move} state: {stateStay.ToString("n0")}";
                    break;
            }

            stateStay += Time.deltaTime;
        }
    }

}