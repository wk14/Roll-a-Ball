using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

	private const float CAMERA_TRANSITION_SPEED = 3.0f;

	public GameObject levelButtonPrefab;
	public GameObject levelButtonContainer;

	public GameObject ShopButtonPrefab;
	public GameObject ShopItemContainer;

	private Transform cameraTransform;
	private Transform cameraDesiredLookAt;

	public Text currecyText;

	public Material playerMaterial;

	private void Start()
	{
		ChangePlayerSkin (GameManager.Instance.currentSkinIndex);
		currecyText.text = "Currency: "+GameManager.Instance.currency.ToString();

		cameraTransform = Camera.main.transform;
		//using thumbnail through the object for UI 
		Sprite[] thumbnails = Resources.LoadAll<Sprite>("Levels");
		foreach (Sprite thumbnail in thumbnails) 
		{
			// Selecting Levels
			//instantiates container as a GameObject
			GameObject container = Instantiate (levelButtonPrefab) as GameObject;
			container.GetComponent<Image> ().sprite = thumbnail;
			// sets the prefab to spawn in the levelButtonContainer parent
			// false -> tells the object not to stay at its world position but relative to its parent
			container.transform.SetParent (levelButtonContainer.transform, false);

			// when clicking on thumbnail, sceneName will be obtained on click
			string sceneName = thumbnail.name;
			container.GetComponent<Button> ().onClick.AddListener ( () => LoadLevel(sceneName));
		}

		// Selecting textures
		int textureIndex = 0;
		Sprite[] textures = Resources.LoadAll<Sprite> ("Player");
		foreach (Sprite texture in textures) 
		{
			GameObject container = Instantiate (ShopButtonPrefab) as GameObject;
			container.GetComponent<Image> ().sprite = texture;
			container.transform.SetParent (ShopItemContainer.transform, false);

			int index = textureIndex;
			container.GetComponent<Button> ().onClick.AddListener ( () => ChangePlayerSkin (index));
			if ((GameManager.Instance.skinAvailability & 1 << index) == 1 << index) 
			{
				container.transform.GetChild (0).gameObject.SetActive (false);
			}
			textureIndex++;
		}
	}

	private void Update()
	{
		if (cameraDesiredLookAt != null) // if it exists
		{
			cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraDesiredLookAt.rotation, CAMERA_TRANSITION_SPEED*Time.deltaTime);
		}
	}


	private void LoadLevel(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void LookAtMenu(Transform menuTransform)
	{
		cameraDesiredLookAt = menuTransform;
		//Camera.main.transform.LookAt (menuTransform.position);
	}


	private void ChangePlayerSkin(int index)
	{
		if ((GameManager.Instance.skinAvailability & 1 << index) == 1 << index) { // bit flag


			float x = (index % 4) * 0.25f;
			float y = 0.75f - ((int)index / 4) * 0.25f;

			// index starts from bottom left of the 4x4 textures, therefore we flip y to the top
			//y = 0.75f - y;

			playerMaterial.SetTextureOffset ("_MainTex", new Vector2 (x, y));
			GameManager.Instance.currentSkinIndex = index;
			GameManager.Instance.Save (); 
		} 
		else 
		{
			// when the player doesn't have the skin, decide to buy or not
			int cost = 100;
			if (GameManager.Instance.currency >= cost) 
			{
				GameManager.Instance.currency -= cost;
				GameManager.Instance.skinAvailability += 1 << index;
				GameManager.Instance.Save ();
				currecyText.text = "Currency: "+GameManager.Instance.currency.ToString();

				ShopItemContainer.transform.GetChild (index).GetChild (0).gameObject.SetActive (false);
				ChangePlayerSkin (index);
			}
		}
	}
}
