using UnityEngine;
using System.Collections;

public class Villager : MonoBehaviour 
{
	public GameObject TownHub;
	public AI_Component Idle_AI;
	public AI_Component Attack_AI;
	
	public AI_Component Current_AI;
	public float AddedAnger = -0.05f;
	
	public float AttackThreshold = 0.4f;
	
	Pissed_O_Meter MeterInScene;
	void Start()
	{
		MeterInScene = FindObjectOfType<Pissed_O_Meter>();
	
		AI_IdleWander IdleWander = (AI_IdleWander)Idle_AI;
		if ( (AI_IdleWander)Idle_AI != null)
			IdleWander.HubPosition = TownHub.transform.position;
	
		Current_AI = Idle_AI;	
	}
	
	void Update()
	{
		Current_AI.AI_Update();
		
		if (MeterInScene.CurrentAnger < AttackThreshold || TownHub == null)
			Current_AI = Attack_AI;
		else
			Current_AI = Idle_AI;
	
	}
	
	void OnDestroy()
	{
		if (MeterInScene != null)
			MeterInScene.AddAngerModifier(AddedAnger, "A villagers life has ended");
	}
}
