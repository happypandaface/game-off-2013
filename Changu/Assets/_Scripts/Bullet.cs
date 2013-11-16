using UnityEngine;
using System.Collections;

//enemy bullet
public class Bullet : MonoBehaviour 
{
	#region vars
	string alliance = "enemy"; //"player" OR "enemy"

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

		this.gameObject.transform.position = new Vector3( this.gameObject.transform.position.x + 1.0f * Time.deltaTime,
		                                                  this.gameObject.transform.position.y,
		                                                  this.gameObject.transform.position.z );
	}

	void OnCollisionEnter2D ( Collision2D collision )
	{
		if ( alliance == "enemy" )
		{
			if ( collision.collider.tag == "Player" ) //hit player
			{
				collision.collider.gameObject.GetComponent<PlayerCore>().hp -= 10.0f; //TODO: call the right thing.
				Destroy ( this.gameObject );
			}
			else if ( collision.collider.tag != "Enemy" && collision.collider.tag != "Bullet" ) //can't hit self, allies, or bullets
			{
				Destroy ( this.gameObject );
			}
		}
		else if ( alliance == "player" )
		{
			if ( collision.collider.tag == "Enemy" ) //hit enemy (use common core stats)
			{
				//collider.gameObject.GetComponent<PlayerCore>().hp -= 10.0f; //TODO: call the right thing.
				Destroy ( this.gameObject );
			}
			else if ( collision.collider.tag != "Player" && collision.collider.tag != "Bullet" ) //can't hit self or bullets
			{
				Destroy ( this.gameObject );
			}
		}
	}
}
