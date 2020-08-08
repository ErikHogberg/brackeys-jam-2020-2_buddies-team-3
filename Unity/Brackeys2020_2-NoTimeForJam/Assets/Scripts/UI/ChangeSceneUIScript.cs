using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneUIScript : MonoBehaviour
{
	public void ChangeScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}

}
