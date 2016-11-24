using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public GameObject pauseMenu; 

	private void Start()
	{
		pauseMenu.SetActive (false);
	}

	public void TogglePauseMenu()
	{
		pauseMenu.SetActive (!pauseMenu.activeSelf);
	}


	public void ToMenu()
	{
		SceneManager.LoadScene ("Menu");
	}

}
