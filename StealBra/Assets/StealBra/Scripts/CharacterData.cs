using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(Collider2D))]
public class CharacterData : MonoBehaviour {

	[SerializeField]
	private float JumpForce = 300;
	[SerializeField]
	private float MoveSpeed = 20;
	[SerializeField]
	private Transform Foot;
	[SerializeField]
	private float Radius=0.01f;
	[SerializeField]
	private Camera m_camera;

	private Rigidbody2D m_rigid;
	private Animator m_animator;
	private float move=0;
	private bool isGrounded=false;
	private Transform m_trans;
	private float m_cameraWidth;
	private float m_limitX;
	private Collider2D m_collider2D;
	// Use this for initialization
	void Start () {
		m_rigid = GetComponent<Rigidbody2D>();
		m_animator = GetComponent<Animator>();
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
	
		m_cameraWidth = m_camera.orthographicSize*m_camera.aspect-m_collider2D.bounds.size.x*0.5f;
	}

	void LimitX()
	{
		m_limitX = m_camera.transform.position.x - m_cameraWidth;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Collider2D[] colliders = Physics2D.OverlapCircleAll(Foot.transform.position, Radius);
		isGrounded = false;
		if(colliders != null)
		{
			for(int i=0; i < colliders.Length; i++)
			{
				if(colliders[i].gameObject.Equals(this.gameObject))
				{
					continue;
				}
				else
				{
					isGrounded = true;
				}
			}
		}
		m_animator.SetBool ("Jump", !isGrounded);
		m_animator.SetFloat ("YSpeed", m_rigid.velocity.y);
	}

	public void Move(bool jump, float horizontalInput)
	{
		if(jump && isGrounded)
		{
			m_rigid.AddForce (new Vector2(0, JumpForce));
			m_animator.SetBool ("Jump", true);
		}
		move = horizontalInput*MoveSpeed;
		//flip
		if(move > 0 )
		{
			m_trans.localScale = new Vector3( Mathf.Abs(m_trans.localScale.x),
			                                 m_trans.localScale.y,
			                                 m_trans.localScale.z);
		}
		else if(move < 0)
		{
			m_trans.localScale = new Vector3( -Mathf.Abs(m_trans.localScale.x),
			                                 m_trans.localScale.y,
			                                 m_trans.localScale.z);
		}
		//
		LimitX();
		if(m_rigid.transform.position.x <= m_limitX && move < 0)
		{
			m_rigid.velocity = new Vector2(0, m_rigid.velocity.y);
		}
		else
		{
			m_rigid.velocity = new Vector2(move, m_rigid.velocity.y);
		}
		m_animator.SetFloat("MoveSpeed", Mathf.Abs(move));
	}
}
