using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
//[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(Collider2D))]
public class CharacterData : MonoBehaviour {

	[SerializeField]
	private float JumpForce = 300;
	[SerializeField]
	private float MoveSpeed = 3;
	[SerializeField]
	private float ClimbSpeed = 2;
	[SerializeField]
	private Transform Foot;
	[SerializeField]
	private float Radius=0.01f;
	[SerializeField]
	private Camera m_camera;
	[SerializeField]
	private GameObject m_mesh;

	private Rigidbody2D m_rigid;
	private Animator m_animator;
	private float move=0;
	private bool isGrounded=false;
	[SerializeField]
	private bool isClimb = false;
	private Transform m_trans;
	private Collider2D m_collider2D;
	// Use this for initialization
	void Start () {
		m_rigid = GetComponent<Rigidbody2D>();
		m_animator = GetComponentInChildren<Animator>();
		m_collider2D = GetComponent<Collider2D>();
		if(Foot == null)
		{
			Foot = this.transform.Find("Foot");
		}
		m_trans = this.transform;
		if(m_camera == null)
		{
			m_camera = Camera.main;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Collider2D[] colliders = Physics2D.OverlapCircleAll(Foot.transform.position, Radius);
		isGrounded = false;
		if(colliders != null)
		{
			for(int i=0; i < colliders.Length; i++)
			{
				if(colliders[i].gameObject.Equals(this.gameObject) || 
				   colliders[i].gameObject.tag.Equals("bra") ||
					colliders[i].gameObject.tag.Equals("underpants") ||
					colliders[i].gameObject.tag.Equals("ladder"))
				{
					continue;
				}
				else 
				{
					isGrounded = true;
				}
			}
		}
		m_animator.SetBool ("jump", !isGrounded && !isClimb);
	}
	public void Climb(bool jump, float verticalInput){
		if(isGrounded && Mathf.Abs(verticalInput) > 0.001f){
			Collider2D[] colliders = Physics2D.OverlapCircleAll(Foot.transform.position, Radius);
			isClimb = false;
			for(int i=0; i < colliders.Length; i++)
			{
				if(colliders[i].gameObject.tag.Equals("ladder"))
				{
					isClimb = true;
				}
			}
		}
		if(isClimb)
		{
			if(jump){
				m_rigid.gravityScale = 1;
				m_rigid.velocity = Vector2.one;
				m_rigid.AddForce (new Vector2(0, JumpForce));
				m_animator.SetBool ("Jump", true);
				isClimb = false;
			}else{
				m_rigid.velocity = new Vector2(0, ClimbSpeed*verticalInput);
				m_rigid.gravityScale = 0;
			}
		}else{
			m_rigid.gravityScale = 1;
		}

		m_animator.SetBool ("climb", isClimb);
	}
	public void Move(bool jump, float horizontalInput, float verticalInput)
	{
		move = horizontalInput*MoveSpeed;
		//flip
		if(!isClimb && move > 0 )
		{
			m_mesh.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
		}
		else if(!isClimb && move < 0)
		{
			m_mesh.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
		}

		m_rigid.velocity = new Vector2(move, m_rigid.velocity.y);

		if(jump && isGrounded)
		{
			m_rigid.AddForce (new Vector2(0, JumpForce));
			m_animator.SetBool ("Jump", true);
			return;
		}

		if(Mathf.Abs(move) > 0.0f)
		{
			m_animator.SetBool("run", true);
		}else{
			m_animator.SetBool("run", false);
		}
		//climb

		Collider2D[] colliders = Physics2D.OverlapCircleAll(Foot.transform.position, Radius);
		bool isOnClimb = false;
		for(int i=0; i < colliders.Length; i++)
		{
			if(colliders[i].gameObject.tag.Equals("ladder"))
			{
				isOnClimb = true;
			}
		}

		if(isClimb)
		{
			if(jump){
				m_rigid.gravityScale = 1;
				m_rigid.AddForce (new Vector2(0, JumpForce));
				m_animator.SetBool ("Jump", true);
				isClimb = false;
			}else{
				m_rigid.velocity = new Vector2(0, ClimbSpeed*verticalInput);
				m_rigid.gravityScale = 0;
				m_animator.SetBool ("Jump", false);
			}
		}else{
			m_rigid.gravityScale = 1;
		}

		m_animator.SetBool ("climb", isClimb);
	}
}
