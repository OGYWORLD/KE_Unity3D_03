using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyProject.State;

namespace MyProject.Skill
{
    // ���� ���� ������ ����, ���� �����̴� ������ �ƴϸ� �����ִ��� �Ǵ��ϰ� ����
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

        public State currentState; // 0�̸� �����ִ� ����, 1�̸� �����̴� ����.
                                   // ���� Ȯ���� �Ǹ� 2�� ����. 3�̸� ���� ���...

        public float stateStay; // ���� ���¿� �ӹ� �ð�.
        public float sumTotalTime; // ���� �̵��ð�.

        public Transform shotPoint; // ��ų ���� ��, ����ü�� ������ �ڸ�
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

            // ������ ��ư ������ ��ų ���
            if(Input.GetButtonDown("Fire1"))
            {
                skillContext.UseSkill();
            }

            // ��ų �ٲٱ�
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
            // ĳ���� �̵�
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 moveDir = new Vector3(x, 0, z);
            //cc.Move(moveDir * moveSpeed * Time.deltaTime);
            cc.transform.localPosition += new Vector3(x * moveSpeed * Time.deltaTime, 0f, z * moveSpeed * Time.deltaTime);

            // ���� ����
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


            // ���콺 ȸ��
            mouseX += Input.GetAxis("Mouse X") * Time.deltaTime * 1000f;
            gameObject.transform.rotation = Quaternion.Euler(0, mouseX, 0);
        }

        // ���� ����
        public void TransitionState(State nextState)
        {
            if (currentState != nextState)
            {
                // exit
                switch (currentState)
                {
                    case State.Idle:
                        print("��� ���� ����");
                        break;
                    case State.Move:
                        print("�̵� ���� ����");
                        break;
                    default:
                        break;
                }

                // enter
                switch (nextState)
                {
                    case State.Idle:
                        print("��� ���� ����");
                        break;
                    case State.Move:
                        print("�̵� ���� ����");
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
            // ���� ���¿� ���� �ൿ ����
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