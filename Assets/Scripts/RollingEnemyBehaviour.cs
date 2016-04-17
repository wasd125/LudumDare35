using UnityEngine;
using System.Collections;

public class RollingEnemyBehaviour : IEnemyBehaviour {

	public Enemy MyController;
	public enum EnumRollingEnemyState { IdleLeft, RollRight, IdleRight, RollLeft }
	public EnumRollingEnemyState State;

	public float waitForNextState = 0.3f;	
	public float startPosX;
	public float moveToX;

	public void DoUpdate()
	{
		waitForNextState -= Time.deltaTime;
		if (waitForNextState > 0) return;

		switch (State)
		{
		case EnumRollingEnemyState.IdleLeft:
			IdleLeft();
			break;
		case EnumRollingEnemyState.RollLeft:
			RollLeft();
			break;
		case EnumRollingEnemyState.IdleRight:
			IdleRight();
			break;
		case EnumRollingEnemyState.RollRight:
			RollRight();
			break;
		}
	}

	void IdleLeft()
	{	
		MyController.rend.sprite = MyController.AttackSprite;
		MyController.gameObject.tag = "DamageSource";
		MyController.gc2d.enabled = true;
		MyController.bc2d.enabled = true;
		MyController.cc2d.enabled = false;
		moveToX = startPosX + MyController.MoveDistance;
		State = EnumRollingEnemyState.RollRight;
		waitForNextState = 0.5f;
	}
	void IdleRight()
	{	
		MyController.rend.sprite = MyController.AttackSprite;
		MyController.gameObject.tag = "DamageSource";
		MyController.gc2d.enabled = true;
		MyController.bc2d.enabled = true;
		MyController.cc2d.enabled = false;
		moveToX = MyController.transform.position.x - MyController.MoveDistance;
		State = EnumRollingEnemyState.RollLeft;
		waitForNextState = 0.5f;
	}
	void RollLeft()
	{
		if (MyController.transform.position.x > moveToX)
		{
			MyController.transform.position = Vector3.MoveTowards(MyController.transform.position, new Vector3(moveToX, MyController.transform.position.y, 1), MyController.rollingSpeed * Time.deltaTime);
			MyController.gc2d.enabled = false;
			MyController.bc2d.enabled = false;
			MyController.cc2d.enabled = true;
			MyController.rend.sprite = MyController.NormalSprite;
			MyController.gameObject.tag = "Obstacle";
		}
		else
		{
			State = EnumRollingEnemyState.IdleLeft;
		}
	}
	void RollRight()
	{
		if (MyController.transform.position.x < moveToX)
		{
			MyController.transform.position = Vector3.MoveTowards(MyController.transform.position, new Vector3(moveToX, MyController.transform.position.y, 1), MyController.rollingSpeed * Time.deltaTime);
			MyController.gc2d.enabled = false;
			MyController.bc2d.enabled = false;
			MyController.cc2d.enabled = true;
			MyController.rend.sprite = MyController.NormalSprite;
			MyController.gameObject.tag = "Obstacle";

		}
		else
		{
			State = EnumRollingEnemyState.IdleRight;
		}
	}

	public void InitBehaviour(Enemy enemy)
	{
		MyController = enemy;
		startPosX = enemy.transform.position.x;
	}
}
