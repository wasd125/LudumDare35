using UnityEngine;
using System.Collections;

public class WalkingEnemy : IEnemyBehaviour {

    public Enemy MyController;
    public enum EnumWalkingEnemyState { IdleLeft, WalkRight, IdleRight, WalkLeft }
    public EnumWalkingEnemyState State;

    public float waitForNextState = 0.3f;

    public float WalkDistance = 3f;
    public float startPosX;
    public float moveToX;

    public void DoUpdate()
    {
        waitForNextState -= Time.deltaTime;
        if (waitForNextState > 0) return;

        Debug.Log(State);

        switch (State)
        {
            case EnumWalkingEnemyState.IdleLeft:
                IdleLeft();
                break;
            case EnumWalkingEnemyState.WalkLeft:
                WalkLeft();
                break;
            case EnumWalkingEnemyState.IdleRight:
                IdleRight();
                break;
            case EnumWalkingEnemyState.WalkRight:
                WalkRight();
                break;
        }
    }

    void IdleLeft()
    {
        State = EnumWalkingEnemyState.WalkRight;
        moveToX = startPosX + WalkDistance;
        waitForNextState = 0.5f;
    }
    void IdleRight()
    {
        State = EnumWalkingEnemyState.WalkLeft;
        moveToX = MyController.transform.position.x - WalkDistance;
        waitForNextState = 0.5f;
    }
    void WalkLeft()
    {
        if (MyController.transform.position.x > moveToX)
        {
            MyController.transform.position = Vector3.MoveTowards(MyController.transform.position, new Vector3(moveToX, MyController.transform.position.y, 1), MyController.walkingSpeed * Time.deltaTime);
        }
        else
        {
            State = EnumWalkingEnemyState.IdleLeft;
        }
    }
    void WalkRight()
    {
        if (MyController.transform.position.x < moveToX)
        {
            MyController.transform.position = Vector3.MoveTowards(MyController.transform.position, new Vector3(moveToX, MyController.transform.position.y, 1), MyController.walkingSpeed * Time.deltaTime);
        }
        else
        {
            State = EnumWalkingEnemyState.IdleRight;
        }
    }

    public void InitBehaviour(Enemy enemy)
    {
        MyController = enemy;
        startPosX = enemy.transform.position.x;
    }
}
