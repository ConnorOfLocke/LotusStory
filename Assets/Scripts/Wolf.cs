using UnityEngine;
using System.Collections;

public class Wolf : MonoBehaviour
{
	public GameObject WolfCave;
	public float AddedAnger = 0.05f;
	public AI_Component Idle_AI;
	public AI_Component Attack_AI;
	
	private AI_Component Current_AI = null;
	
	public float AttackThreshold = 0.7f;
	
	private Pissed_O_Meter MeterInScene;
						
	public void Start()
	{
		MeterInScene = FindObjectOfType<Pissed_O_Meter>();
		Current_AI = Idle_AI;
	}
	
	public void Update()
	{
		Current_AI.AI_Update();
		
		if (MeterInScene.CurrentAnger > AttackThreshold)
			Current_AI = Attack_AI;
		else
			Current_AI = Idle_AI;
	}
		
	void OnDestroy()
	{
		if (MeterInScene != null)
			MeterInScene.AddAngerModifier(AddedAnger, "A wolf's life has ended");
	}
	
}

