using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	// Changement de Scene
	public void LoadScene(string sceneName) {
		Application.LoadLevel (sceneName);
	}
}
