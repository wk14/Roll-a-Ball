using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	//instance will be available 
	private static GameManager instance;
	public static GameManager Instance{get{return instance; }}

	public int currentSkinIndex = 0; // tracking which skin is been used
	public int currency = 0;
	public int skinAvailability = 1;

	private void Awake() // load before Start() in MainMenu
	{
		instance = this;
		DontDestroyOnLoad (gameObject);

		//  check: has the game been played before? if not, create it. if yes, simply load the data
		if (PlayerPrefs.HasKey ("CurrentSkin")) { // is there any current skin saved under the registry?
			// if true, we have a previous session
			currentSkinIndex = PlayerPrefs.GetInt("CurrentSkin");
			currency = PlayerPrefs.GetInt ("Currency");
			skinAvailability = PlayerPrefs.GetInt ("SkinAvailability");
		} 
		else 
		{
			// create new game
			Save();
		}

	}

	public void Save()
	{
		PlayerPrefs.SetInt("CurrentSkin", currentSkinIndex);
		PlayerPrefs.SetInt ("Currency", currency);
		PlayerPrefs.SetInt ("SkinAvailability", skinAvailability);
	}


}
