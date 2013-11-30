using UnityEngine;
using System.Collections;

public class CustomController : MonoBehaviour 
{

	#region vars
	public int facing = 3; //direction for sprites (0: Right, 1: Up, 2: Left, 3: Down)

	[HideInInspector]
	public bool move_enabled = true; //for disabling motion

	public float speed = 2.0f; //unity units per second (we're using Tile Size pixels per unit (64) )

	BoxCollider2D my_collider;

	public GameObject eye_obj;
	public Sprite[] eye_sprites = new Sprite[4]; 

	public GameObject hair_obj;
	public Sprite[] hair_sprites = new Sprite[4];

	#endregion

	// Use this for initialization
	void Start () 
	{
		my_collider = this.gameObject.GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector2 move_vec = new Vector2( 0.0f, 0.0f );

		#region input parsing
		if ( move_enabled )
		{
			if ( Input.GetKey( KeyCode.A ) || Input.GetKey( KeyCode.LeftArrow ) )
			{
				move_vec.x -= 1;
			}
			if ( Input.GetKey ( KeyCode.D ) || Input.GetKey ( KeyCode.RightArrow ) )
			{
				move_vec.x += 1;
			}
			if ( Input.GetKey ( KeyCode.S ) || Input.GetKey ( KeyCode.DownArrow ) )
			{
				move_vec.y -= 1;
			}
			if ( Input.GetKey ( KeyCode.W ) || Input.GetKey ( KeyCode.UpArrow ) )
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
			#region animate
			//Animate
			if ( facing == 0 ){ this.gameObject.GetComponent<Animator>().Play( "WalkRight" ); }
			else if ( facing == 1 ){ this.gameObject.GetComponent<Animator>().Play( "WalkUp" ); }
			else if ( facing == 2 ){ this.gameObject.GetComponent<Animator>().Play( "WalkLeft" ); }
			else if ( facing == 3 ){ this.gameObject.GetComponent<Animator>().Play( "WalkDown" ); }
			#endregion

			//Move.
			//Get direction from vec.

			//Scale to speed
			move_vec = move_vec * speed * Time.deltaTime;

			Move ( move_vec );

			//Force Camera to lock onto player
			Camera.main.transform.position = new Vector3( 
				this.gameObject.transform.position.x,
			    this.gameObject.transform.position.y,
			    Camera.main.transform.position.z 
			);
		}
		else
		{
			//Idle
			#region animate
			//this.gameObject.GetComponent<Animator>().Play("Idle");
			//Animate
			if ( facing == 0 ){ this.gameObject.GetComponent<Animator>().Play( "IdleRight" ); }
			else if ( facing == 1 ){ this.gameObject.GetComponent<Animator>().Play( "IdleUp" ); }
			else if ( facing == 2 ){ this.gameObject.GetComponent<Animator>().Play( "IdleLeft" ); }
			else if ( facing == 3 ){ this.gameObject.GetComponent<Animator>().Play( "IdleDown" ); }
			#endregion

		}

		#region animate
		//hair and eyes
		hair_obj.GetComponent<SpriteRenderer>().sprite = hair_sprites[ facing ];
		//eye_obj.GetComponent<SpriteRenderer>().sprite = eye_sprites[ facing ];
		#endregion
	}

	public void Move( Vector3 move_vec )
	{
		//Moves the player by move_vec (pos = pos + vec),
		//but also respects collisions.

		#region Collision Detection
		//Get the distance from center to edge of box collider
		float x_diff = my_collider.size.x / 2.0f + 0.01f;
		float y_diff = my_collider.size.y / 2.0f + 0.01f;
		
		//proper sign (+/-)
		if ( move_vec.x < 0 )
		{
			x_diff = x_diff * -1.0f;
		}
		if ( move_vec.y < 0 )
		{
			y_diff = y_diff * -1.0f;
		}
		
		//Collision Detection
		//TODO: isolate by axis
		bool blockedx = BlockF ( new Vector3( x_diff + move_vec.x, 0.0f, 0.0f ) ) || 
			BlockF ( new Vector3( x_diff + move_vec.x, y_diff, 0.0f ) ) ||
				BlockF ( new Vector3( x_diff + move_vec.x, y_diff * -1.0f, 0.0f) );
		
		bool blockedy = BlockF ( new Vector3( 0.0f, y_diff + move_vec.y, 0.0f ) ) ||
			BlockF ( new Vector3( x_diff, y_diff + move_vec.y, 0.0f ) ) ||
				BlockF ( new Vector3( x_diff * -1.0f, y_diff + move_vec.y, 0.0f ) );
		#endregion
		
		if ( ! blockedx )
		{
			//abstraction inefficiency
			float x = this.gameObject.transform.position.x;
			float y = this.gameObject.transform.position.y;
			float z = this.gameObject.transform.position.z;
			
			//Actually move
			this.gameObject.transform.position = new Vector3( x + move_vec.x, y, z );
		}
		if ( ! blockedy)
		{
			//abstraction inefficiency
			float x = this.gameObject.transform.position.x;
			float y = this.gameObject.transform.position.y;
			float z = this.gameObject.transform.position.z;
			
			//Actually move
			this.gameObject.transform.position = new Vector3( x, y + move_vec.y, z );
		}
	}

	bool BlockF( Vector3 vec )
	{
		//Utility Wall Block Check Function
		//For collision detection. (triple raycast)
		/*
		Debug.DrawLine( 
			this.gameObject.transform.position, 
		    this.gameObject.transform.position + vec,
		    new Color( 1.0f, 0.0f, 0.0f, 1.0f ),
		    0.033f
		);
		*/

		Vector3 offset = new Vector3( my_collider.center.x, my_collider.center.y, 0.0f );
		return  Physics2D.Linecast( 
					this.gameObject.transform.position + offset, 
					this.gameObject.transform.position + offset + vec, 
					1 << LayerMask.NameToLayer( "Wall" ) 
		); 
	}
}
