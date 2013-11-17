using UnityEngine;
using System.Collections;

public enum GenerationalTransition { CHILD, STUDENT, ENCORE };

//Class to store player data over generations
public class GenerationalData : MonoBehaviour 
{

	#region vars
	public PlayerCore player;

	ArrayList transitions; //list of transitions: child, student, encore<only if not dead>.

	bool penalized = false;
	#endregion

	// Use this for initialization
	void Start () 
	{
		transitions.Add ( GenerationalTransition.CHILD );
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnNextGeneration()
	{
		//Only call this directly if the hero has NOT died.
		//Randomize player + "level up"

		//get transition
		GenerationalTransition transition = (GenerationalTransition)transitions[ player.generation ];
		player.generation++;

		//Always improve relics
		UpgradeRelics();

		#region undo penalties
		//Undo stat penalties
		if ( penalized )
		{
			PenalizeStats ( -1.0f );
		}
		#endregion

		if ( transition == GenerationalTransition.CHILD )
		{
			//CHILD:
			//LOOK:
			ChildAppearance(); //randomize appearance within a smaller range (keep colors, change styles)
			RandomizeGender(); //randomize gender
			RandomizeClass(); //randomize class
		}
		if ( transition == GenerationalTransition.STUDENT )
		{
			//STUDENT:
			RandomizeAppearance(); //randomize appearance
			RandomizeGender();     //randomize gender
			//keep class
		}
		if ( transition == GenerationalTransition.ENCORE )
		{
			//ENCORE:
			AgeAppearance(); //age appearance
			//keep gender
			//keep class

			//stat penalties //store something to undo them
			PenalizeStats ( 1.0f );
			penalized = true;
			//attack bonuses?
			UpgradeSkills();
		}
	}

	void OnHeroDeath()
	{
		//Re-shuffle encore
		GenerationalTransition transition = (GenerationalTransition)transitions[ player.generation ];

		if ( transition == GenerationalTransition.ENCORE )
		{
			//mess with the array list.
		}
		OnNextGeneration ();
	}

	void UpgradeSkills()
	{
	}

	void PenalizeStats( float magnitude )
	{
		//param 1 penalizes stats,
		//param -1 undoes it.
		player.max_hp -= magnitude * 10.0f;
		player.max_sta -= magnitude * 10.0f;
		player.move_controller.speed -= magnitude * 0.5f; //maybe move speed into core?

		//down dodge speed as well?
	}

	void UpgradeRelics()
	{
		player.SetRelicStats ();
	}

	#region appearance functions
	void AgeAppearance() 
	{
		//if male, set to old man
		//if female, set to old lady
	    //player.
	}

	void ChildAppearance()
	{
		//keep colors, change styles
	}

	void RandomizeAppearance()
	{
		//RANDOM!
	}

	void RandomizeGender()
	{
		int rn = Random.Range(0, 1);
		if ( rn == 0 )
		{
			player.gender = Gender.MALE;
		}
		else
		{
			player.gender = Gender.FEMALE;
		}
	}

	void RandomizeClass()
	{
		int rn =  Random.Range (0, 1);
		if ( rn == 0 )
		{
			player.character_class = CharacterClass.MELEE;
		}
		else if ( rn == 1 )
		{
			player.character_class = CharacterClass.RANGED;
		}
		else
		{
			player.character_class = CharacterClass.MID;
		}
	}
	#endregion
}
