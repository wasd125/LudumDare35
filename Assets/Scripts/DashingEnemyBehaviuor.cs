using UnityEngine;
using System.Collections;

public class DashingEnemyBehaviour : IEnemyBehaviour
{

    public Enemy MyController;
    public enum EnumDashingEnemyState { IdleLeft, DashRight, IdleRight, DashLeft }
    public EnumDashingEnemyState State;

    public float waitForNextState = 0.3f;
    public float startPosX;
    public float moveToX;

    public void DoUpdate()
    {
        waitForNextState -= Time.deltaTime;
        if (waitForNextState > 0) return;

        switch (State)
        {
            case EnumDashingEnemyState.IdleLeft:
                IdleLeft();
                break;
            case EnumDashingEnemyState.DashLeft:
                DashLeft();
                break;
            case EnumDashingEnemyState.IdleRight:
                IdleRight();
                break;
            case EnumDashingEnemyState.DashRight:
                DashRight();
                break;
        }
    }

    void IdleLeft()
    {
        MyController.rend.sprite = MyController.NormalSprite;
        MyController.gameObject.tag = "Obstacle";
        MyController.bb2d.enabled = true;
        MyController.bc2d.enabled = false;
        MyController.gc2d.enabled = false;
        moveToX = startPosX + MyController.MoveDistance;
        State = EnumDashingEnemyState.DashRight;
        waitForNextState = MyController.transformTime;
    }
    void IdleRight()
    {
        MyController.rend.sprite = MyController.NormalSprite;
        MyController.gameObject.tag = "Obstacle";
        MyController.bb2d.enabled = true;
        MyController.bc2d.enabled = false;
        MyController.gc2d.enabled = false;
        moveToX = MyController.transform.position.x - MyController.MoveDistance;
        State = EnumDashingEnemyState.DashLeft;
        waitForNextState = MyController.transformTime;
    }
    void DashLeft()
    {
        if (MyController.transform.position.x > moveToX)
        {
            MyController.transform.position = Vector3.MoveTowards(MyController.transform.position, new Vector3(moveToX, MyController.transform.position.y, 1), MyController.rollingSpeed * Time.deltaTime);
            MyController.bb2d.enabled = false;
            MyController.bc2d.enabled = true;
            MyController.gc2d.enabled = true;
            MyController.transform.eulerAngles = new Vector3(0, 0, 90);
            MyController.rend.sprite = MyController.AttackSprite;
            MyController.gameObject.tag = "DamageSource";
        }
        else
        {
            State = EnumDashingEnemyState.IdleLeft;
            MyController.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
    void DashRight()
    {
        if (MyController.transform.position.x < moveToX)
        {
            MyController.transform.position = Vector3.MoveTowards(MyController.transform.position, new Vector3(moveToX, MyController.transform.position.y, 1), MyController.rollingSpeed * Time.deltaTime);
            MyController.bb2d.enabled = false;
            MyController.bc2d.enabled = true;
            MyController.gc2d.enabled = true;
            MyController.transform.eulerAngles = new Vector3(0, 0, 270);
            MyController.rend.sprite = MyController.AttackSprite;
            MyController.gameObject.tag = "DamageSource";

        }
        else
        {
            State = EnumDashingEnemyState.IdleRight;
            MyController.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void InitBehaviour(Enemy enemy)
    {
        MyController = enemy;
        startPosX = enemy.transform.position.x;
    }
}
