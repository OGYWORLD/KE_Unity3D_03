using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Skill{
public class SkillBehaviour : MonoBehaviour
{
        internal SkillContext context;

        public virtual void Apply()
		{
			Debug.Log($"{GetType().Name} skill applied");
		}

		public virtual void Use()
		{
			Debug.Log($"{GetType().Name} skill used");
		}

		public virtual void Remove()
		{
			Debug.Log($"{GetType().Name} skill removed");
		}
	}
}
