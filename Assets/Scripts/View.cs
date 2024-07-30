
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Windows.Kinect;

using Toggle = UnityEngine.UI.Toggle;

public class View: MonoBehaviour
{
	public MultiSourceManager multiSourceManager; // for depth and rgb data
	public ResolveKinectJointDataIntoMasses kinectMassResolver;
	public KinectSingleBodyManager singleBodyManager;

	public RawImage kinectRGBFeed;
	public int individualMassesCount = 14;


	private KinectSensor sensor;
	private CoordinateMapper mapper;

	private List<ColorSpacePoint> individualMasses2DPositions;

	public RawImage absoluteCenterOfMass;
	private RawImage _absoluteCenterOfMass;

	public RawImage individualCenterOfMass;
	private RawImage[] _individualCenterOfMasses;

	private ColorSpacePoint coMColorPoint;

	float mirrorLine;


	private bool _displayIndividualMasses { get; set; }
	public bool DisplayIndividualMasses
	{
		get { return _displayIndividualMasses; }
		set { _displayIndividualMasses = value; }
	}
	[SerializeField] Toggle toggle;

	private void Start()
	{
		individualMasses2DPositions = new List<ColorSpacePoint>();
		_individualCenterOfMasses = new RawImage[individualMassesCount];
		for (int i = 0; i < individualMassesCount; i++)
		{
			individualMasses2DPositions.Add(new ColorSpacePoint());
			_individualCenterOfMasses[i] = Instantiate(individualCenterOfMass);
			_individualCenterOfMasses[i].transform.SetParent(transform, false);
			_individualCenterOfMasses[i].name = "individualCenter-" + i;
			_individualCenterOfMasses[i].gameObject.SetActive(false);
		}

		sensor = KinectSensor.GetDefault();
		mapper = sensor.CoordinateMapper;

		_absoluteCenterOfMass = Instantiate(absoluteCenterOfMass);
		_absoluteCenterOfMass.transform.SetParent(transform, false);
		_absoluteCenterOfMass.name = "absoluteCenterOfMass";

		coMColorPoint = new ColorSpacePoint();
		mirrorLine = (Screen.height / 2);

		toggle.onValueChanged.AddListener(delegate { DisplayIndividualMassesChanged(toggle.isOn); });
	}



	void Update()
	{
		kinectRGBFeed.texture = multiSourceManager.GetColorTexture();
		if (kinectMassResolver.Masses != null && singleBodyManager.TrackingState)
		{ // durch KinectSingleBodyManager.Traackinstate ersetzén!
			UpdateAllIndividualMasses(kinectMassResolver.Masses);
			DrawIndividualMassesOnTexture();
			_absoluteCenterOfMass.gameObject.SetActive(true);
		}
		else
		{
			_absoluteCenterOfMass.gameObject.SetActive(false);
		}



	}
	/// <summary>
	/// draw the absolute Center of Mass onto Canvas
	/// </summary>
	private void DrawCenterOfMassOnTexture()
	{
		Vector3 v = new Vector3(coMColorPoint.X, coMColorPoint.Y, 0);
		if (v.x == float.NegativeInfinity)
		{
			return;
		}
		float diffMirrorLinePoint = (v.y - mirrorLine); // recalculate due to flipped rgb feed, kann man auch in einzelne Methode auslagern.
		Vector3 jointPos = new(v.x, mirrorLine - diffMirrorLinePoint, v.z);
		_absoluteCenterOfMass.rectTransform.position = jointPos;

	}
	/// <summary>
	/// Update the individual masses and draw them onto screen, eg. upperarm,head,core etc.
	/// </summary>
	private void DrawIndividualMassesOnTexture()
	{
		if (_displayIndividualMasses && singleBodyManager.TrackingState)
		{ // ersetzen durch KinectSingleBodyManager.TrackingState
			for (int i = 0; i < _individualCenterOfMasses.Length; i++)
			{
				_individualCenterOfMasses[i].gameObject.SetActive(true);
				DrawSingleMass(_individualCenterOfMasses[i], individualMasses2DPositions[i]);
			}
		}
		else
		{
			for (int i = 0; i < _individualCenterOfMasses.Length; i++)
			{
				_individualCenterOfMasses[i].gameObject.SetActive(false);
			}
		}
	}
	/// <summary>
	/// update single mass position
	/// </summary>
	/// <param name="mass">mass the be updated</param>
	/// <param name="pos">new positio</param>
	private void DrawSingleMass(RawImage mass, ColorSpacePoint pos)
	{
		float mirror = (pos.Y - mirrorLine);
		Vector3 drawPos = new(pos.X, mirrorLine - mirror, 0f);
		mass.rectTransform.position = drawPos;
	}


	/// <summary>
	/// update the masses according to new positions.
	/// </summary>
	/// <param name="masses"></param>
	private void UpdateAllIndividualMasses(Vector3[] masses)
	{
		for (int i = 0; i < masses.Length; i++)
		{
			ColorSpacePoint colorSpacePoint = CreateColorSpacePointFromVector3(masses[i]);
			individualMasses2DPositions[i] = colorSpacePoint;
		}
	}

	private ColorSpacePoint CreateColorSpacePointFromVector3(Vector3 v)
	{
		ColorSpacePoint cameraPoint = new ColorSpacePoint();
		CameraSpacePoint csp = new CameraSpacePoint();
		csp.X = v.x;
		csp.Y = v.y;
		csp.Z = v.z;
		cameraPoint = mapper.MapCameraPointToColorSpace(csp);
		return cameraPoint;
	}
	/// <summary>
	/// ensure, that individual COMs are displayed according to UI
	/// </summary>
	/// <param name="val">if true, show individual COMs</param>
	private void DisplayIndividualMassesChanged(bool val)
	{
		_displayIndividualMasses = val;
	}



	#region public methods 




	/// <summary>
	/// update just the center of gravity eg  center of mass
	/// </summary>
	/// <param name="pos"></param>
	public void UpdateCenterOfMassPosition(Vector3 pos)
	{
		CameraSpacePoint csp = new CameraSpacePoint();
		csp.X = pos.x;
		csp.Y = pos.y;
		csp.Z = pos.z;
		coMColorPoint = mapper.MapCameraPointToColorSpace(csp);

		DrawCenterOfMassOnTexture();
	}



	#endregion public methods

}



