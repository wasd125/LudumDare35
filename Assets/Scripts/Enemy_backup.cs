using UnityEngine;
using System.Collections;

public class Enemy_backup : MonoBehaviour {

    public SpriteRenderer rend { get; private set; }
    public Animator anim { get; private set; }
    public Rigidbody2D rb2d { get; private set; }
    private IEnemyBehaviour myBehaviour;

    public enum EnumEnemyType { JumpingEnemy,WalkingEnemy }
    public EnumEnemyType EnemyType;

    public float jumpForce;
    public float walkingSpeed;

    public Sprite AttackSprite;
    public Sprite NormalSprite;

    // Use this for initialization
    void Start () {
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        NormalSprite = rend.sprite;

        switch (EnemyType)
        {
            case EnumEnemyType.JumpingEnemy:
                myBehaviour = new JumpingEnemyBehaviour();
                break;
            case EnumEnemyType.WalkingEnemy:
                myBehaviour = new WalkingEnemy();
                break;
        }

        //myBehaviour.InitBehaviour(this);
	}
	
	// Update is called once per frame
	void Update () {
        myBehaviour.DoUpdate();
	}
}
