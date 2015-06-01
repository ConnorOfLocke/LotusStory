using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collusus : MonoBehaviour {

	public HexGridGenerator HexGenerator;
	
	public float Speed = 1.0f;
	public float StopDistance = 0.5f;
	public float EaseOffDistance = 5.0f;
	
	public float RotatationDelta = 0.3f;
	
	public float PathTowardsTownChance = 1.0f;
	
	private Vector3 PositionToBe;
	private Quaternion RotationToBe;
	private Vector3 Velocity;
	
	private Vector3 StartPos;
	private Vector3 EndPos;
	private CharacterController AttachedController;
	
	private float y_pos;
	
	private List<Vector3> DetourPositions = null;
	private List<GameObject> LeftEdgeHexs = null;
	private List<GameObject> RightEdgeHexs = null;
	private List<GameObject> TownList = null;
	
	private bool isTravelLeftToRight = true;
	
	// Use this for initialization
	void Start () {		
	
		AttachedController = GetComponent<CharacterController>();
		if (DetourPositions == null)
			DetourPositions = new List<Vector3>();
	}
	
	public void RecalculatePath()
	{
		if (isTravelLeftToRight)
		{
			int hex_chunk_start_index = (int)Random.Range(0, LeftEdgeHexs.Count);
			int hex_chunk_end_index = (int)Random.Range(0, RightEdgeHexs.Count);
		
			StartPos = LeftEdgeHexs[hex_chunk_start_index].transform.position;
			EndPos = RightEdgeHexs[hex_chunk_end_index].transform.position;
			isTravelLeftToRight = false;
		}
		else
		{
			int hex_chunk_start_index = (int)Random.Range(0, RightEdgeHexs.Count);
			int hex_chunk_end_index = (int)Random.Range(0, LeftEdgeHexs.Count);
			
			StartPos = RightEdgeHexs[hex_chunk_start_index].transform.position;
			EndPos = LeftEdgeHexs[hex_chunk_end_index].transform.position;
			isTravelLeftToRight = true;
		}
		
		StartPos.y = y_pos;
		EndPos.y = y_pos;
		
		SetTargetPosition(EndPos);
		
		
		if ( Random.value < PathTowardsTownChance)
		{
			int town_detour_index = (int)Random.Range(0, TownList.Count);
			Vector3 TownPos = TownList[town_detour_index].transform.position;
			TownPos.y = y_pos;
			SetDetour(TownPos);
		}
	}
	
	public void Initialise(List<GameObject> a_LeftEdgeHexs, List<GameObject> a_RightEdgeHexs, List<GameObject> a_TownList)
	{
		LeftEdgeHexs = a_LeftEdgeHexs;
		RightEdgeHexs = a_RightEdgeHexs;
		TownList = a_TownList;
		
		//creates first path
		RecalculatePath();
		
		//start at the StartHexChunk
		y_pos = transform.position.y;
		transform.position = StartPos;
	}
	
	public void SetTargetPosition(Vector3 Position)
	{
		PositionToBe = new Vector3(Position.x, y_pos, Position.z);
		RotationToBe = Quaternion.LookRotation( PositionToBe - transform.position);
	}
	
	public void SetDetour(Vector3 Position)
	{	
		if (DetourPositions == null)
			DetourPositions = new List<Vector3>();
	
		if (!DetourPositions.Contains(Position))
		{
			DetourPositions.Add( new Vector3(Position.x, y_pos, Position.z) );
			SetTargetPosition(new Vector3(Position.x, y_pos, Position.z) );
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		Debug.DrawLine(StartPos + new Vector3(0, 10, 0), EndPos + new Vector3(0, 10, 0), Color.magenta);
		Debug.DrawLine(transform.position + new Vector3(0, 10, 0), transform.position + (EndPos - transform.position).normalized * 10 + new Vector3(0, 10, 0), Color.green); 
		
		//Rotate the things
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation( PositionToBe - transform.position), RotatationDelta);
			
		float Distance = Vector3.Distance(transform.position, PositionToBe);
	
		//Move Towards the thing
		if (Distance > StopDistance)
		{
			//Vector3 Direction = (PositionToBe - transform.position).normalized;
			Vector3 Direction = (transform.rotation * new Vector3(0, 0, 1));
			Vector3 Movement = Direction.normalized * Speed * Time.deltaTime;
			Movement.y = 0;

			AttachedController.Move(Movement);
		}
		else
		{
			if (DetourPositions.Count > 0)
			{
				SetTargetPosition(DetourPositions[DetourPositions.Count - 1]);
				DetourPositions.RemoveAt(DetourPositions.Count - 1);
			}
			else
			{
				SetTargetPosition(EndPos);
			}
		}
		
		if ( (DetourPositions.Count == 0) && Distance < StopDistance)
			RecalculatePath();
		
		//for not breaking purposes
		//transform.position = new Vector3(transform.position.x, y_pos, transform.position.z);
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.gameObject.tag == "Wall")
		{
			RecalculatePath();
		}
	
	}
}
