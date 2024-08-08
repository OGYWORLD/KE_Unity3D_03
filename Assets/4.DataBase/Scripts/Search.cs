using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject{

    public class Search : MonoBehaviour
    {
        public Button toLoginButton;
        public InputField searchInput;
        public Dropdown dropdown;

        public Text resultText;

        public GameObject searchPannel;
        public GameObject loginPannel;

        private void Awake()
        {
            toLoginButton.onClick.AddListener(ToLogin);
            searchInput.onSubmit.AddListener(OnSearchInfo);
        }

        private void OnSearchInfo(string message)
        {
            DataBaseManager.Instance.Search(dropdown.value, message, SuccessSearch, FailSearch);
        }

        private void SuccessSearch(string result)
        {
            resultText.color = Color.black;

            resultText.text = "";

            resultText.text = result;
        }

        private void FailSearch()
        {
            resultText.color = Color.red;
            resultText.text = "검색결과가 없습니다.";
        }

        private void ToLogin()
        {
            searchPannel.SetActive(false);
            loginPannel.SetActive(true);
        }
    }
}
