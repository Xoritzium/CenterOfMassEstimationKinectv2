using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Windows.Kinect;
using Toggle = UnityEngine.UI.Toggle;


public class AngleVisualizer : MonoBehaviour
{
	/* Ablauf:
     * Bekommen der Positionen von Gelenk, sowie den Vectoren der Richtungen
     * Mappen der Vectoren in kürzer von Gelenk in Richtung des jeweiligen Gliedmaßes
     * mappen auf ColorSpacepoint 
     * UiLineRenderer mit jeweils einem "singleJointManager"
     *
     * */
	[SerializeField] KinectSingleBodyManager kinectSingleBodyManager;
	[SerializeField] JointAngleManager angleManager;
	[SerializeField] GameObject SingleJointAngle;
	[SerializeField] TextMeshProUGUI angleTextDigits;
	[SerializeField] Toggle toggleAngles;

	[SerializeField] float lineThickness;
	[SerializeField] float angleLength;
	private AngleContainer[] _singleJointAngle;

	private float mirrorLine;

	private CoordinateMapper coordinateMapper;

	public bool showJointAngles;

	private string emptyAngles = "0 \n0 \n0 \n0 \n0 \n0 \n0 \n0";


	// Start is called before the first frame update
	void Start()
	{
		_singleJointAngle = new AngleContainer[8];
		for (int i = 0; i < _singleJointAngle.Length; i++)
		{
			_singleJointAngle[i].joint = Instantiate(SingleJointAngle);
			_singleJointAngle[i].joint.transform.SetParent(this.transform);
			_singleJointAngle[i].joint.SetActive(false);
			_singleJointAngle[i].lineRenderer = _singleJointAngle[i].joint.GetComponent<UILineRenderer>();
			_singleJointAngle[i].lineRenderer.thickness = lineThickness;
		}
		coordinateMapper = KinectSensor.GetDefault().CoordinateMapper;
		mirrorLine = Screen.height / 2;
		toggleAngles.onValueChanged.AddListener(delegate { DisplayAngles(toggleAngles.isOn); });
	}

	// Update is called once per frame
	void Update()
	{
		if (showJointAngles && kinectSingleBodyManager.TrackingState)
		{
			ShowJointAngles();
		}
		if (!showJointAngles)
		{
			HideJointAngles();
		}
	}

	private void ShowJointAngles()
	{
		AngledJoint[] aj = angleManager.AngledJoints;
		string angleDigits = "";
		for (int i = 0; i < aj.Length; i++)
		{ // change back to aj.Length
		  // draw angles onto canvas, they need to be mirrored, like the masses
			_singleJointAngle[i].joint.SetActive(true);
			_singleJointAngle[i].lineRenderer.points = GetColorSpacePointsFromVectors(aj[i]);
			_singleJointAngle[i].lineRenderer.SetAllDirty();
			angleDigits += Mathf.Round(aj[i].angle) + "°\n";
		}
		angleTextDigits.text = angleDigits;
	}

	private void HideJointAngles()
	{
		for (int i = 0; i < _singleJointAngle.Length; i++)
		{
			// draw angles onto canvas
			_singleJointAngle[i].joint.SetActive(false);
		}
		angleTextDigits.text = emptyAngles;

	}

	private void DisplayAngles(bool val)
	{
		showJointAngles = val;
	}


	private Vector2[] GetColorSpacePointsFromVectors(AngledJoint joint)
	{
		ColorSpacePoint[] colorPoint = new ColorSpacePoint[3];
		CameraSpacePoint[] cameraPoint = new CameraSpacePoint[3];
		Vector2[] ret = new Vector2[3];
		for (int i = 0; i < 3; i++)
		{
			cameraPoint[i].X = joint.data[i].x;
			cameraPoint[i].Y = joint.data[i].y;
			cameraPoint[i].Z = joint.data[i].z;
			colorPoint[i] = coordinateMapper.MapCameraPointToColorSpace(cameraPoint[i]);
			ret[i] = Vector2.zero;
		}
		//flip data on y according to rgb view

		for (int i = 0; i < 3; i++)
		{
			float diffMirrorLine = (colorPoint[i].Y - mirrorLine);
			ret[i].x = colorPoint[i].X;
			ret[i].y = mirrorLine - diffMirrorLine;
		}
		//shorten line
		Vector2 ab = ret[0] - ret[1];
		Vector2 ac = ret[2] - ret[1];

		ret[0] = ret[1] + ab * angleLength;
		ret[2] = ret[1] + ac * angleLength;
		// 0 - Axy, 1- Jointxy, 2 - Bxy

		return ret;

	}

	private struct AngleContainer
	{
		public GameObject joint;
		public UILineRenderer lineRenderer;
	}


}
