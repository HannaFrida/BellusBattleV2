using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeListener : MonoBehaviour
{
	private void Start()
	{
		SceneChangeEvent.RegisterListener(ChangeScene);
	}

	private void ChangeScene(SceneChangeEvent sce)
	{
		SceneManager.LoadScene(sce.scene);
		//GameManager.Instance.UpdateGameState(sce.state);
	}

	private void OnDestroy()
	{
		SceneChangeEvent.UnregisterListener(ChangeScene);
	}
}
