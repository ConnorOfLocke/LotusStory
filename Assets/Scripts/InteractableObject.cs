using UnityEngine;
using System.Collections;

public abstract class InteractableObject : MonoBehaviour {

	public float Mana = 1.0f;
	protected Collider AttachedCollider;

	public abstract void OnHover();
	public abstract void OnClickOnEvent();
	public abstract void OnClickOffEvent();
	
	public abstract void GiveMana(float Mana);
	public abstract void DrainMana(Player TargetPlayer, float Mana);
	
	virtual public void Start()
	{
	
	}
	

}
