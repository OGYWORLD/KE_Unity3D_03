using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace MyProject{

    public enum CharClass
    {
        none,
        Warrior,
        Wizard,
        Rogue,
    }

    public class UserData
    {
        private int uid;
        public int UID => uid;

        private string passwd;
        public string PW { get; set; }

        public string email;

        public string name;
        public int lv;
        public CharClass charClass;

        public string profileText;

        public UserData(int uid, string email, string passwd, string name, int lv, CharClass charClass, string profileText)
        {
            this.uid = uid;
            this.email = email;
            this.passwd = passwd;
            this.name = name;
            this.lv = lv;
            this.charClass = charClass;
            this.profileText = profileText;
        }

        public UserData(DataRow row) : this(
            int.Parse(row["uid"].ToString()),
            row["email"].ToString(),
            row["pw"].ToString(),
            row["name"].ToString(),
            int.Parse(row["lv"].ToString()),
            (CharClass)int.Parse(row["class"].ToString()),
            row["profile_text"].ToString()
            )
        { }

        public UserData(string email, string passwd, string name, int lv, CharClass charClass, string profileText)
        {
            this.email = email;
            this.PW = passwd;
            this.name = name;
            this.lv = lv;
            this.charClass = charClass;
            this.profileText = profileText;
        }


        public bool ComparePasswd(string passwd)
        {
            return this.passwd.Equals(passwd);
        }


    }
}
