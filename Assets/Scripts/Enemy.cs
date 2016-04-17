using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    public SpriteRenderer rend { get; private set; }
    public Animator anim { get; private set; }
    public Rigidbody2D rb2d { get; private set; }
    public BoxCollider2D bc2d { get; private set; }
    public BoxCollider2D gc2d { get; private set; }
    public BoxCollider2D bb2d { get; private set; }
    public CircleCollider2D cc2d { get; private set; }
    private IEnemyBehaviour myBehaviour;

    public enum EnumEnemyType { JumpingEnemy, WalkingEnemy, RollingEnemy, DashingEnemy, DiggingEnemy, Elevator }
    public EnumEnemyType EnemyType;

    public float jumpForce;
    public float walkingSpeed;
    public float rollingSpeed;
    public float transformTime;

    public Sprite AttackSprite;
    public Sprite NormalSprite;

    public float MoveDistance = 3f;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        bb2d = GetComponent<BoxCollider2D>();
        gc2d = GetComponent<BoxCollider2D>();
        cc2d = GetComponent<CircleCollider2D>();

        NormalSprite = rend.sprite;

        switch (EnemyType)
        {
            case EnumEnemyType.JumpingEnemy:
                myBehaviour = new JumpingEnemyBehaviour();
                break;
            case EnumEnemyType.WalkingEnemy:
                myBehaviour = new WalkingEnemy();
                break;
            case EnumEnemyType.RollingEnemy:
                myBehaviour = new RollingEnemyBehaviour();
                break;
            case EnumEnemyType.DashingEnemy:
                myBehaviour = new DashingEnemyBehaviour();
                break;
			case EnumEnemyType.Elevator:
				myBehaviour = new ElevatingEnemyBehaviour();
				break;
                //case EnumEnemyType.DiggingEnemy:
                //	myBehaviour = new DiggingEnemyBehaviour();
                //	break;
        }

        myBehaviour.InitBehaviour(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.ControllerState == InputManager.EnumControllerState.Menu) return;

        myBehaviour.DoUpdate();
    }
}
