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

	//Objects to take on values
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

		//set up text
		text = text_obj.GetComponent<GUIText>();
		text.pixelOffset = new Vector2( Screen.width / 2, 32 );
		text.text = "Once upon a time ...";

		OpeningCutscene ();
		//state = CutsceneState.START;
		//state = "Fade In";
	}
	
	// Update is called once per frame
	void Update () 
	{
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
					//TODO: img

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
			//TODO: hide fg
		}
		else if ( state == CutsceneState.END )
		{
			//fade bg out
			background.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f - ( t / t_goal ) );
			//hide text
			text.color = new Color( 1.0f, 1.0f, 1.0f, 0.0f );
			//TODO: hide fg

		}
		else if ( state == CutsceneState.FADE_IN )
		{
			//full alpha bg
			background.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
			//fade text in
			text.color = new Color( 1.0f, 1.0f, 1.0f, t / t_goal );
			//TODO: fade fg in
		}
		else if ( state == CutsceneState.WAIT )
		{
			//full alpha bg
			background.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
			//full alpha text
			text.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
			//TODO: full alpha fg
		}
		else if ( state == CutsceneState.FADE_OUT )
		{
			//full alpha bg
			background.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
			//fade text out
			text.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f - ( t / t_goal ) );
			//TODO: fade fg out
		}
		else
		{
			//off
			background.color = new Color( 1.0f, 1.0f, 1.0f, 0.0f );
			text.color = new Color( 1.0f, 1.0f, 1.0f, 0.0f );
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
		scenes.Add ( new CutsceneData(2.0f, "there was a great evil.", 0) );
		scenes.Add ( new CutsceneData(2.5f, "Many heroes fought against it,", 0) );
		scenes.Add ( new CutsceneData(2.0f, "and many heroes fell.", 0) );
		scenes.Add ( new CutsceneData(1.0f, "In the end,", 0) );
		scenes.Add ( new CutsceneData(1.0f, "one prevailed.", 0) );
		scenes.Add ( new CutsceneData(2.5f, "He sealed the evil away...", 0) );
		scenes.Add ( new CutsceneData(2.5f, "And hid it from the eyes of men.", 0) );
		scenes.Add ( new CutsceneData(0.0f, "", 0) );

		scenes.Add ( new CutsceneData(3.0f, "For his valor, the hero was given riches.", 0) );
		scenes.Add ( new CutsceneData(2.0f, "But when he died...", 0) );
		scenes.Add ( new CutsceneData(2.0f, "His children became greedy.", 0) );
		scenes.Add ( new CutsceneData(1.5f, "They fought over it.", 0) );
		scenes.Add ( new CutsceneData(2.0f, "After much bloodshed,", 0) );
		scenes.Add ( new CutsceneData(2.5f, "They decided to hide the treasure,", 0) );
		scenes.Add ( new CutsceneData(2.0f, "so no one would fight over it.", 0) );
		scenes.Add ( new CutsceneData(0.0f, "", 0) );

		scenes.Add ( new CutsceneData(1.0f, "Many years passed.", 0) );
		scenes.Add ( new CutsceneData(2.5f, "People forgot the hero, and the evil.", 0) );
		scenes.Add ( new CutsceneData(3.0f, "But they still search for his treasure...", 0) );
		//start it!
		StartCutscene();
	}
}
