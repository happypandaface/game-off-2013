using UnityEngine;
using System.Collections;

public enum Alliance { ENEMY, PLAYER };

//enemy bullet
public class Bullet : MonoBehaviour 
{
	#region vars
	public Alliance alliance = Alliance.ENEMY; //"player" OR "enemy"

	public float damage = 10.0f;
	public float speed = 12.0f; //speed (units / s)
	public Vector3 direction = new Vector3( 0.0f, 0.0f, 0.0f ); //unit vector indicating direction

	float t = 0.0f;
	#endregion


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		t += Time.deltaTime;
		if ( t > 10.0f )
		{
			Destroy ( this.gameObject );
		}

		this.gameObject.transform.position = 
			new Vector3( 
			    this.gameObject.transform.position.x + direction.x * speed * Time.deltaTime,
		        this.gameObject.transform.position.y + direction.y * speed * Time.deltaTime,
		        this.gameObject.transform.position.z 
			);

		//Collision testing

		//Abstraction inefficiency
		float x = this.gameObject.transform.position.x;
		float y = this.gameObject.transform.position.y;
		Vector2 center = new Vector2( x, y );
		float r = this.gameObject.GetComponent<CircleCollider2D>().radius;

		#region enemy bullets
		if ( alliance == Alliance.ENEMY )
		{
			Collider2D player = Physics2D.OverlapCircle( center, r, 1 << LayerMask.NameToLayer( "Player" ) );
			if ( player != null ) //hit player
			{
				player.gameObject.GetComponent<PlayerCore>().OnHit( 10.0f );
				Destroy ( this.gameObject );
			}
			else if ( Physics2D.OverlapCircle( center, r, 1 << LayerMask.NameToLayer( "Wall" ) ) != null ) //can't hit self, allies, or bullets
			{
				Destroy ( this.gameObject );
			}
		}
		#endregion
		#region player bullets
		else if ( alliance == Alliance.PLAYER )
		{
			Collider2D[] hits = Physics2D.OverlapCircleAll( center, r, 1 << LayerMask.NameToLayer( "Enemy" ) );
			if ( hits.Length > 0 ) //hit enemy (use common core stats)
			{
				foreach ( Collider2D c in hits )
				{
					//c.gameObject.GetComponent<>().;
				}
				Destroy ( this.gameObject );
			}
			else if ( Physics2D.OverlapCircle( center, r, 1 << LayerMask.NameToLayer( "Wall" ) ) != null ) //can't hit self or bullets
			{
				Destroy ( this.gameObject );
			}
		}
		#endregion

	}
}
