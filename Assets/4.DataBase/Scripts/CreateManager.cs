using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject{

    public class CreateManager : MonoBehaviour
    {
        public InputField emailInput;
        public InputField pwInput;
        public InputField pwCheckInput;

        public InputField userNameInput;
        public InputField profileInput;

        public Text pwCheckText;

        public Dropdown job;

        public Button okButton;

        private UserData userData;

        public Text creatResultText;

        private bool isPWRight = false;

        public GameObject createPannel;
        public GameObject loginPannel;

        private void Awake()
        {
            okButton.onClick.AddListener(CreateAccount);
        }

        private void Update()
        {
            CheckPW();
        }

        private void CreateAccount()
        {
            if(isPWRight)
            {
                userData = new UserData(emailInput.text.ToString(), pwInput.text.ToString(), userNameInput.text.ToString(),
                    1, (CharClass)(job.value + 1), profileInput.text.ToString());

                DataBaseManager.Instance.CreateAccount(userData, SuccessCreateAccount, FailCreateAccount);
            }
        }

        public void SuccessCreateAccount()
        {
            creatResultText.color = Color.green;
            creatResultText.text = "회원가입에 성공했습니다. 잠시후 로그인 화면으로 돌아갑니다.";

            StartCoroutine(DelayReturnLogin());
        }

        public void FailCreateAccount()
        {
            creatResultText.color = Color.red;
            creatResultText.text = "회원가입에 실패했습니다.";
        }

        private void CheckPW()
        {
            if(pwCheckInput.text.Length == 0 && pwCheckText.text.Length == 0)
            {
                pwCheckText.text = "";
            }
            else if(pwCheckInput.text.CompareTo(pwInput.text) == 0)
            {
                isPWRight = true;
                pwCheckText.color = Color.green;
                pwCheckText.text = "비밀번호가 일치합니다.";
            }
            else
            {
                isPWRight = false;
                pwCheckText.color = Color.red;
                pwCheckText.text = "비밀번호가 일치하지 않습니다.";
            }
        }

        private IEnumerator DelayReturnLogin()
        {
            yield return new WaitForSeconds(2f);

            createPannel.SetActive(false);
            loginPannel.SetActive(true);
        }
    }
}
