using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManaUI : MonoBehaviour {

	public PlayerContoller AttachedPlayer;
	public Image ManaBar;
	
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
		
		float ManaLeftX = (CurrMinManaPower / MaxManaPower) * MaxManaScale + LeftManaPosition;
		float ManaRightX = (CurrMaxManaPower / MaxManaPower) * MaxManaScale + LeftManaPosition;
		float ManaWidth = ManaRightX - ManaLeftX;

		
		ManaBar.rectTransform.position = new Vector3(ManaLeftX, ManaBar.rectTransform.position.y, ManaBar.rectTransform.position.z);
		ManaBar.rectTransform.sizeDelta = new Vector2(ManaWidth, ManaBar.rectTransform.sizeDelta.y);
		
	}
}
