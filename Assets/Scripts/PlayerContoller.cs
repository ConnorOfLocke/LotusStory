using UnityEngine;
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
	
	public float SpellProjectorHeight = 20.0f;
	public GameObject FireBallAimEffect = null;
	public GameObject FireBallProjectile = null;
	
	public PLAYER_SPELL HeldSpell = PLAYER_SPELL.SPELL_NONE;
	public float CurrMaxManaPower = 6.0f;
	public float CurrMinManaPower = 5.0f;
	public float MaxManaPower = 10.0f;
	public float MinMaxPower = 0.0f;
	
	public float SpellHoldTime = 0.0f;
	
	private Plane GroundPlane;
	private Camera AttachedCamera;
	
	private bool RightMouseDown = false;
	
	private ProjectorLightFadeInAndExpand HeldSpellEffect;
	
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
					GameObject pewpew = GameObject.Instantiate(FireBallProjectile, TargetPlayer.transform.position, Quaternion.identity) as GameObject;
					pewpew.GetComponent<Projectile>().Destination = mouseRay.GetPoint(RayDistance);
					pewpew.GetComponent<Projectile>().Source = TargetPlayer.transform.position;
					pewpew.GetComponent<Projectile>().GivenPower = SpellHoldTime;
				}
				Destroy(HeldSpellEffect.gameObject);
				HeldSpellEffect = null;
				HeldSpell = PLAYER_SPELL.SPELL_NONE;
				SpellHoldTime = 0;
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
			else if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.Q))
			{
				Vector3 HitPos = mouseRay.GetPoint(RayDistance);
				Vector3 Direction = (HitPos - TargetPlayer.transform.position);
			
				HeldSpell = PLAYER_SPELL.SPELL_FIREBALL;
				if (HeldSpellEffect == null)
					HeldSpellEffect = (GameObject.Instantiate(FireBallAimEffect) as GameObject).GetComponent<ProjectorLightFadeInAndExpand>();
					
				HeldSpellEffect.gameObject.transform.position = HitPos + new Vector3(0, SpellProjectorHeight, 0);
				
				TargetPlayer.SetTargetPosition(TargetPlayer.transform.position);
				TargetPlayer.SetTargetRotation(Quaternion.LookRotation(Direction));
				SpellHoldTime += Time.deltaTime;
			}
			//Spell 2
			else if (Input.GetMouseButton(1) && Input.GetKeyDown(KeyCode.W))
			{
				
				
			}
			//spell 3
			else if (Input.GetMouseButton(1) && Input.GetKeyDown(KeyCode.E))
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
