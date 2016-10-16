using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GameObject levelButtonPrefab;
	public GameObject levelButtonContainer;

	private Transform cameraTransform;
	private Transform cameraDesiredLookAt;

	private void Start()
	{
		cameraTransform = Camera.main.transform;
		//using thumbnail through the object for UI 
		Sprite[] thumbnails = Resources.LoadAll<Sprite>("Levels");
		foreach (Sprite thumbnail in thumbnails) 
		{
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
	}

	private void Update()
	{
		
	}


	private void LoadLevel(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void LookAtMenu(Transform menuTransform)
	{
		Camera.main.transform.LookAt (menuTransform.position);
	}



}
