using UnityEngine;
using System.Collections;
using System;

public class JumpingEnemyBehaviour : IEnemyBehaviour {

    public Enemy MyController;
    public enum EnumJumpingEnemyState {Idle,Jumping,Falling,Landing }
    public EnumJumpingEnemyState State;

    public float waitForNextState = 0.3f;

    public void DoUpdate()
    {
        waitForNextState -= Time.deltaTime;
        if (waitForNextState > 0) return;
        switch (State)
        {
            case EnumJumpingEnemyState.Idle:
                Idle();
                break;
            case EnumJumpingEnemyState.Jumping:
                Jumping();
                break;
            case EnumJumpingEnemyState.Falling:
                Falling();
                break;
            case EnumJumpingEnemyState.Landing:
                Landing();
                break;
        }
    }

    void Idle()
    {       
        State = EnumJumpingEnemyState.Jumping;
        waitForNextState = 0.5f;
    }
    void Jumping()
    {
        MyController.rb2d.velocity = new Vector2(MyController.rb2d.velocity.x, MyController.jumpForce);
        waitForNextState = 0.5f;
        State = EnumJumpingEnemyState.Falling;
    }
    void Falling()
    {
        if (MyController.rb2d.velocity.y < 0)
        {
            MyController.rend.sprite = MyController.AttackSprite;
            MyController.gameObject.tag = "DamageSource";
            State = EnumJumpingEnemyState.Landing;
            MyController.transform.Rotate(new Vector3(0, 0, 180));
        }
    }
    void Landing()
    {
        if (MyController.rb2d.velocity.y >= 0)
        {
            MyController.rend.sprite = MyController.NormalSprite;
            MyController.gameObject.tag = "Obstacle";
            MyController.transform.Rotate(new Vector3(0, 0, 180));
            State = EnumJumpingEnemyState.Idle;
            
        }
    }

    public void InitBehaviour(Enemy enemy)
    {
        MyController = enemy;
    }


}
