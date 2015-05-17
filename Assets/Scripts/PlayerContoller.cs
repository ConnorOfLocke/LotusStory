﻿using UnityEngine;
using System.Collections;

public enum PLAYER_SPELL
{
	SPELL_NONE,
	SPELL_FIREBALL,
	SPELL_2,
	SPELL_3
}

public class PlayerContoller : MonoBehaviour {

	public Player TargetPlayer;
	public GameObject MouseLeftClickEffect = null;
	public GameObject MouseRightClickEffect = null;
	public GameObject SelectedObject = null;
	public float MaxDistanceToSelectObject = 20.0f;
	
	public GameObject FireBallAimEffect = null;
	public GameObject FireBallProjectile = null;
	
	private Plane GroundPlane;
	private Camera AttachedCamera;
	
	private bool RightMouseDown = false;
	
	private PLAYER_SPELL HeldSpell = PLAYER_SPELL.SPELL_NONE;
	
	// Use this for initialization
	void Start () {
	
		GroundPlane = new Plane( new Vector3(0, -1, 0), TargetPlayer.transform.position);
		AttachedCamera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
	
		Ray mouseRay = AttachedCamera.ScreenPointToRay(Input.mousePosition);
		
		float RayDistance = 0.0f;
		if (GroundPlane.Raycast(mouseRay, out RayDistance))
		{
			//cast any spells currently being held down
			if (!Input.GetMouseButton(1) && HeldSpell != PLAYER_SPELL.SPELL_NONE)
			{
				if (HeldSpell == PLAYER_SPELL.SPELL_FIREBALL)
				{
					//FIRE THE FIREBALL
				}
				//AND TEH REST LATER
				
				HeldSpell = PLAYER_SPELL.SPELL_NONE;
			}
			////////////////////////////////////////////
			//MOVEMENT
			if (Input.GetMouseButton(0))
			{
				if (SelectedObject != null)
				{
					SelectedObject.GetComponent<InteractableObject>().OnClickOffEvent();
					SelectedObject = null;
				}
			
				Vector3 PositionToBe = mouseRay.GetPoint(RayDistance);
				TargetPlayer.SetTargetPosition(PositionToBe);
				
				if (MouseLeftClickEffect != null)
					GameObject.Instantiate(MouseLeftClickEffect, PositionToBe + new Vector3(0, 1, 0), Quaternion.identity);
			}
			
			//////////////////////////////////////////////////////////////
			//FIRE BALL SPELL / DRAIN
			else if (Input.GetMouseButtonDown(1) && Input.GetKeyDown(KeyCode.Q))
			{
				
			
			}
			//FIRE BALL SPELL
			else if (Input.GetMouseButtonDown(1) && Input.GetKeyDown(KeyCode.Q))
			{
				
				
			}
			//FIRE BALL SPELL
			else if (Input.GetMouseButtonDown(1) && Input.GetKeyDown(KeyCode.Q))
			{
				
				
			}
			
			//////////////////////////////////////////////////////////////
			//Interactables
			else if (Input.GetMouseButtonDown(1) && !RightMouseDown)
			{
				RaycastHit Hit;
				Physics.Raycast(mouseRay, out Hit);
				if (Hit.transform.gameObject.tag == "Interactable")
				{
				
					//do effect
					GameObject.Instantiate(MouseRightClickEffect, Hit.point, Quaternion.identity);

					//rotate the player
					Vector3 HitPos = Hit.transform.position;
					HitPos.y = TargetPlayer.transform.position.y;
					
					Vector3 Direction = (HitPos - TargetPlayer.transform.position);
					
					if (Direction.magnitude < MaxDistanceToSelectObject)
					{
						RightMouseDown = true;
						TargetPlayer.SetTargetPosition(TargetPlayer.transform.position);
						TargetPlayer.SetTargetRotation(Quaternion.LookRotation(Direction));
					
						//select object
						Hit.transform.gameObject.GetComponent<InteractableObject>().OnClickOnEvent();
						if (SelectedObject != null && SelectedObject != Hit.transform.gameObject)
							SelectedObject.GetComponent<InteractableObject>().OnClickOffEvent();
						SelectedObject = Hit.transform.gameObject;
					}
					else if (SelectedObject != null)
					{
						SelectedObject.GetComponent<InteractableObject>().OnClickOffEvent();
						SelectedObject = null;
					}
				}
			}
			else if (Input.GetMouseButtonUp(1) && RightMouseDown)
				RightMouseDown = false;
				
		}
	
	}
}
