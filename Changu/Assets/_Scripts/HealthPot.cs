using UnityEngine;
using System.Collections;

public class HealthPot : MonoBehaviour 
{

	#region vars
	public GameObject health_drop; //health drop
	#endregion


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Abstraction inefficiency
		float x = this.gameObject.transform.position.x;
		float y = this.gameObject.transform.position.y;
		Vector2 center = new Vector2( x, y );
		float r = this.gameObject.GetComponent<CircleCollider2D>().radius;

		Collider2D bullet = Physics2D.OverlapCircle( center, r, 1 << LayerMask.NameToLayer( "Bullet" ) );

		if ( bullet != null )
		{
			//only players break jars (so they can choose to save them for future generations)
			if ( bullet.gameObject.GetComponent<Bullet>().alliance == Alliance.PLAYER )
			{
				//TODO: randomize possible drops?
				Instantiate ( health_drop, this.transform.position, Quaternion.identity );
				Destroy ( this.gameObject );
			}
		}
	}
}
