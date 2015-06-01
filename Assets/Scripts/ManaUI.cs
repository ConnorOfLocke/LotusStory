using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManaUI : MonoBehaviour {

	public PlayerContoller AttachedPlayer;
	public Image ManaBar;
	public Image CurrentSpellBar;
	public float LerpFactor = 0.2f;
	
	private float LeftManaPosition;
	private float RightManaPosition;
	private float MaxManaScale;
	
	// Use this for initialization
	void Start () {
		LeftManaPosition = ManaBar.rectTransform.position.x;
		MaxManaScale = ManaBar.rectTransform.sizeDelta.x;
		RightManaPosition = LeftManaPosition + MaxManaScale;
	}
	
	// Update is called once per frame
	void Update () {
		
		float CurrMinManaPower = AttachedPlayer.CurrMinManaPower;
		float CurrMaxManaPower = AttachedPlayer.CurrMaxManaPower;
		float MaxManaPower = AttachedPlayer.MaxManaPower;
		
		float CurPower = AttachedPlayer.SpellHoldTime;
		
		float ManaLeftX = (CurrMinManaPower / MaxManaPower) * MaxManaScale + LeftManaPosition;
		float ManaRightX = (CurrMaxManaPower / MaxManaPower) * MaxManaScale + LeftManaPosition;
		float ManaWidth = ManaRightX - ManaLeftX;
		
		float CurPowerRightX = (CurPower / MaxManaPower) * MaxManaScale + LeftManaPosition;
		float PowerWidth = CurPowerRightX - ManaLeftX;


		ManaBar.rectTransform.position = Vector3.Lerp(	ManaBar.rectTransform.position,
													  	new Vector3(ManaLeftX, ManaBar.rectTransform.position.y, ManaBar.rectTransform.position.z),
		                                              	LerpFactor);
													  
		ManaBar.rectTransform.sizeDelta = Vector2.Lerp(	ManaBar.rectTransform.sizeDelta,
														new Vector2(ManaWidth, ManaBar.rectTransform.sizeDelta.y),
		                                               	LerpFactor);
		
		CurrentSpellBar.rectTransform.position = Vector3.Lerp(	CurrentSpellBar.rectTransform.position,
																new Vector3(ManaLeftX, ManaBar.rectTransform.position.y, ManaBar.rectTransform.position.z),
		                                                      	LerpFactor);
																
		CurrentSpellBar.rectTransform.sizeDelta = new Vector2(PowerWidth, CurrentSpellBar.rectTransform.sizeDelta.y);		
		
	}
}
