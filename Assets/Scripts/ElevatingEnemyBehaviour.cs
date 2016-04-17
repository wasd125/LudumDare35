using UnityEngine;
using System.Collections;

public class ElevatingEnemyBehaviour : IEnemyBehaviour {

	public Enemy MyController;
	public enum EnumElevatingEnemyState {Idle,Elevating,Falling,Staying }
	public EnumElevatingEnemyState State;

	public float waitForNextState = 0.3f;
	public float startPosY;
	public float moveToY;

	public void DoUpdate()
	{
		waitForNextState -= Time.deltaTime;
		if (waitForNextState > 0) return;
		switch (State)
		{
		case EnumElevatingEnemyState.Idle:
			Idle();
			break;
		case EnumElevatingEnemyState.Elevating:
			Elevating();
			break;
		case EnumElevatingEnemyState.Staying:
			Staying();
			break;
		case EnumElevatingEnemyState.Falling:
			Falling();
			break;
		
		}
	}

	void Idle()
	{       
		MyController.rend.sprite = MyController.NormalSprite;
		MyController.gameObject.tag = "Obstacle";
		MyController.bb2d.enabled = true;
		MyController.bc2d.enabled = false;
		MyController.gc2d.enabled = false;
		moveToY = startPosY + MyController.MoveDistance;
		State = EnumElevatingEnemyState.Elevating;
		waitForNextState = MyController.transformTime;
	}
	void Elevating()
	{
		if (MyController.transform.position.y < moveToY)
		{
			MyController.transform.position = Vector3.MoveTowards(MyController.transform.position, new Vector3(MyController.transform.position.x, moveToY, 1), MyController.rollingSpeed * Time.deltaTime);
			MyController.bb2d.enabled = true;
			MyController.bc2d.enabled = false;
			MyController.gc2d.enabled = false;
		}
		else
		{
			State = EnumElevatingEnemyState.Staying;
			MyController.transform.eulerAngles = new Vector3(0, 0, 0);
		}
	}

	void Staying()
	{
		MyController.rend.sprite = MyController.NormalSprite;
		MyController.gameObject.tag = "Obstacle";
		MyController.bb2d.enabled = true;
		MyController.bc2d.enabled = false;
		MyController.gc2d.enabled = false;
		State = EnumElevatingEnemyState.Falling;
		moveToY = MyController.transform.position.y - MyController.MoveDistance;
		waitForNextState = MyController.transformTime;
	}

	void Falling()
	{
		if (MyController.transform.position.y > moveToY)
		{
			MyController.transform.position = Vector3.MoveTowards(MyController.transform.position, new Vector3(MyController.transform.position.x, moveToY, 1), MyController.rollingSpeed * Time.deltaTime);
			MyController.rend.sprite = MyController.AttackSprite;
			MyController.gameObject.tag = "DamageSource";
			MyController.bb2d.enabled = false;
			MyController.bc2d.enabled = true;
			MyController.gc2d.enabled = true;
			MyController.transform.eulerAngles = new Vector3(0, 0, 180);
		} 
		else {
			State = EnumElevatingEnemyState.Idle;
			MyController.transform.eulerAngles = new Vector3(0, 0, 0);
		}
	}


	public void InitBehaviour(Enemy enemy)
	{
		MyController = enemy;
		startPosY = enemy.transform.position.y;
	}


}