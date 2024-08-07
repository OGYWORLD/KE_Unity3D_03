using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject{

    public class EditInfo : MonoBehaviour
    {
        public InputField emailInput;
        public InputField pwInput;
        public InputField pwCheckInput;

        public InputField userNameInput;
        public InputField profileInput;

        public Text pwCheckText;

        public Dropdown job;

        public Button okButton;
        public Button editButton;
        public Button cancleButton;

        private UserData userData;

        public Text creatResultText;

        private bool isPWRight = false;

        public GameObject infoPannel;
        public GameObject editPannel;

        public Text infoText;

        private void Awake()
        {
            okButton.onClick.AddListener(EditAccount);
            editButton.onClick.AddListener(InfoToEditPannel);
            cancleButton.onClick.AddListener(EditToInfoPannel);
        }

        private void Update()
        {
            CheckPW();
        }

        private void EditAccount()
        {
            if (isPWRight)
            {
                userData = new UserData(emailInput.text.ToString(), pwInput.text.ToString(), userNameInput.text.ToString(),
                    1, (CharClass)(job.value + 1), profileInput.text.ToString());

                DataBaseManager.Instance.EditAccount(userData, SuccessCreateAccount, FailCreateAccount);
            }
        }

        public void SuccessCreateAccount(UserData data)
        {
            infoText.text = "";

            print("!!!");
            StringBuilder sb = new StringBuilder();
            sb.Append($"�ȳ��ϼ���, {data.name}\n");
            sb.Append($"�̸���: , {data.email}\n");
            sb.Append($"����: , {data.lv}\n");
            sb.Append($"����: , {data.charClass}\n");
            sb.Append($"�λ�: , {data.profileText}\n");

            infoText.text = sb.ToString();

            creatResultText.color = Color.green;
            creatResultText.text = "���� ������ �����߽��ϴ�. ����� ���� ȭ������ ���ư��ϴ�.";

            StartCoroutine(DelayReturnLogin());
        }

        public void FailCreateAccount()
        {
            creatResultText.color = Color.red;
            creatResultText.text = "���� ������ �����߽��ϴ�.";
        }

        private void CheckPW()
        {
            if (pwCheckInput.text.Length == 0 && pwCheckText.text.Length == 0)
            {
                pwCheckText.text = "";
            }
            else if (pwCheckInput.text.CompareTo(pwInput.text) == 0)
            {
                isPWRight = true;
                pwCheckText.color = Color.green;
                pwCheckText.text = "��й�ȣ�� ��ġ�մϴ�.";
            }
            else
            {
                isPWRight = false;
                pwCheckText.color = Color.red;
                pwCheckText.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
            }
        }

        private void InfoToEditPannel()
        {
            editPannel.SetActive(true);
            infoPannel.SetActive(false);
        }

        private void EditToInfoPannel()
        {
            editPannel.SetActive(false);
            infoPannel.SetActive(true);
        }

        private IEnumerator DelayReturnLogin()
        {
            yield return new WaitForSeconds(2f);

            editPannel.SetActive(false);
            infoPannel.SetActive(true);
        }
    }
}
