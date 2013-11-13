using UnityEngine;
using System.Collections;

public class CustomController : MonoBehaviour 
{

	#region vars
	public int facing = 3; //direction for sprites (0: Right, 1: Up, 2: Left, 3: Down)

	[HideInInspector]
	public bool move_enabled = true; //for disabling motion

	public float speed = 2.0f; //unity units per second (we're using Tile Size pixels per unit (64) )
	#endregion

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector2 move_vec = new Vector2( 0.0f, 0.0f );

		#region input parsing
		if ( move_enabled )
		{
			if ( Input.GetKey( KeyCode.A ) )
			{
				move_vec.x -= 1;
			}
			if ( Input.GetKey ( KeyCode.D ) )
			{
				move_vec.x += 1;
			}
			if ( Input.GetKey ( KeyCode.S ) )
			{
				move_vec.y -= 1;
			}
			if ( Input.GetKey ( KeyCode.W ) )
			{
				move_vec.y += 1;
			}
		}
		#endregion

		#region facing parsing
		//mouse controls dir?
		//ASSUMPTION: player will always be centered!
		//otherwise we need to cast x/y -> screen x y.

		float angle = Mathf.Rad2Deg * Mathf.Atan2 ( Input.mousePosition.y - Screen.height / 2, Input.mousePosition.x - Screen.width / 2 );

		//pull into range?
		if ( angle < 0.0f )
		{
			angle += 360.0f;
		}
		else if ( angle > 360.0f )
		{
			angle -= 360.0f;
		}

		//Based on angle, get facing.
		if ( (angle >= 0.0f && angle <= 45.0f) || (angle >= 315.0f && angle <= 360.0f) )
		{
			facing = 0;
		}
		else if ( angle >= 45.0f && angle <= 135.0f )
		{
			facing = 1;
		}
		else if ( angle >= 135.0f && angle <= 225.0f )
		{
			facing = 2;
		}
		else if ( angle >= 225.0f && angle <= 315.0f )
		{
			facing = 3;
		}

		//Debug.Log ( angle + " : " + facing ); //DEBUG LINE

		#endregion

		if ( move_vec.sqrMagnitude > 0.0f ) //sqrMagnitude is more efficient.
		{
			//Move.
			//Get direction from vec.

			//abstraction inefficiency
			float x = this.gameObject.transform.position.x;
			float y = this.gameObject.transform.position.y;
			float z = this.gameObject.transform.position.z;

			//Scale to speed
			move_vec = move_vec * speed * Time.deltaTime;

			//Actually move
			this.gameObject.transform.position = new Vector3( x + move_vec.x, y + move_vec.y, z );

			//Force Camera to lock onto player
			Camera.main.transform.position = new Vector3( this.gameObject.transform.position.x,
			                                              this.gameObject.transform.position.y,
			                                              Camera.main.transform.position.z );
		}
		else
		{
			//Idle
		}
	}
}
