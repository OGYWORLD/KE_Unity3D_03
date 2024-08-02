using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Skill{
	public class Fireball : SkillBehaviour
	{
		public FireBallProjectile projectile; // 투사체 프리팹
		public float projectileSpeed = 5f;

		public override void Apply()
		{
			base.Apply();
		}

		public override void Use()
		{
			base.Use();

			Transform shotPoint = context.owner.shotPoint;

			var obj = Instantiate(projectile, shotPoint.position, shotPoint.rotation);

			obj.SetProjectile(projectileSpeed);

			Destroy(obj, 3);
		}

		public override void Remove()
		{
			base.Remove();
		}
	}
}
