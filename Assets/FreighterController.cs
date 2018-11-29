using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreighterController : EnemyController {


    public GameObject Gun;
    public Transform GunPivot;
	
	protected override void Fire()
	{
		Vector2 direction = new Vector2(-Mathf.Cos(Mathf.Deg2Rad * Gun.transform.eulerAngles.z), -Mathf.Sin(Mathf.Deg2Rad * Gun.transform.eulerAngles.z));

		GameObject p = Instantiate(Projectile);
		p.transform.position = transform.position + new Vector3(direction.x, direction.y, 0) * 2f;
		p.GetComponent<Rigidbody2D>().velocity = direction * ProjectileSpeed;
        
		Destroy(p, 5);
		FireAllowance--;
	}

	protected override void MaintainPlayerDistance()
	{
        Vector2 desiredVelocity = new Vector2(Mathf.Cos(Time.time) * 4, Mathf.Sin(Time.time) * 4);
        body.velocity = body.velocity* 0.8f + desiredVelocity* 0.2f;

        float angle = Mathf.Atan2(body.velocity.y, body.velocity.x) * Mathf.Rad2Deg + 180;
        body.rotation = angle;
    }

	protected override void AimAtPlayer()
	{
		Vector2 playerPos = player.transform.position;
		float angle = Mathf.Atan2(playerPos.y - transform.position.y, playerPos.x - transform.position.x) * Mathf.Rad2Deg + 180;

        float currentAngle = Gun.transform.eulerAngles.z;

        Gun.transform.RotateAround(GunPivot.position, Vector3.forward, angle - currentAngle);
	}
}
