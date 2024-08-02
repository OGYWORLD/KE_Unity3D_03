using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Skill{

    public class SkillContext : MonoBehaviour
    {
        public Player owner;

        // internal: ���� ����� �������� ���� ����
        // ���� ���� Ŭ���������� ���� �����ϳ�
        // ����Ƽ ������ ������ �� �����Ƿ� inspector���� ������ �� ��.
        internal List<SkillBehaviour> skills = new List<SkillBehaviour>();

        public SkillBehaviour currentSkill;

        public void AddSkill(SkillBehaviour skill)
        {
            skill.context = this;
            skills.Add(skill);
        }

        public void SetCurrentSkill(int index)
        {
            if(index >= skills.Count)
            {
                index = 0;
            }

            currentSkill?.Remove();
            currentSkill = skills[index];
            currentSkill?.Apply();
        }

        public void UseSkill()
        {
            currentSkill.Use();
        }
    }
}
