using UnityEngine;
using System.Collections;

public class FollowMeCam : MonoBehaviour {

	public GameObject ObjectToFollow = null;

	public float TimeToMove = 0.5f;
	public float MouseSpring = 1.0f;
	public float CutoffMouseMagnitude = 0.8f;
	
	public float ZoomInSubstractedDistance = 5;
	public float ZoomOutAddedDistance = 5;

	public float CurrentShakeAmount = 0.0f;
	public float MaxShakeAmount = 1.0f;
	public float MaxShakeTime = 2.0f;

	public bool IsZooming
	{
		get {return (ZoomDistance == StartDistance.magnitude);}
	}
	
	private float ZoomDistance;
	private float ZoomDistanceToBe;

	private Vector3 StartDistance;
	private Vector3 SmoothVelocity;
	
	public PlayerContoller AttachedController = null;

	// Use this for initialization
	void Start () {
		ZoomDistanceToBe = GetComponent<Camera>().fieldOfView;
		ZoomDistance = ZoomDistanceToBe;
		
		if (ObjectToFollow != null)
		{
			StartDistance = this.transform.position - ObjectToFollow.transform.position;
			ZoomDistanceToBe = StartDistance.magnitude;
		}
		else
			Debug.Log("FollowMeCam Script : Object To Follow not set");
	}
	
	public void ZoomIn()
	{
		ZoomDistanceToBe -= ZoomInSubstractedDistance;
	}
	
	public void ZoomOut()
	{
		ZoomDistanceToBe += ZoomOutAddedDistance;
	}
	
	public void ResetZoom()
	{
		ZoomDistanceToBe = StartDistance.magnitude;
	}
	
	// Update is called once per frame
	void Update () {
	
		//Adjust Field of View
		//GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, ZoomDistanceToBe, 0.01f);
		ZoomDistance = Mathf.Lerp(ZoomDistance, ZoomDistanceToBe, 0.1f);
		Vector3 FinalZoomDistance = StartDistance.normalized * ZoomDistance;
	
		Vector3 PlaceToBe = ObjectToFollow.transform.position + FinalZoomDistance;
		
		//Mouse Spring effect
		Vector2 MouseInput = (  new Vector2(Input.mousePosition.x, Input.mousePosition.y));
														
		Vector2 MouseInputVectorFromCenter = Vector2.zero;
		if (MouseInput.magnitude != 0)
			MouseInputVectorFromCenter = new Vector2( (MouseInput.x - Screen.currentResolution.width * 0.5f) / Screen.currentResolution.width,
													  (MouseInput.y - Screen.currentResolution.height * 0.5f) / Screen.currentResolution.height) * 2.0f;
							
												
		if (Mathf.Abs( MouseInputVectorFromCenter.x) > CutoffMouseMagnitude)																	
			PlaceToBe.x += MouseInputVectorFromCenter.x * MouseSpring;
			
		if (Mathf.Abs(MouseInputVectorFromCenter.y) > CutoffMouseMagnitude)																	
			PlaceToBe.z += MouseInputVectorFromCenter.y * MouseSpring;
		
		transform.position = Vector3.SmoothDamp(transform.position, PlaceToBe, ref SmoothVelocity, TimeToMove);
		
		//Shakey shakes
		if (CurrentShakeAmount > 0)
		{
			Vector3 Shakeyshakes;
			if (CurrentShakeAmount < MaxShakeAmount)
				Shakeyshakes = Random.insideUnitSphere * CurrentShakeAmount;
			else
				Shakeyshakes = Random.insideUnitSphere * MaxShakeAmount;
				
			transform.position += Shakeyshakes  * 1f;
			CurrentShakeAmount -= Time.deltaTime;
		}
	
	}
	
	public void AddShake(float ShakeAmount)
	{
		if (CurrentShakeAmount + ShakeAmount < MaxShakeTime)
			CurrentShakeAmount += ShakeAmount;
		else
			CurrentShakeAmount = MaxShakeTime;
	}
}
