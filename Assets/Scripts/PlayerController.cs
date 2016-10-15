using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 0.1f;
	public Text countText;
	public Text winText;
	public AudioClip collideSound;

	private Rigidbody rb;
	private int count;

	private AudioSource source;

	public Vector3 movement{ set; get; }
	public VirtualJoyStick joystick;



	void Awake()
	{
		source = GetComponent<AudioSource>(); 
	}

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		count = 0;
		setCountText ();
		winText.text = "";
		speed = 20;
	}

	void Update()
	{
		movement = dirInput ();
		Move ();
	} 


	private void Move()
	{
		rb.AddForce (movement * speed);
	}


	private Vector3 dirInput()
	{
		Vector3 dir = Vector3.zero;

		#if UNITY_STANDALONE || UNITY_WEBPLAYER

		dir.x = Input.GetAxis ("Horizontal");
		dir.z = Input.GetAxis ("Vertical");

		return dir;

		#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

		dir.x = joystick.Horizontal();
		dir.z = joystick.Vertical();
		
		#endif

		if(dir.magnitude>1)
		{
			dir.Normalize();
		}

		return dir;
	}



	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			source.PlayOneShot (collideSound,1F);
			other.gameObject.SetActive (false);
			count = count + 1;
			setCountText ();
		}
	}



	void setCountText()
	{
		countText.text = "Score: " + count.ToString ();
		if (count >= 66) {
			winText.text = "<3愛你<3<3"; 
		}
	}



}
