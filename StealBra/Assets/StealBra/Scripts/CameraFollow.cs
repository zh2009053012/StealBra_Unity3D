using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Vector2 MaxPos, MinPos;
	public Transform FollowTarget;
	public float MoveSpeed = 3;
	private Vector2 targetPos;
	private Vector2 selfPos;
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(null == FollowTarget)
			return;
		targetPos = new Vector2(FollowTarget.position.x, FollowTarget.position.y);
		selfPos = new Vector2(transform.position.x, transform.position.y);
//		if(FollowTarget.position.x - transform.position.x < 0)
//		{
//			targetPos = new Vector2(transform.position.x, FollowTarget.position.y);
//		}
		float x=targetPos.x, y=targetPos.y;
		x = x < MinPos.x ? MinPos.x : x;
		x = x > MaxPos.x ? MaxPos.x : x;
		y = y < MinPos.y ? MinPos.y : y;
		y = y > MaxPos.y ? MaxPos.y : y;
		targetPos = new Vector2(x, y);

		Vector2 result = Vector2.Lerp (selfPos, targetPos, MoveSpeed*Time.deltaTime);
		transform.position = new Vector3(result.x, result.y, transform.position.z);
	}
}
