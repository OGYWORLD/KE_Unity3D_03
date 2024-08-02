using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Skill{
	public class ArcaneShpere : SkillBehaviour
	{
		public GameObject projectilePrefab;
		private Transform spinner;

		public override void Apply()
		{
			// 양 옆으로 구체 2개 생성
			Instantiate(projectilePrefab, spinner);
			Instantiate(projectilePrefab, spinner);

			spinner.GetChild(0).localPosition = Vector3.right;
			spinner.GetChild(1).localPosition = Vector3.left;

			print("비전 구체 시전");
		}

		public override void Use()
		{
			print("비전 구체 스킬은 사용 효과가 없습니다.");
		}

		public override void Remove()
		{
			foreach(Transform proj in spinner)
            {
				Destroy(proj.gameObject);
            }

			print("비전 구체 취소");
		}

        private void Awake()
        {
            if(transform.childCount > 0)
            {
				spinner = transform.GetChild(0);
            }
			else
            {
				spinner = new GameObject("ArcaneShpere").transform;
            }
        }

        private void Update()
        {
			spinner.Rotate(Vector3.up * 180 * Time.deltaTime);
        }
    }
}
