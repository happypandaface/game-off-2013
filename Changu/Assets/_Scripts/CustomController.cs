using UnityEngine;
using System.Collections;

public class CustomController : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector2 move_vec = new Vector2( 0.0f, 0.0f );

		#region input parsing
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
		#endregion

		if ( move_vec.sqrMagnitude > 0.0f )
		{
			//Move.
			//Get direction from vec.

			//abstraction inefficiency
			float x = this.gameObject.transform.position.x;
			float y = this.gameObject.transform.position.y;
			float z = this.gameObject.transform.position.z;

			//Scale to frame speed
			move_vec = move_vec * Time.deltaTime;

			//Actually move
			this.gameObject.transform.position = new Vector3( x + move_vec.x, y + move_vec.y, z );
		}
		else
		{
			//Idle
		}
	}
}
