using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler,IPointerDownHandler {

	private Image bgImg;
	private Image joystickImg;
	private Vector3 inputVec;

	private void Start()
	{
		//declares the variables
		bgImg = GetComponent<Image>();
		joystickImg = transform.GetChild (0).GetComponent<Image> ();

	}

	// creates three evidences: OnPointerDown, OnPointerUp, OnDrag
	public virtual void OnDrag(PointerEventData ped)
	{
		Vector2 pos;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos)) 
		{
			pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);  // getting 0~1 value 
			pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);
			// the above still doesn't set the center of joystick to (0,0), therefore
			inputVec = new Vector3(pos.x*2 + 1, 0, pos.y*2 -1 );
			inputVec = (inputVec.magnitude > 1.0f) ? inputVec.normalized : inputVec; //normalized to be within the circle
			// Move Joystick Image
			joystickImg.rectTransform.anchoredPosition = 
				new Vector3 (inputVec.x * (bgImg.rectTransform.sizeDelta.x / 3), inputVec.z * (bgImg.rectTransform.sizeDelta.y / 3)); //z component is 0
			

		}
	}

	public virtual void OnPointerDown(PointerEventData ped)  
	{
		OnDrag (ped);
	}

	public virtual void OnPointerUp(PointerEventData ped)
	{
		//reset the Joystick
		inputVec = Vector3.zero;
		joystickImg.rectTransform.anchoredPosition = Vector3.zero;
	}


	public float Horizontal()
	{
		if (inputVec.x != 0) {
			return inputVec.x;
		} else {
			return Input.GetAxis ("Horizontal");
		}
	}

	public float Vertical()
	{
		if (inputVec.z != 0) {
			return inputVec.z;
		}else{
			return Input.GetAxis ("Vertical");
	}
}
}