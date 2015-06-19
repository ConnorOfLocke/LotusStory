using UnityEngine;
using System.Collections;

public enum eAI_TYPE
{
	AI_IDLE,
	AI_ATTACK
}

public abstract class AI_Component : MonoBehaviour
{	
	public eAI_TYPE AI_Type; 
	public abstract void AI_Update();
}

