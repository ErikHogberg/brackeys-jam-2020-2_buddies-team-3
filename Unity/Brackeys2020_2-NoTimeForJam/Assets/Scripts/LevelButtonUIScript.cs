using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButtonUIScript : MonoBehaviour
{

	public static Dictionary<string,bool> LevelProgress = new Dictionary<string, bool>();

	public string LevelScene;

	bool levelComplete => LevelProgress.ContainsKey(LevelScene) && LevelProgress[LevelScene];

	[Space]
	public GameObject CompletionIndicator;

	private void Start() {
		CompletionIndicator.SetActive(levelComplete);
	}

	public void ChangeScene()
	{
		SceneManager.LoadScene(LevelScene, LoadSceneMode.Single);
	}

}
