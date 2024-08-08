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

        private MySqlConnection conn; // mysql DB와 연결상태를 유지하는 객체

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

        // 로그인을 하려고 할 때, 로그인 쿼리를 날린 즉시 데이터가 오지 않을 수 있으므로
        // 로그인이 완료 되었을 때 호출될 함수를 파라미터로 함께 받아주도록 함.
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
                // 로그인 성공(email과 pw 값이 동시에 일치하는 행이 존재함)

                DataRow row = set.Tables[0].Rows[0];
                data = new UserData(row);

                print(data.email);

                successCallback?.Invoke(data);
            }
            else
            {
                // 로그인 실패
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

            // 업데이트 확인 함수
            int queryCount = cmd.ExecuteNonQuery(); // 쿼리에 영향받은 행의 개수를 반환

            if(queryCount > 0)
            {
                // 쿼리 정상적 수행
                data.lv = nextLevel;
                successCallback?.Invoke();
            }
            else
            {
                // 쿼리 수행 실패
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

                // 업데이트 확인 함수
                int queryCount = cmd.ExecuteNonQuery(); // 쿼리에 영향받은 행의 개수를 반환

                if (queryCount > 0)
                {
                    // 쿼리 정상적 수행
                    SuccessQuery?.Invoke();
                }
                else
                {
                    // 쿼리 수행 실패
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

                // 업데이트 확인 함수
                int queryCount = cmd.ExecuteNonQuery(); // 쿼리에 영향받은 행의 개수를 반환

                if (queryCount > 0)
                {
                    // 쿼리 정상적 수행
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
                    // 쿼리 수행 실패
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

            int queryCount = cmd.ExecuteNonQuery(); // 쿼리에 영향받은 행의 개수를 반환

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
                    sb.Append($"이메일: {ud.email}, 이름: {ud.name}, 레벨: {ud.lv}, 직업: {(CharClass)ud.charClass}, 인삿말: {ud.profileText}\n");
                }

                SuccessQuery?.Invoke(sb.ToString());
            }
            else
            {
                // 로그인 실패
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
                pwHash += $"{b:X2}"; // 16진수로 보간
                // == pwHash += b.ToString("X2");
                }   
            }
             */
            SHA256 sha256 = SHA256.Create();
            byte[] hashArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwd));
            foreach (byte b in hashArray)
            {
                pwHash += $"{b:X2}"; // 16진수로 보간
                // == pwHash += b.ToString("X2");
            }

            sha256.Dispose();

            return pwHash;
        }

    }
}
