using UnityEngine;
using System.Collections;

public class Game_Manager : MonoBehaviour {

	public Pissed_O_Meter Pissed_O_Meter;
	public PlayerContoller Player;
	
	// Update is called once per frame
	void Update () 
	{
		if (Pissed_O_Meter.TownList.Count == 0)
		{
			//NATURE WINS AAAAHHHHHHHHH
		}
		
		else if (Pissed_O_Meter.AttachedCollusus.Health <= 0)
		{
			//CIVILISATION WINS
		}
		
		else if (Player.CurrMaxManaPower <= 0)
		{
			//PLAYER LOSES AHHHHHH
		}	
	}
}
