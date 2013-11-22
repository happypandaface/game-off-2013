using UnityEngine;
using System.Collections;


//Class that handles displaying cutscene text.

//TODO: Disable all other GUI Elements when this is up.
//TODO: Freeze game while this is up.

//TODO: Add fade in/out, transitions: text change, bg change, timing.
public class CutsceneData
{
	//Stores cutscene data
	public float t = 0.0f;    //duration of this frame, in seconds
	public string text = "";  //text to be displayed
	public int img_index = 0; //index of the still to be displayed.

	public CutsceneData( float initial_t, string initial_text, int initial_i )
	{
		t = initial_t;
		text = initial_text;
		img_index = initial_i;
	}
}

public enum CutsceneState { START, END, FADE_IN, WAIT, FADE_OUT, OFF };

public class Cutscene : MonoBehaviour 
{
	#region vars
	//The scene processing data
	ArrayList scenes = new ArrayList();
	CutsceneData current_scene;

	//state and timers
	CutsceneState state = CutsceneState.START;
	float t = 0.0f;
	float t_goal = 1.0f;

	public Texture2D bg_blank; //bg texture
	public Texture2D[] fgs = new Texture2D[20]; //assign in editor

	//Objects to take on values
	public GameObject foreground_obj; //assign in editor
	GUITexture foreground;

	public GameObject background_obj; //assign in editor
	GUITexture background;

	public GameObject text_obj; //assign in editor
	GUIText text;
	#endregion

	// Use this for initialization
	void Start () 
	{
		//set up background image
		background = background_obj.GetComponent<GUITexture>();
		background.pixelInset = new Rect( 0, 0, Screen.width, Screen.height );
		background.texture = bg_blank;

		//set up foreground image
		foreground = foreground_obj.GetComponent<GUITexture>();
		foreground.pixelInset = new Rect( 0 + 64, 0 + 64, Screen.width - 64, Screen.height - 64 );
		foreground.texture = fgs[0];

		//set up text
		text = text_obj.GetComponent<GUIText>();
		text.pixelOffset = new Vector2( Screen.width / 2, 32 );
		text.text = "Once upon a time ...";

		OpeningCutscene ();
		//EncoreCutscene();
		//PandorasBoxCutscene ();
		//DeathCutscene ();
		//VictoryCutscene ();
		//state = CutsceneState.START;
		//state = "Fade In";
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( Input.GetKeyDown ( KeyCode.Space ) || Input.GetKeyDown ( KeyCode.Return ) )
		{
			state = CutsceneState.END;
		}

		#region state management
		t += Time.deltaTime;
		if ( t >= t_goal ) //state transition
		{
			#region start
			if ( state == CutsceneState.START )
			{
				if ( scenes.Count > 0 )
				{
					state = CutsceneState.FADE_IN;
					//copy constructor: current scene = scene[0]
					current_scene = new CutsceneData( ((CutsceneData)scenes[0]).t, ((CutsceneData)scenes[0]).text, ((CutsceneData)scenes[0]).img_index );

					t = 0.0f;
					t_goal = 1.0f; //constant fade in time
					text.text = current_scene.text;
					foreground.texture = fgs[current_scene.img_index];

					//delete data
					scenes.RemoveAt ( 0 );
				}
				else
				{
					t = 0.0f;
					t_goal = 1.0f;	//constant end fade out time
					//no new scenes, end.
					state = CutsceneState.END;
				}
			}
			#endregion
			#region end
			else if ( state == CutsceneState.END )
			{
				//it's over! Do nothing.
				state = CutsceneState.OFF;
			}
			#endregion
			#region fade in
			else if ( state == CutsceneState.FADE_IN )
			{
				t = 0.0f;
				t_goal = current_scene.t; //wait for the allotted time
				state = CutsceneState.WAIT;
			}
			#endregion
			#region wait
			else if ( state == CutsceneState.WAIT )
			{
				t = 0.0f;
				t_goal = 1.0f; //constant fade out time
				state = CutsceneState.FADE_OUT;
			}
			#endregion
			#region fade out
			else if ( state == CutsceneState.FADE_OUT )
			{
				if ( scenes.Count > 0 )
				{
					state = CutsceneState.FADE_IN;
					//copy constructor: current scene = scene[0]
					current_scene = new CutsceneData( ((CutsceneData)scenes[0]).t, ((CutsceneData)scenes[0]).text, ((CutsceneData)scenes[0]).img_index );
					
					t = 0.0f;
					t_goal = 1.0f; //constant start fade in time
					text.text = current_scene.text;
					foreground.texture = fgs[current_scene.img_index];
					//TODO: img
					
					//delete data
					scenes.RemoveAt ( 0 );
				}
				else
				{
					t = 0.0f;
					t_goal = 1.0f; //constant end fade out time
					//no new scenes, end.
					state = CutsceneState.END;
				}
			}
			#endregion
		}
		#endregion

		#region alpha control
		//Blending.
		if ( state == CutsceneState.START )
		{
			//fade bg in
			background.color = new Color( 1.0f, 1.0f, 1.0f, t / t_goal );
			//hide text
			text.color = new Color( 1.0f, 1.0f, 1.0f, 0.0f );
			//hide fg
			foreground.color = new Color( 1.0f, 1.0f, 1.0f, 0.0f );
		}
		else if ( state == CutsceneState.END )
		{
			//fade bg out
			background.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f - ( t / t_goal ) );
			//hide text
			text.color = new Color( 1.0f, 1.0f, 1.0f, 0.0f );
			//hide fg
			foreground.color = new Color( 1.0f, 1.0f, 1.0f, 0.0f );

		}
		else if ( state == CutsceneState.FADE_IN )
		{
			//full alpha bg
			background.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
			//fade text in
			text.color = new Color( 1.0f, 1.0f, 1.0f, t / t_goal );
			//fade fg in
			foreground.color = new Color( 1.0f, 1.0f, 1.0f, t / t_goal );
		}
		else if ( state == CutsceneState.WAIT )
		{
			//full alpha bg
			background.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
			//full alpha text
			text.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
			//full alpha fg
			foreground.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
		}
		else if ( state == CutsceneState.FADE_OUT )
		{
			//full alpha bg
			background.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
			//fade text out
			text.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f - ( t / t_goal ) );
			//fade fg out
			foreground.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f - ( t / t_goal ) );
		}
		else
		{
			//off
			background.color = new Color( 1.0f, 1.0f, 1.0f, 0.0f );
			text.color = new Color( 1.0f, 1.0f, 1.0f, 0.0f );
			foreground.color = new Color( 1.0f, 1.0f, 1.0f, 0.0f );
		}
		#endregion
	}

	void StartCutscene()
	{
		//utility function to set up start state
		state = CutsceneState.START;
		t = 0.0f;
		t_goal = 1.0f;
	}

	public void OpeningCutscene()
	{
		//starts the opening cutscene
		//set up data
		scenes.Add ( new CutsceneData(2.0f, "Once, a long time ago...", 0) );
		scenes.Add ( new CutsceneData(2.0f, "there was a great evil.", 1) );
		scenes.Add ( new CutsceneData(2.5f, "Many heroes fought against it,", 2) );
		scenes.Add ( new CutsceneData(2.0f, "and many heroes fell.", 3) );
		scenes.Add ( new CutsceneData(1.0f, "In the end,", 4) );
		scenes.Add ( new CutsceneData(1.0f, "one prevailed.", 5) );
		scenes.Add ( new CutsceneData(2.5f, "He sealed the evil away...", 6) );
		scenes.Add ( new CutsceneData(2.5f, "And hid it from the eyes of men.", 10) );
		scenes.Add ( new CutsceneData(0.0f, "", 0) );

		scenes.Add ( new CutsceneData(3.0f, "For his valor, the hero was given riches.", 7) );
		scenes.Add ( new CutsceneData(2.0f, "But when he died...", 8) );
		scenes.Add ( new CutsceneData(2.0f, "His children became greedy.", 0) );
		scenes.Add ( new CutsceneData(1.5f, "They fought over it.", 9) );
		scenes.Add ( new CutsceneData(2.0f, "After much bloodshed,", 3) );
		scenes.Add ( new CutsceneData(2.5f, "They decided to hide the treasure,", 10) );
		scenes.Add ( new CutsceneData(2.0f, "so no one would fight over it.", 0) );
		scenes.Add ( new CutsceneData(0.0f, "", 0) );

		scenes.Add ( new CutsceneData(1.0f, "Many years passed.", 0) );
		scenes.Add ( new CutsceneData(2.5f, "People forgot the hero, and the evil.", 4) );
		scenes.Add ( new CutsceneData(3.0f, "But they still search for his treasure...", 10) );
		//start it!
		StartCutscene();
	}

	public void PandorasBoxCutscene()
	{
		//starts the first boss fight cutscene
		//set up data
		scenes.Add ( new CutsceneData(4.0f, "Was this the treasure of the hero of old?", 10) );
		scenes.Add ( new CutsceneData(0.5f, "...", 7) );
		scenes.Add ( new CutsceneData(0.5f, "No.", 10) );
		scenes.Add ( new CutsceneData(0.0f, "", 10) );
		scenes.Add ( new CutsceneData(2.0f, "This was the ancient evil!", 6) );
		//start it!
		StartCutscene();
	}

	public void EncoreCutscene()
	{
		//starts the encore transition cutscene
		//set up data
		string hero = "hero"; //heroine
		string his = "his"; //her

		scenes.Add ( new CutsceneData(2.5f, "The " + hero + " vanquished the evil", 5) );
		scenes.Add ( new CutsceneData(2.0f, "...Or so they thought.", 0) );
		scenes.Add ( new CutsceneData(1.0f, "Years passed.", 0) );
		scenes.Add ( new CutsceneData(2.5f, "And the evil stirred once again.", 0) );
		scenes.Add ( new CutsceneData(2.5f, "But some " + hero + "s are the stuff of legends.", 0) );

		scenes.Add ( new CutsceneData(1.0f, "Some " + hero + "s never die.", 4) );
		//start it!
		StartCutscene();
	}

	public void DeathCutscene()
	{
		//starts the death transition cutscene
		//set up data
		string hero = "hero"; //heroine
		string his = "his"; //her
		string children = "children";  //disciples

		scenes.Add ( new CutsceneData(2.0f, "The " + hero + " fell in battle.", 11) );
		scenes.Add ( new CutsceneData(2.5f, his + " " + children + " mourned thier loss.", 8) );
		scenes.Add ( new CutsceneData(2.0f, "And vowed revenge.", 4) );
		//start it!
		StartCutscene();
	}

	public void VictoryCutscene()
	{
		//starts the victory transition cutscene
		//set up data
		string hero = "hero"; //heroine
		string his = "his"; //her
		string child = "descendant"; //disciple
		
		scenes.Add ( new CutsceneData(2.5f, "The " + hero + " vanquished the evil", 5) );
		scenes.Add ( new CutsceneData(2.0f, "...Or so they thought.", 0) );
		scenes.Add ( new CutsceneData(1.0f, "Years passed.", 0) );
		scenes.Add ( new CutsceneData(1.5f, "The " + hero + " died.", 8) );
		scenes.Add ( new CutsceneData(2.5f, "And the evil stirred once again.", 0) );
		scenes.Add ( new CutsceneData(2.5f, "But there will always be a hero.", 0) );
		scenes.Add ( new CutsceneData(2.5f, his + " brave " + child + " took up " + his + " mantle,", 0) );
		scenes.Add ( new CutsceneData(2.5f, "And set out to beat it again.", 4) );
		//start it!
		StartCutscene();
	}

	public void FinalDeathCutscene()
	{
	}

	public void FinalVictoryCutscene()
	{
	}
}
