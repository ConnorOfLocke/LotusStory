using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractableObject_UI : InteractableObject
{
	public ManaUIMovement ManaUI = null;
	public Image ManaBar = null;
	
	private Vector3 OriginalManaBarScale;
	
	// Use this for initialization
	public override void Start ()
	{
		OriginalManaBarScale = ManaBar.rectTransform.localScale;
		
	}

	// Update is called once per frame
	void Update ()
	{
		ManaBar.rectTransform.localScale = OriginalManaBarScale * Mana;
	}
	
	public override void OnHover()
	{
	
	}
	
	public override void OnClickOnEvent()
	{
		ManaUI.Active = true;
	}
	public override void OnClickOffEvent()
	{
		ManaUI.Active = false;
	}
	
	public override void GiveMana(float a_Mana)
	{
		Mana += a_Mana;
	}
	public override void DrainMana(Player TargetPlayer, float a_Mana)
	{
		Mana -= a_Mana;
	
	}
}

