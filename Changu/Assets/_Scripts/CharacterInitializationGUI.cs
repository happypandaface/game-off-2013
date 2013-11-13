using UnityEngine;
using System.Collections;

public class CharacterInitializationGUI : MonoBehaviour 
{

	#region vars
	public PlayerCore player_core;

	int state = 0;
	float t = 0.0f;

	//state 0 - 1 : M/F
	Rect m_gender_male;
	Rect m_gender_male_start = new Rect( 0, -50, 50, 50 );
	Rect m_gender_male_end   = new Rect( 0, 100, 50, 50 );
	Rect m_gender_female;
	Rect m_gender_female_start = new Rect( 100, -50, 50, 50 );
	Rect m_gender_female_end   = new Rect( 100, 100, 50, 50 );
	//public Texture2D m_gender_male_tex;
	//public Texture2D m_gender_female_tex;

	//state 2-3 : Look
	Rect m_hair;
	Rect m_hair_start = new Rect( 0, -50, 50, 50 );
	Rect m_hair_end   = new Rect( 0,   0, 50, 50 );
	Rect m_eyes;
	Rect m_eyes_start = new Rect( 0, -50, 50, 50 );
	Rect m_eyes_end   = new Rect( 0,  50, 50, 50 );
	Rect m_look_confirm;
	Rect m_look_confirm_start = new Rect( 0, -50, 50, 50 );
	Rect m_look_confirm_end   = new Rect( 0, 100, 50, 50 );

	//state 4-5 : Class
	Rect m_class_melee;
	Rect m_class_melee_start = new Rect( 0, -50, 50, 50 );
	Rect m_class_melee_end   = new Rect( 0, 100, 50, 50 );
	Rect m_class_ranged;
	Rect m_class_ranged_start = new Rect( 50, -50, 50, 50 );
	Rect m_class_ranged_end   = new Rect( 50, 100, 50, 50 );
	Rect m_class_mid;
	Rect m_class_mid_start = new Rect( 100, -50, 50, 50 );
	Rect m_class_mid_end   = new Rect( 100, 100, 50, 50 );

	//state 6-7 : Relic
	Rect m_relic_shield;
	Rect m_relic_shield_start = new Rect( 0, -50, 50, 50 );
	Rect m_relic_shield_end   = new Rect( 0, 100, 50, 50 );
	Rect m_relic_boots;
	Rect m_relic_boots_start = new Rect( 50, -50, 50, 50 );
	Rect m_relic_boots_end   = new Rect( 50, 100, 50, 50 );
	Rect m_relic_berserk;
	Rect m_relic_berserk_start = new Rect( 100, -50, 50, 50 );
	Rect m_relic_berserk_end   = new Rect( 100, 100, 50, 50 );

	#endregion

	// Use this for pre-initialization
	void Awake()
	{
		//
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//primarily for transitions.
		if ( state == 0 )
		{
			if ( t < 1.0f )
			{
				m_gender_female = LerpRect( m_gender_female_start, m_gender_female_end, t );
				m_gender_male = LerpRect( m_gender_male_start, m_gender_male_end, t );
				t += Time.deltaTime;
			}
			else
			{
				state = 1; //done moving, transition
				t = 0.0f;
			}
		}
		else if ( state == 2 )
		{
			if ( t < 1.0f )
			{
				m_hair = LerpRect( m_hair_start, m_hair_end, t );
				m_eyes = LerpRect( m_eyes_start, m_eyes_end, t );
				m_look_confirm = LerpRect( m_look_confirm_start, m_look_confirm_end, t );
				t += Time.deltaTime;
			}
			else
			{
				//done moving, transition
				state = 3;
				t = 0.0f;
			}
		}
		else if ( state == 4 )
		{
			if ( t < 1.0f )
			{
				m_class_melee = LerpRect( m_class_melee_start, m_class_melee_end, t );
				m_class_mid = LerpRect( m_class_mid_start, m_class_mid_end, t );
				m_class_ranged = LerpRect( m_class_ranged_start, m_class_ranged_end, t );
				t += Time.deltaTime;
			}
			else
			{
				//done moving, transition
				state = 5;
				t = 0.0f;
			}
		}
		else if ( state == 6 )
		{
			if ( t < 1.0f )
			{
				m_relic_shield = LerpRect( m_relic_shield_start, m_relic_shield_end, t );
				m_relic_boots = LerpRect( m_relic_boots_start, m_relic_boots_end, t );
				m_relic_berserk = LerpRect( m_relic_berserk_start, m_relic_berserk_end, t );
				t += Time.deltaTime;
			}
			else
			{
				//done moving, transition
				state = 7;
				t = 0.0f;
			}
		}
	}

	Rect LerpRect( Rect from, Rect to, float t )
	{
		//Smoothly blends from rect 1 to rect 2.
		return new Rect( Mathf.Lerp (from.x, to.x, t), 
		                 Mathf.Lerp (from.y, to.y, t),
		                 Mathf.Lerp (from.width, to.width, t),
		                 Mathf.Lerp (from.height, to.height, t) );
	}

	void OnGUI()
	{
		//Do GUI stuff.
		if ( state == 0 ) //transition to gender select
		{
			GUI.Button ( m_gender_male, "Male" );
			GUI.Button ( m_gender_female, "Female" );
		}
		if ( state == 1 ) //gender select
		{
			if ( GUI.Button ( m_gender_male, "Male" ) )
			{
				//set player gender to male
				player_core.gender = Gender.MALE;
				state = 2;
			}
			if ( GUI.Button ( m_gender_female, "Female" ) )
			{
				//set player gender to female
				player_core.gender = Gender.FEMALE;
				state = 2;
			}
		}
		if ( state == 2 ) //transition -> look select
		{
			//fade out state 1 stuff.
			GUI.Button ( m_hair, "Hair" );
			GUI.Button ( m_eyes, "Eyes" );
			GUI.Button ( m_look_confirm, "OK" );
		}
		if ( state == 3 ) //look select
		{
			if ( GUI.Button ( m_look_confirm, "OK" ) )
			{
				state = 4;
			}
		}
		if ( state == 4 ) //transition -> class select
		{
			//fade out look stuff
			GUI.Button ( m_class_melee, "Melee" );
			GUI.Button ( m_class_mid, "Mid" );
			GUI.Button ( m_class_ranged, "Ranged" );
		}
		if ( state == 5 ) //class select
		{
			if ( GUI.Button ( m_class_melee, "Melee" ) )
			{
				//set class to melee
				player_core.character_class = CharacterClass.MELEE;
				state = 6;
			}
			if ( GUI.Button ( m_class_mid, "Mid" ) )
			{
				//set class to mid
				player_core.character_class = CharacterClass.MID;
				state = 6;
			}
			if ( GUI.Button ( m_class_ranged, "Ranged" ) )
			{
				//set class to ranged
				player_core.character_class = CharacterClass.RANGED;
				state = 6;
			}
		}
		if ( state == 6 ) //transition -> relic select
		{
			//Fade out stuff from state 5
			GUI.Button ( m_relic_shield, "Shield" );
			GUI.Button ( m_relic_boots,  "Boots" );
			GUI.Button ( m_relic_berserk, "Berserk" );
		}
		if ( state == 7 ) //relic select
		{
			if ( GUI.Button ( m_relic_shield, "Shield" ) )
			{
				//set relic to shield
				player_core.relic = Relic.SHIELD;
				state = 8;
			}
			if ( GUI.Button ( m_relic_boots,  "Boots" ) )
			{
				//set relic to boots
				player_core.relic = Relic.BOOTS;
				state = 8;
			}
			if ( GUI.Button ( m_relic_berserk, "Berserk" ) )
			{
				//set relic to berserk
				player_core.relic = Relic.BERSERK;
				state = 8;
			}
		}
		if ( state == 8 )
		{
			//EXIT
		}
	}
}
