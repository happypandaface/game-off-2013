using UnityEngine;
using System.Collections;

#region enums
public enum CharacterClass {MELEE, RANGED, MID};

public enum Relic {SHIELD, BOOTS, BERSERK};

public enum Gender {MALE, FEMALE};
#endregion

public class PlayerCore : MonoBehaviour 
{
	#region vars
	//Inter-generational data


	//Other core player stuff
	[HideInInspector]
	public CustomController move_controller;

	public GameObject arrow_prefab;

	//HP
	public float hp = 100.0f;
	public float max_hp = 100.0f;

	//Stamina
	public float sta = 100.0f;
	public float max_sta = 100.0f;
	float sta_regen_rate = 10.0f; //stamina regen per sec.

	[HideInInspector]
	public int generation = 0;

	public CharacterClass character_class; //ENUM: melee, ranged, ...
	public Relic relic = Relic.BOOTS; //ENUM: shield, boots, berserk
	public Gender gender = Gender.MALE; //ENUM: 0: female, 1: male

	//hair color / style
	//eye color / style
	//skin tone

	float berserk_t = 0.0f;
	bool berserk_on = false;
	float berserk_sta_cost;

	float berserk_dur = 15.0f; //seconds
	float berserk_dmg_multiplier = 2.0f; //damage amp. factor
	float berserk_def_multiplier = 0.5f; //damage intake reduced by this percent.
	float berserk_cooldown_t;
	float berserk_cooldown_total = 10.0f; //seconds
	//cast time
	
	bool shield_on = false;
	float shield_sta_cost = 10.0f; //multiplied by damage?
	float shield_dmg;      //current damage accumulation
	float shield_max_dmg = 20.0f;  //shield breaker damage
	float shield_regen_rate = -10.0f; //dmg / sec
	//cast time

	bool boots_on = false;
	float boots_sta_cost = 25.0f;
	float boots_t = 0.0f;
	float boots_duration = 0.5f;
	float boots_invincible_duration = 7.5f / 60.0f; //7.5 frames of invulnerability
	float boots_speed = 8.0f; //unity units / s
	Vector2 boots_dir = new Vector2( 1.0f, 0.0f );
	bool dodge_enqueued = false;
	Vector2 dodge_dir = new Vector2( 0, 0 );
	//cast/ani time


	#endregion

	//Use this for pre-initialization
	void Awake()
	{
		//set up links
		move_controller = this.gameObject.GetComponent<CustomController>();
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		HandleInput ();
		DeathCheck ();

		//regens
		if ( boots_on == false && shield_on == false )
		{
			sta = Mathf.Min( sta + sta_regen_rate * Time.deltaTime, max_sta );
		}

		if ( shield_on == false )
		{
			shield_dmg = Mathf.Max( shield_dmg + shield_regen_rate * Time.deltaTime, 0.0f );
		}


		if ( berserk_cooldown_t > 0.0f )
		{
			berserk_cooldown_t -= Time.deltaTime;
		}

		//Berserk on/off.
		if ( berserk_on == true )
		{
			berserk_t -= Time.deltaTime;
			if ( berserk_t <= 0.0f )
			{
				berserk_on = false;
				//TODO: turn off berserk effect (highlight?)
			}
		}

		//Dodge on/off.
		if ( boots_on == true )
		{
			boots_t -= Time.deltaTime;
			if ( boots_t <= 0.0f )
			{
				if ( dodge_enqueued == true)
				{
					dodge_enqueued = false;
					//TODO: refactor
					if ( sta >= boots_sta_cost )
					{
						sta -= boots_sta_cost;
						boots_dir = dodge_dir;
						boots_t = boots_duration;
					}
					else
					{
						//not enough stamina
						//do breath ani.
					}
				}
				else
				{
					//Turn off
					boots_on = false;
					move_controller.move_enabled = true;
				}
			}
			else
			{

				move_controller.Move ( new Vector3( boots_dir.x * boots_speed * Time.deltaTime, 
				                                    boots_dir.y * boots_speed * Time.deltaTime,
				                                    0.0f ) );

				//Force Camera to lock onto player
				Camera.main.transform.position = new Vector3( this.gameObject.transform.position.x,
				                                             this.gameObject.transform.position.y,
				                                             Camera.main.transform.position.z );
			}
		}
	}

	void HandleInput()
	{
		#region relics
		if ( /*Input.GetKeyDown( KeyCode.Q ) ||*/ Input.GetMouseButtonDown( 1 ) ) //0: L, 1: R, 2: M
		{
			//DO Abilities
			#region Shield
			if ( relic == Relic.SHIELD )
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
			else if ( relic == Relic.BOOTS )
			{
				if ( boots_on == false )
				{
					if ( sta >= boots_sta_cost )
					{
						sta -= boots_sta_cost;
						move_controller.move_enabled = false;
						boots_on = true;
						boots_t = boots_duration;
						boots_dir = GetBootDir();
						//get dir, start moving
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
					//boots are still animating (enqueue another dodge?)
					dodge_enqueued = true;
					//set direction
					dodge_dir = GetBootDir();
				}
			}
			#endregion

			#region Zerk
			else if ( relic == Relic.BERSERK )
			{
				if ( berserk_on == false )
				{
					if ( sta >= berserk_sta_cost )
					{
						if ( berserk_cooldown_t <= 0 )
						{
							sta -= berserk_sta_cost;
							berserk_on = true;
							berserk_t = berserk_dur;
							berserk_cooldown_t = berserk_cooldown_total + berserk_dur;
							//Pause game + special animation?
						}
						else
						{
							//Cooldown hasn't expired yet.
						}
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

					float percent_used = ( (berserk_dur - berserk_t) / berserk_dur);
					berserk_cooldown_t -= (1.0f - percent_used) * (0.5f * berserk_cooldown_total + berserk_dur); //prorate the cooldown.
					//50% penalty applied to the cooldown portion to keep from spamming it on/off?

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
				TurnOffShield();
			}
			#endregion
		}
		#endregion

		#region attacks
		if ( Input.GetMouseButtonUp( 0 ) )
		{
			GameObject arrow = (GameObject)Instantiate ( arrow_prefab, 
				new Vector3( 
			    this.gameObject.transform.position.x,
			    this.gameObject.transform.position.y,
			    3.0f), 
			    Quaternion.identity );
			arrow.GetComponent<Bullet>().alliance = Alliance.PLAYER;
			arrow.GetComponent<Bullet>().damage = 10.0f;

			float angle = Mathf.Atan2 ( Input.mousePosition.y - Screen.height / 2, Input.mousePosition.x - Screen.width / 2 );

			arrow.GetComponent<Bullet>().direction = new Vector3( Mathf.Cos ( angle ), Mathf.Sin ( angle ) );

			//attacks
			if ( character_class == CharacterClass.MELEE )
			{
				//
			}
			if ( character_class == CharacterClass.RANGED )
			{
				//
			}
		}
		#endregion
	}

	void TurnOffShield()
	{
		shield_on = false;
		//Un-do motion modifications
		//animate
	}

	Vector2 GetBootDir()
	{
		//returns the normalized direction vector to dodge towards.
		float angle = Mathf.Atan2 ( Input.mousePosition.y - Screen.height / 2, Input.mousePosition.x - Screen.width / 2 );
		return new Vector2( Mathf.Cos ( angle ), Mathf.Sin ( angle ) );
	}

	public void OnHit( float dmg )
	{
		//Player was hit by something

		//Mitigation:
		if ( shield_on == true )
		{
			//check if it breaks the shield
			shield_dmg += dmg;
			if ( shield_dmg >= shield_max_dmg )
			{
				TurnOffShield();
			}

			//ignore damage
			//TODO: decide block or not based on impact angle.
			//TODO: damage % cutout? (ie getting hit does 10% damage, a break does 20%?)
			dmg = 0.0f;
			//animate block
		}
		else if ( boots_on == true && boots_t > boots_duration - boots_invincible_duration )
		{
			//ignore damage, dodged it with i-frames.
			dmg = 0.0f;
		}
		else
		{
			if ( berserk_on == true )
			{
				//take reduced damage
				dmg = ( 1.0f - berserk_def_multiplier) * dmg;
			}

			hp -= dmg;
			//animate
			//check death?
		}
	}

	void DeathCheck()
	{
		if ( hp <= 0.0f )
		{
			//YOU DIE
			//if gen < end, play sad music, go next gen
			//else, "next gen" = bad ending.
			if ( generation <= 7 )
			{
				//Generational stuff using that script

				//death flag
				//trigger death event.
				//disable controls + ai?

				//relic upgrades: change stats
				//ability levels up...
				//class + appearance changes
				//roll matching scene.
			}
		}
	}

	public void SetRelicStats()
	{
		//sets the relic stats based on the generation (Upgrades them)

		int g = generation; //base 0
		float max_g = 7.0f;   //maximum generations (remember: base 0)

		boots_invincible_duration = (7.0f * (g / max_g) + 7.5f) / 60.0f; //increase i fram duration
		boots_sta_cost = -10.0f * (g / max_g) + 20.0f; //reduce stamina cost

		shield_max_dmg = 80.0f * (g / max_g) + 20.0f; //increase damage capacity
		shield_regen_rate = -40.0f * (g / max_g) - 10.0f; //increase regen rate.
		shield_sta_cost = -5.0f * (g / max_g) + 10.0f; //reduce stamina cost?

		berserk_dur = 15.0f * (g / max_g) + 15.0f; //increase duration
		berserk_dmg_multiplier = 1.0f * (g / max_g) + 2.0f; //increase damage bonus
		berserk_def_multiplier = 0.25f * (g / max_g) + 0.5f; //increase defense bonus
		berserk_sta_cost = 0.0f * (g / max_g) + 0.0f; //reduce stamina cost?
		berserk_cooldown_total = -1.0f * (g / max_g) + 10.0f; //reduce CD?
	}
}
