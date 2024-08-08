using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySqlConnector;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace MyProject
{
    public class DataBaseManager : MonoBehaviour
    {
        public static DataBaseManager Instance { get; private set; }

        private string dbName = "game";
        private string tableName = "users";

        public string rootPasswd = "";

        private MySqlConnection conn; // mysql DB�� ������¸� �����ϴ� ��ü

        private UserData data;

        public enum SearchInfo
        {
            email,
            name,
            lv
        }

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DBConnect();
        }

        public void DBConnect()
        {
            string config = $"server=43.203.209.38;port=3306;database={dbName};uid=root;pwd={rootPasswd};charset=utf8;";

            conn = new MySqlConnection(config);
            conn.Open();
            //print(conn.State);

        }

        // �α����� �Ϸ��� �� ��, �α��� ������ ���� ��� �����Ͱ� ���� ���� �� �����Ƿ�
        // �α����� �Ϸ� �Ǿ��� �� ȣ��� �Լ��� �Ķ���ͷ� �Բ� �޾��ֵ��� ��.
        public void Login(string email, string passwd, Action<UserData> successCallback, Action failureCallback)
        {
            passwd = HashFunctionSHA256(passwd);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = $"SELECT * FROM {tableName} WHERE email='{email}' AND pw='{passwd}'";

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);

            DataSet set = new DataSet();

            dataAdapter.Fill(set);

            bool isLoginSuccess = set.Tables.Count > 0 && set.Tables[0].Rows.Count > 0;

            if(isLoginSuccess)
            {
                // �α��� ����(email�� pw ���� ���ÿ� ��ġ�ϴ� ���� ������)

                DataRow row = set.Tables[0].Rows[0];
                data = new UserData(row);

                print(data.email);

                successCallback?.Invoke(data);
            }
            else
            {
                // �α��� ����
                failureCallback.Invoke();
            }
        }

        public void LevelUp(UserData data, Action successCallback)
        {
            int level = data.lv;
            int nextLevel = level + 1;

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"UPDATE {tableName} SET lv={nextLevel} WHERE uid={data.UID}";

            // ������Ʈ Ȯ�� �Լ�
            int queryCount = cmd.ExecuteNonQuery(); // ������ ������� ���� ������ ��ȯ

            if(queryCount > 0)
            {
                // ���� ������ ����
                data.lv = nextLevel;
                successCallback?.Invoke();
            }
            else
            {
                // ���� ���� ����
            }
        }

        public void CreateAccount(UserData data, Action SuccessQuery, Action FailQuery)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();

                data.PW = HashFunctionSHA256(data.PW);

                cmd.Connection = conn;
                cmd.CommandText = $"INSERT INTO {tableName}(email,pw,name,lv,class,profile_text)" +
                    $" VALUES('{data.email}','{data.PW}','{data.name}',{data.lv},{(int)data.charClass},'{data.profileText}')";

                // ������Ʈ Ȯ�� �Լ�
                int queryCount = cmd.ExecuteNonQuery(); // ������ ������� ���� ������ ��ȯ

                if (queryCount > 0)
                {
                    // ���� ������ ����
                    SuccessQuery?.Invoke();
                }
                else
                {
                    // ���� ���� ����
                    FailQuery?.Invoke();
                }
            }
            catch(Exception e)
            {
                FailQuery?.Invoke();
                print(e.Message);
            }
           
        }

        public void EditAccount(UserData data, Action<UserData> SuccessQuery, Action FailQuery)
        {
            try
            {
                data.PW = HashFunctionSHA256(data.PW);

                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = conn;
                cmd.CommandText = $"UPDATE {tableName}" +
                    $" SET email='{data.email}',pw='{data.PW}',name='{data.name}',lv={data.lv},class={(int)data.charClass},profile_text='{data.profileText}'" +
                    $"WHERE uid={this.data.UID}";

                // ������Ʈ Ȯ�� �Լ�
                int queryCount = cmd.ExecuteNonQuery(); // ������ ������� ���� ������ ��ȯ

                if (queryCount > 0)
                {
                    // ���� ������ ����
                    cmd.CommandText = $"SELECT * FROM {tableName} WHERE email='{data.email}' AND pw='{data.PW}'";

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);

                    DataSet set = new DataSet();

                    dataAdapter.Fill(set);

                    DataRow row = set.Tables[0].Rows[0];
                    this.data = new UserData(row);

                    SuccessQuery?.Invoke(this.data);
                }
                else
                {
                    // ���� ���� ����
                    FailQuery?.Invoke();
                }
            }
            catch (Exception e)
            {
                FailQuery?.Invoke();
            }

        }

        public void DeleteAccount(Action SuccessQuery, Action FailQuery)
        {
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = $"DELETE FROM {tableName} WHERE uid={this.data.UID}";

            int queryCount = cmd.ExecuteNonQuery(); // ������ ������� ���� ������ ��ȯ

            if (queryCount > 0)
            {
                SuccessQuery?.Invoke();
            }
            else
            {
                FailQuery?.Invoke();
            }
        }

        public void Search(int category, object target, Action<string> SuccessQuery, Action FailQuery)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = $"SELECT * FROM {tableName} WHERE {(SearchInfo)category} LIKE '{target}%'";

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);

            DataSet set = new DataSet();

            dataAdapter.Fill(set);

            bool isLoginSuccess = set.Tables.Count > 0 && set.Tables[0].Rows.Count > 0;

            if (isLoginSuccess)
            {
                StringBuilder sb = new StringBuilder();

                for(int i = 0; i < set.Tables[0].Rows.Count; i++)
                {
                    DataRow row = set.Tables[0].Rows[i];
                    UserData ud = new UserData(row);
                    sb.Append($"�̸���: {ud.email}, �̸�: {ud.name}, ����: {ud.lv}, ����: {(CharClass)ud.charClass}, �λ�: {ud.profileText}\n");
                }

                SuccessQuery?.Invoke(sb.ToString());
            }
            else
            {
                // �α��� ����
                FailQuery.Invoke();
            }
        }

        private string HashFunctionSHA256(string passwd)
        {
            string pwHash = "";

            /*
             using(SHA256 sha256 = SHA256.Create())
             {
                byte[] hashArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwd));
                foreach(byte b in hashArray)
                {
                pwHash += $"{b:X2}"; // 16������ ����
                // == pwHash += b.ToString("X2");
                }   
            }
             */
            SHA256 sha256 = SHA256.Create();
            byte[] hashArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwd));
            foreach (byte b in hashArray)
            {
                pwHash += $"{b:X2}"; // 16������ ����
                // == pwHash += b.ToString("X2");
            }

            sha256.Dispose();

            return pwHash;
        }

    }
}
