using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class RecorderManager : MonoBehaviour
{

	[SerializeField] CalculateCenterOfMass massCalculator;
	[SerializeField] ResolveKinectJointDataIntoMasses kinectResolver;
	[SerializeField] KinectSingleBodyManager singleBodyManager;
	[SerializeField] string filePath;

	int startRecord = 0;
	int recordedFrames = 0;
	/**
	 * because unity transforms floating point with a comma insteat of a point, seperation sign: ; -> semicolon
	 * */
	private string path;
	private string fileHeader = "Frame;Xpos;Ypos;Zpos; tracked";
	private string currentlyToSave = "";
	private string[] toFile;

	private List<string> savingValues; // might be smarter to convert it into bytes to save RAM

	private bool recording = false;

	void Start()
	{
		path = Application.dataPath + "/" + filePath + ".csv";
		Debug.Log("csv Data will be writen to " + path);

		savingValues = new List<string>();
	}


	private void Update()
	{
		if (recording && singleBodyManager.TrackingState)
		{
			SaveMassData();
		}
		if (recording && !singleBodyManager.TrackingState)
		{
			SaveEmptyMassData();
		}
	}
	/// <summary>
	/// save Center of Mass data of current frame
	/// </summary>
	private void SaveMassData()
	{
		Vector3 currentPosition = massCalculator.GetCenterOfMass();
		currentlyToSave =
			Time.frameCount + $";{currentPosition.x};{currentPosition.y};{currentPosition.z};" + true;
		savingValues.Add(currentlyToSave);
	}
	/// <summary>
	/// save an empty entry, if nothing is tracked
	/// </summary>
	private void SaveEmptyMassData()
	{
		currentlyToSave = Time.frameCount + ";X ;Y; Z;" + false;
		savingValues.Add(currentlyToSave);
	}
	/// <summary>
	/// save recordings into file
	/// is done by an extra thread, so wait until file is written before closing the app
	/// </summary>
	private void SaveRecordingToFile()
	{
		Debug.Log("start Saving to File: " + filePath);

		TextWriter tw = new StreamWriter(path, false); // false: override previous things in the file
		tw.WriteLine(fileHeader); // every column one entry, its solved by semicolon
		tw.Close();

		tw = new StreamWriter(path, true);
		for (int i = 0; i < savingValues.Count; i++)
		{
			tw.WriteLine(savingValues[i]);
		}
		Debug.Log("saving to file finished: " + filePath);

	}

	#region public methods

	public void StartRecording()
	{
		recording = true;
		startRecord = Time.frameCount;
	}
	public void StopRecording()
	{
		recording = false;
		recordedFrames = Time.frameCount - startRecord;
		var savingTask = Task.Run(() => { SaveRecordingToFile(); }); // !!
	}
	public int GetRecordedFrames()
	{
		return recordedFrames;
	}
	#endregion
}
