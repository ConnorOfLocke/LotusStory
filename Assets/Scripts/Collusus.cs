using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collusus : MonoBehaviour {

	public HexGridGenerator HexGenerator;
	
	public float Speed = 1.0f;
	public float StopDistance = 0.5f;
	public float EaseOffDistance = 5.0f;
	
	public float RotatationDelta = 0.3f;
	
	private Vector3 PositionToBe;
	private Quaternion RotationToBe;
	private Vector3 Velocity;
	
	private Vector3 StartPos;
	private Vector3 EndPos;
	private CharacterController AttachedController;
	
	// Use this for initialization
	void Start () {
	
		AttachedController = GetComponent<CharacterController>();
	
		List<GameObject> hex_chunks = HexGenerator.GetGeneratedChunks();
		int grid_width = HexGenerator.grid_width;
		int grid_height = HexGenerator.grid_height;
		
		int hex_chunk_start_index = (int)Random.Range(0, grid_height);
		int hex_chunk_end_index = (int)Random.Range(0, grid_height);
		
		GameObject StartHexChunk = hex_chunks[hex_chunk_start_index * grid_width];
		GameObject EndHexChunk = hex_chunks[hex_chunk_end_index * grid_width + (grid_width - 1) ];
		
		//start at the StartHexChunk
		float y_pos = transform.position.y;
		transform.position = StartHexChunk.transform.position;
		transform.position = new Vector3( transform.position.x, y_pos, transform.position.z);
		
		StartPos = StartHexChunk.transform.position;
		EndPos = EndHexChunk.transform.position;
		StartPos.y = y_pos;
		EndPos.y = y_pos;
		SetTargetPosition(EndPos);
	
	}
	
	public void SetTargetPosition(Vector3 Position)
	{
		PositionToBe = new Vector3(Position.x, PositionToBe.y, Position.z);
		RotationToBe = Quaternion.LookRotation( PositionToBe - transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
		Debug.DrawLine(StartPos + new Vector3(0, 10, 0), EndPos + new Vector3(0, 10, 0), Color.magenta);
		Debug.DrawLine(transform.position, transform.position + (EndPos - transform.position).normalized * 10, Color.green); 
		
		//Rotate the things
		transform.rotation = Quaternion.Slerp(transform.rotation, RotationToBe, RotatationDelta);
		if ( Vector3.Distance(RotationToBe.eulerAngles, transform.rotation.eulerAngles) < 5)
			transform.rotation = RotationToBe;
			
		float Distance = Vector3.Distance(transform.position, PositionToBe);
	
		//Move Towards the thing
		if (Distance > StopDistance)
		{
			//Vector3 Direction = (PositionToBe - transform.position).normalized;
			Vector3 Direction = (transform.rotation * new Vector3(0, 0, 1));
			Vector3 Movement = Direction.normalized * Speed * Time.deltaTime;
			Movement.y = 0;
			
			//slows down near destination
			if (Distance < EaseOffDistance)
			{
				Movement = Movement * Distance / EaseOffDistance;
			}
			
			AttachedController.Move(Movement);
		}
		else
		{
			AttachedController.Move( Vector3.zero);
		}
	}

}
