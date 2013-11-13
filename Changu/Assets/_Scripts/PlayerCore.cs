using UnityEngine;
using System.Collections;

public class PlayerCore : MonoBehaviour 
{
	#region vars
	//HP
	public float hp = 100.0f;
	public float max_hp = 100.0f;

	//Stamina
	public float sta = 100.0f;
	public float max_sta = 100.0f;

	int generation = 0;

	string character_class; //ENUM: melee, ranged, ...
	string relic; //ENUM: shield, boots, berserk
	
	float berserk_t = 0.0f;
	bool berserk_on = false;
	float berserk_sta_cost;
	//cast time
	
	bool shield_on = false;
	float shield_sta_cost = 10.0f; //multiplied by damage?
	float shield_dmg;      //current damage accumulation
	float shield_max_dmg;  //shield breaker damage
	//cast time

	float boots_t = 0.0f;
	bool boots_on = false;
	float boots_sta_cost = 10.0f;
	//cast/ani time


	#endregion

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		HandleInput ();
	}

	void HandleInput()
	{
		#region relics
		if ( /*Input.GetKeyDown( KeyCode.Q ) ||*/ Input.GetMouseButtonDown( 1 ) ) //0: L, 1: R, 2: M
		{
			//DO Abilities
			#region Shield
			if ( relic == "shield" )
			{
				if ( shield_on == false )
				{
					if ( sta >= shield_sta_cost )
					{
						shield_on = true;
						sta -= shield_sta_cost;
						//Disable / slow movement. Freeze facing?
						//Animate
					}
					else
					{
						//not enough stamina
						//Do breath animation
					}
				}
				else
				{
					//shield is already on. (Should be unreachable code)
				}
			}
			#endregion

			#region Boots
			else if ( relic == "boots" )
			{
				if ( boots_on == false )
				{
					if ( sta >= boots_sta_cost )
					{
						sta -= boots_sta_cost;
						//animate
					}
					else
					{
						//not enough stamina
						//Do breath animation
					}
				}
				else
				{
					//boots are still animating
				}
			}
			#endregion

			#region Zerk
			else if ( relic == "berserk" )
			{
				if ( berserk_on == false )
				{
					if ( sta >= berserk_sta_cost )
					{
						sta -= berserk_sta_cost;
						berserk_on = true;
						berserk_t = 60; //TODO: formula all over it.
						//animate
					}
					else
					{
						//not enough stamina
						//Do breath animation
					}
				}
				else
				{
					//toggle it off. (reduces CD)
					berserk_on = false;
					berserk_t = 0;
				}
			}
			#endregion
		}
		if ( Input.GetMouseButtonUp( 1 ) )
		{
			#region shield
			if ( shield_on == true )
			{
				shield_on = false;
				//Un-do motion modifications
				//animate
			}
			#endregion
		}
		#endregion

		#region attacks
		if ( Input.GetMouseButtonUp( 0 ) )
		{
			hp -= 1.0f;
			//attacks
			if ( character_class == "melee" )
			{
				//
			}
			if ( character_class == "ranged" )
			{
				//
			}
		}
		#endregion
	}
}
