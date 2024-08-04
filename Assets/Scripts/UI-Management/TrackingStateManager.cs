using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackingStateManager : MonoBehaviour
{
	[SerializeField] private GameObject activeTracking;
	[SerializeField] private GameObject notTracking;
	[SerializeField] private string nameOfMassVisualisationUIElement;
	[SerializeField] private ResolveKinectJointDataIntoMasses kinectResolver;
	[SerializeField] private KinectSingleBodyManager singleBodyManager;

	private GameObject massVisualizer;
	private bool isTracking;
	void Start()
	{
		isTracking = false;
		activeTracking.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if (massVisualizer == null) {
			massVisualizer = GameObject.Find(nameOfMassVisualisationUIElement); //doesn´t work beacause wanted object is inactive
		}

		isTracking = singleBodyManager.TrackingState;

		if (isTracking && !activeTracking.activeSelf) {
			activeTracking.SetActive(true);
			notTracking.SetActive(false);
			massVisualizer.SetActive(true);
		}
		if (!isTracking && !notTracking.activeSelf) {
			notTracking.SetActive(true);
			activeTracking.SetActive(false);
			if (massVisualizer != null) {
				massVisualizer.SetActive(false);
			}
		}
	}


	public void OnClickRestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public void OnClickQuit()
	{
		Application.Quit();
	}

}
