using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform FollowTarget;
	public float MoveSpeed = 3;
	private Vector2 targetPos;
	private Vector2 selfPos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		targetPos = new Vector2(FollowTarget.position.x, FollowTarget.position.y);
		selfPos = new Vector2(transform.position.x, transform.position.y);
		if(FollowTarget.position.x - transform.position.x < 0)
		{
			targetPos = new Vector2(transform.position.x, FollowTarget.position.y);
		}

		Vector2 result = Vector2.Lerp (selfPos, targetPos, MoveSpeed*Time.deltaTime);
		transform.position = new Vector3(result.x, result.y, transform.position.z);
	}
}
