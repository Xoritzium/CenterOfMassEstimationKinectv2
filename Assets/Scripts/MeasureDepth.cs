using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class MeasureDepth : MonoBehaviour
{
	public MultiSourceManager multiSource;

	private KinectSensor kSensor = null; // sensor ^^
	private CoordinateMapper coordinateMapper = null; //map depth data onto colormap

	private CameraSpacePoint[] cameraSpacePoints = null;
	private ColorSpacePoint[] colorSpacePoints = null;

	private ushort[] depthData = null;
	public Texture2D depthTexture;

	private readonly Vector2 depthResolution = new(512, 480);

	private void Awake()
	{
		kSensor = KinectSensor.GetDefault();
		coordinateMapper = kSensor.CoordinateMapper;

		int arrraySize = (int)depthResolution.x * (int)depthResolution.y;
		cameraSpacePoints = new CameraSpacePoint[arrraySize];
		colorSpacePoints = new ColorSpacePoint[arrraySize];
	}


	private void Update()
	{
		
		if (multiSource == null) { // unity loses reference on multisource somehow
			multiSource = GameObject.Find("MultiSourceManager").GetComponent<MultiSourceManager>();
		}
		
		if (Input.GetKeyDown(KeyCode.Space)) {
			DepthToColor();
			depthTexture = CreateTexture();

		}

	}

	private void DepthToColor()
	{
		depthData = multiSource.GetDepthData(); //data for camera and color spacePoints
												//map data
		Debug.Log("depthData: " + depthData.Length + " cameraSpacePoints; " + cameraSpacePoints.Length);
			//coordinateMapper.MapDepthFrameToCameraSpace(depthData, cameraSpacePoints); // fill 2nd argument with depthData
			//coordinateMapper.MapDepthFrameToColorSpace(depthData, colorSpacePoints);


	}

	private Texture2D CreateTexture()
	{
		Texture2D newTexture = new Texture2D(1920, 1080, TextureFormat.Alpha8, false);
		newTexture.name = "depthTexture";
		for (int x = 0; x < 1920; x++) {
			for (int y = 0; y < 1080; y++) {
				newTexture.SetPixel(x, y, Color.red);
			}

		}
		foreach (ColorSpacePoint point in colorSpacePoints) {
			newTexture.SetPixel((int)point.X, (int)point.Y, Color.red);

		}
		newTexture.Apply();
		return newTexture;

	}

}
