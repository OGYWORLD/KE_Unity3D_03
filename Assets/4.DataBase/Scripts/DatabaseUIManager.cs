using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject
{
    public class DatabaseUIManager : MonoBehaviour
    {
        public GameObject loginPannel;
        public GameObject infoPannel;
        public GameObject createPannel;

        public InputField emailInput;
        public InputField pwInput;

        public Button signUpButton;
        public Button loginButton;

        public Text infoText;
        public Text levelText;

        private UserData userData;

        public Button deleteButton;

        private void Awake()
        {
            loginButton.onClick.AddListener(LoginButtonClick);
            signUpButton.onClick.AddListener(OnCreate);
            deleteButton.onClick.AddListener(OnDeleteAccount);
        }

        public void LoginButtonClick()
        {
            DataBaseManager.Instance.Login(emailInput.text, pwInput.text, OnLoginSucces, OnLoginFailure); ;
        }

        public void OnLevelButtonClick()
        {
            DataBaseManager.Instance.LevelUp(userData, OnLevelSuccess);
        }

        private void OnLevelSuccess()
        {
            levelText.text = $"����: {userData.lv}";
        }

        private void OnLoginSucces(UserData data)
        {
            print("�α��� ����!");
            userData = data;

            loginPannel.SetActive(false);
            infoPannel.SetActive(true);

            StringBuilder sb = new StringBuilder();
            sb.Append($"�ȳ��ϼ���, {data.name}\n");
            sb.Append($"�̸���: , {data.email}\n");
            sb.Append($"����: , {data.lv}\n");
            sb.Append($"����: , {data.charClass}\n");
            sb.Append($"�λ�: , {data.profileText}\n");

            infoText.text = sb.ToString();

            //levelText.text = $"����: {data.lv}";
        }

        private void OnLoginFailure()
        {
            print("�α��� ���ФФ�");
        }

        private void OnCreate()
        {
            loginPannel.SetActive(false);
            createPannel.SetActive(true);
        }

        private void OnDeleteAccount()
        {
            DataBaseManager.Instance.DeleteAccount(SuccessDelete, FailDelete);
        }

        private void SuccessDelete()
        {
            loginPannel.SetActive(true);
            infoPannel.SetActive(false);
        }

        private void FailDelete()
        {
            print("���� ���� ����");
        }
        

    }

  
}
