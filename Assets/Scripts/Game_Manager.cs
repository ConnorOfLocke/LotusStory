using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game_Manager : MonoBehaviour {

	public Pissed_O_Meter Pissed_O_Meter;
	
	public FollowMeCam CameraInScene;
	public PlayerContoller PlayerController;
	
	public Collusus CollususObject;
	
	public GameObject FinalParticleEffect;
	
	public float TimeFadingIn = 3.0f;
	private float CurrentTime = 0.0f;
	
	public Image FinalOverlay;
	public Text FinalText;
	public Vector3 ActivePosition;
	public Vector3 InActivePosition;
	
	private bool GameEnded = false;

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
		
		if (Input.GetKey (KeyCode.F1))
		{
			Application.LoadLevel("mainScene");
		}
		
		if (!GameEnded)
		{
			if (Pissed_O_Meter.TownList.Count == 0)
			{
				FinalText.text = "A civilisation crumbles";
				GameEnded = true;
				
				CollususObject.Stop();
				PlayerController.StopInput();
			}
			
			else if (Pissed_O_Meter.AttachedCollusus.Health <= 0)
			{
				FinalText.text = "A force of nature has been defeated";
				CollususObject.Stop();
				PlayerController.StopInput();
				GameObject.Instantiate(FinalParticleEffect, CollususObject.transform.position, Quaternion.identity);
		
				CameraInScene.ObjectToFollow = CollususObject.gameObject;
			
				GameEnded = true;
			}
			
			else if (PlayerController.CurrMaxManaPower <= 0)
			{
				FinalText.text = "You are ";
				CollususObject.Stop();
				PlayerController.StopInput();
				GameObject.Instantiate(FinalParticleEffect, PlayerController.TargetPlayer.transform.position, Quaternion.identity);
				
				GameEnded = true;
			}	
		}
		else
		{
			if (CurrentTime < TimeFadingIn)
			{
				//fade in overlay
				FinalOverlay.color = new Color(FinalOverlay.color.r, FinalOverlay.color.g, FinalOverlay.color.a, CurrentTime / TimeFadingIn * 0.7f );
				
				Vector3 FinalPosition = FinalText.GetComponent<RectTransform>().localPosition;
				
				Vector3 SmoothVelocity = Vector3.one;
				FinalText.GetComponent<RectTransform>().localPosition = Vector3.SmoothDamp(FinalPosition, ActivePosition, ref SmoothVelocity, CurrentTime);
				
				CurrentTime += Time.deltaTime;
			}
			
		}
		
	}
}
