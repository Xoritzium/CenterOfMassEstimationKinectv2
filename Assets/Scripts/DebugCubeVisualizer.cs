using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class DebugCubeVisualizer : MonoBehaviour
{

	[SerializeField] private GameObject cube;
	private List<GameObject> cubeJoints;
	[SerializeField] int jointsCount;
	[SerializeField] private float cubeScale;


	void Start()
	{

		InstantiateCubes();
	}

	private void InstantiateCubes()
	{
		cubeJoints = new List<GameObject>();
		for (int i = 0; i < jointsCount; i++) {
			GameObject blub = Instantiate(cube);
			blub.transform.localScale = new(cubeScale, cubeScale, cubeScale);
			blub.transform.parent = this.transform;
			cubeJoints.Add(blub);
		}
	}

	public void UpdateCubeList(Body body)
	{
		int counter = 0;
		for (JointType jt = JointType.SpineBase; jt <= JointType.ThumbRight; jt++) { // iterate over all joints
			Vector3 v = new(body.Joints[jt].Position.X, body.Joints[jt].Position.Y, body.Joints[jt].Position.Z);

			cubeJoints[counter].transform.localPosition = v;
			counter++;

		}
	}
}
