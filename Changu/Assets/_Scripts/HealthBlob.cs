using UnityEngine;
using System.Collections;

public class HealthBlob : MonoBehaviour 
{

	#region vars
	float t = 0.0f;
	#endregion

	// Use this for initialization
	void Start () 
	{
		t = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		t += Time.deltaTime;

		//blink when close to expiration

		//Lifetime expiration
		if ( t > 15.0f )
		{
			Destroy ( this.gameObject );
		}

		//Collision detection
		//Abstraction inefficiency
		float x = this.gameObject.transform.position.x;
		float y = this.gameObject.transform.position.y;
		Vector2 center = new Vector2( x, y );
		float r = this.gameObject.GetComponent<CircleCollider2D>().radius;
		
		Collider2D player = Physics2D.OverlapCircle( center, r, 1 << LayerMask.NameToLayer( "Player" ) );

		//TODO: do an intro timer? Delay before pickup is enabled.
		if ( player != null )
		{
			PlayerCore pl = player.gameObject.GetComponent<PlayerCore>();
			pl.hp = Mathf.Min ( pl.hp + 25.0f, pl.max_hp );
			Destroy ( this.gameObject );
		}
	}
}
