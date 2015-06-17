using UnityEngine;
using System.Collections;

public enum PLAYER_SPELL
{
	SPELL_NONE,
	SPELL_FIREBALL,
	SPELL_ICICLE,
	SPELL_3
}

public class PlayerContoller : MonoBehaviour {

	public Player TargetPlayer;
	public GameObject MouseLeftClickEffect = null;
	public GameObject MouseRightClickEffect = null;
	public GameObject SelectedObject = null;
	public float MaxDistanceToSelectObject = 20.0f;
	public AudioSource ChargeSoundSource;
	public AudioSource SpellCastSource;
	
	public float SpellProjectorHeight = 20.0f;
	public GameObject FireBallAimEffect = null;
	public GameObject FireBallProjectile = null;
	public float FireBallMaxDistance = 40.0f;
	
	public GameObject IcicleAimEffect = null;
	public GameObject IcicleProjectile = null;
	
	public PLAYER_SPELL HeldSpell = PLAYER_SPELL.SPELL_NONE;
	public float CurrMaxManaPower = 6.0f;
	public float CurrMinManaPower = 5.0f;
	public float MaxManaPower = 10.0f;
	public float MinManaPower = 0.0f;
	
	public float SpellHoldTime = 0.0f;
	
	public float ManaAbsorbPerSecond = 1.0f;
	public GameObject SpellCastingEffect;
	public GameObject ManaAbsorbEffect;
	private GameObject CurrentEffect = null;
	private bool LastHeldEffect = false; //true for spell, false for absorb
	
	private Plane GroundPlane;
	private Camera AttachedCamera;
	
	private bool RightMouseDown = false;
	
	private ProjectorLightFadeInAndExpand HeldSpellEffect;
	
	// Use this for initialization
	void Start () {
	
		GroundPlane = new Plane( new Vector3(0, -1, 0), TargetPlayer.transform.position);
		AttachedCamera = GetComponent<Camera>();
		ChargeSoundSource.volume = 0.01f;
		
	}
	
	// Update is called once per frame
	void Update () {
	
		Ray mouseRay = AttachedCamera.ScreenPointToRay(Input.mousePosition);

		
		float RayDistance = 0.0f;
		if (GroundPlane.Raycast(mouseRay, out RayDistance))
		{
			Vector3 HitPos = mouseRay.GetPoint(RayDistance);
			Vector3 Direction = (HitPos - TargetPlayer.transform.position);
			HitPos.y = TargetPlayer.transform.position.y;
		
			//cast any spells currently being held down
			if (!Input.GetMouseButton(1) && HeldSpell != PLAYER_SPELL.SPELL_NONE)
			{
				FollowMeCam cam = FindObjectOfType<FollowMeCam>();
				if (cam != null)
					cam.AddShake(1.0f);
			
				ChargeSoundSource.volume = 0.0f;
				//SpellCastSource.pitch = 1.0f + Random.value * 0.05f;
				//SpellCastSource.Play();
			
				if (HeldSpell == PLAYER_SPELL.SPELL_FIREBALL)
				{
					GameObject pewpew = GameObject.Instantiate(FireBallProjectile, TargetPlayer.transform.position, Quaternion.identity) as GameObject;
					
					Vector3 DestinationPos = mouseRay.GetPoint(RayDistance);
					if (Vector3.Distance(DestinationPos,TargetPlayer.transform.position) > FireBallMaxDistance)
					{
						DestinationPos = TargetPlayer.transform.position + 
										(DestinationPos - TargetPlayer.transform.position).normalized * (float)FireBallMaxDistance; 
					
						pewpew.GetComponent<Projectile>().Destination = DestinationPos;
					}
					else
						pewpew.GetComponent<Projectile>().Destination = DestinationPos;
					
					
					pewpew.GetComponent<Projectile>().Source = TargetPlayer.transform.position;
					pewpew.GetComponent<Projectile>().GivenPower = SpellHoldTime;
				}
				else if (HeldSpell == PLAYER_SPELL.SPELL_ICICLE)
				{
					GameObject pewpew = GameObject.Instantiate(IcicleProjectile, TargetPlayer.transform.position, Quaternion.identity) as GameObject;
					pewpew.GetComponent<Projectile>().Destination = mouseRay.GetPoint(RayDistance);
					pewpew.GetComponent<Projectile>().Source = TargetPlayer.transform.position;
					pewpew.GetComponent<Projectile>().GivenPower = SpellHoldTime;
				
				}
				
				float SpellPower = SpellHoldTime - CurrMinManaPower;
				if (CurrMinManaPower - SpellPower < 0)
					SpellPower = CurrMinManaPower - MinManaPower;
				
				if (SpellPower > 0)
				{
					CurrMaxManaPower -= SpellPower;
					CurrMinManaPower -= SpellPower;
				}
				
				Destroy(HeldSpellEffect.gameObject);
				HeldSpellEffect = null;
				HeldSpell = PLAYER_SPELL.SPELL_NONE;
				SpellHoldTime = 0;
			}
			
			//ABSORBEN
			if (Input.GetKey(KeyCode.R))
			{
				if (CurrentEffect == null || LastHeldEffect)
				{
					Destroy(CurrentEffect);
					CurrentEffect = GameObject.Instantiate(ManaAbsorbEffect, TargetPlayer.transform.position + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
					LastHeldEffect = false;
					AttachedCamera.GetComponent<FollowMeCam>().ZoomIn();
				}
				
				FollowMeCam cam = FindObjectOfType<FollowMeCam>();
				if (cam != null)
					cam.AddShake(Time.deltaTime * 4.0f);
				
				CurrentEffect.transform.position = TargetPlayer.transform.position;
				TargetPlayer.SetTargetPosition(TargetPlayer.transform.position);
				TargetPlayer.SetTargetRotation(Quaternion.LookRotation( Direction));
				
				RaycastHit hitInfo;
				if (Physics.Raycast(TargetPlayer.transform.position, new Vector3(0 ,-1, 0), out hitInfo, LayerMask.NameToLayer("HexGrid")))
				{
					if (hitInfo.collider.gameObject.GetComponent<HexChunk>() != null)
					{
						if (hitInfo.collider.gameObject.GetComponent<HexChunk>().AbsorbMana())
						{
							if (CurrMaxManaPower < MaxManaPower)
							{
								CurrMaxManaPower += ManaAbsorbPerSecond * Time.deltaTime;
								CurrMinManaPower += ManaAbsorbPerSecond * Time.deltaTime;
							}
							else
								CurrMaxManaPower = MaxManaPower;
						}
					}
				}
				
			}
			//Spell effects
			else if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.E))
			{
				if (CurrentEffect == null || !LastHeldEffect)
				{
					Destroy(CurrentEffect);
					CurrentEffect = GameObject.Instantiate(SpellCastingEffect, TargetPlayer.transform.position + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
					LastHeldEffect = true;
					AttachedCamera.GetComponent<FollowMeCam>().ZoomOut();
				}
				
				TargetPlayer.SetTargetPosition(TargetPlayer.transform.position);
				TargetPlayer.SetTargetRotation(Quaternion.LookRotation( Direction));
			}
			else
			{
				if (CurrentEffect != null)
				{
					Destroy(CurrentEffect);
					AttachedCamera.GetComponent<FollowMeCam>().ResetZoom();
				}
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
			//FIRE BALL SPELL
			else if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.Q))
			{
				if (!ChargeSoundSource.isPlaying)
					ChargeSoundSource.Play();
				
				ChargeSoundSource.volume += Time.deltaTime * 10;
				if (ChargeSoundSource.volume > 0.7f)
					ChargeSoundSource.volume = 0.7f;
			
				FollowMeCam cam = FindObjectOfType<FollowMeCam>();
				if (cam != null)
					cam.AddShake(Time.deltaTime * 10.0f);
				
				HeldSpell = PLAYER_SPELL.SPELL_FIREBALL;
				if (HeldSpellEffect == null)
					HeldSpellEffect = (GameObject.Instantiate(FireBallAimEffect) as GameObject).GetComponent<ProjectorLightFadeInAndExpand>();
				
				
				if ( Vector3.Distance(TargetPlayer.transform.position, HitPos) > FireBallMaxDistance)
				{
					Vector3 EffectPos = TargetPlayer.transform.position + 
									 (HitPos - TargetPlayer.transform.position).normalized * (float)FireBallMaxDistance; 
									 
					HeldSpellEffect.gameObject.transform.position = EffectPos + new Vector3(0, SpellProjectorHeight, 0);
				}
				else		
					HeldSpellEffect.gameObject.transform.position = HitPos + new Vector3(0, SpellProjectorHeight, 0);
				
				
				
				TargetPlayer.SetTargetPosition(TargetPlayer.transform.position);
				TargetPlayer.SetTargetRotation(Quaternion.LookRotation(Direction));
				
				if (SpellHoldTime < CurrMinManaPower)
					SpellHoldTime = CurrMinManaPower;				
				else if (SpellHoldTime < CurrMaxManaPower)
					SpellHoldTime += Time.deltaTime;
			}
			//ICICLE 
			else if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.W))
			{
				if (!ChargeSoundSource.isPlaying)
					ChargeSoundSource.Play();
				
				ChargeSoundSource.volume += Time.deltaTime * 10;
				if (ChargeSoundSource.volume > 0.7f)
					ChargeSoundSource.volume = 0.7f;
				
				FollowMeCam cam = FindObjectOfType<FollowMeCam>();
				if (cam != null)
					cam.AddShake(Time.deltaTime * 6.0f);
				
				HeldSpell = PLAYER_SPELL.SPELL_ICICLE;
				if (HeldSpellEffect == null)
					HeldSpellEffect = (GameObject.Instantiate(IcicleAimEffect) as GameObject).GetComponent<ProjectorLightFadeInAndExpand>();
				
				HeldSpellEffect.gameObject.transform.position = HitPos + new Vector3(0, SpellProjectorHeight, 0);
				
				TargetPlayer.SetTargetPosition(TargetPlayer.transform.position);
				TargetPlayer.SetTargetRotation(Quaternion.LookRotation(Direction));
				
				if (SpellHoldTime < CurrMinManaPower)
					SpellHoldTime = CurrMinManaPower;				
				else if (SpellHoldTime < CurrMaxManaPower)
					SpellHoldTime += Time.deltaTime;
				
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
