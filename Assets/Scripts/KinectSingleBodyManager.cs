using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using Joint = Windows.Kinect.Joint;

[RequireComponent(typeof(BodySourceManager))]

public class KinectSingleBodyManager : MonoBehaviour
{

	private BodySourceManager bodySourceManager;

	private Body _body { get; set; }
	public Body Body { get { return _body; } }

	private bool _trackingState { get; set; }
	/// <summary>
	/// is true, if there is a tracked body
	/// </summary>
	public bool TrackingState { get { return _trackingState; } }

	// Start is called before the first frame update
	void Start()
	{
		bodySourceManager = GetComponent<BodySourceManager>();

	}

	// Update is called once per frame
	void Update()
	{
		if (bodySourceManager == null) {
			if (_trackingState) { _trackingState = false; }
			return;
		}
		Body[] BodiesData = bodySourceManager.GetData(); // recieve body data of tracked people
		if (BodiesData == null) {
			if (_trackingState) { _trackingState = false; }
			return;
		}
		foreach (Body b in BodiesData) { // make sure, theres always the data of a tracked body
			if (b.IsTracked) {
				_body = b;
				break;
			}
		}
		if (_body == null || !_body.IsTracked) { // kinect returns a body, even when there is none

			if (_trackingState) { _trackingState = false; }
			return;
		}
		// body is tracked
		_trackingState = true;
	}




	private Vector3 GetVectorFromJoint(Joint joint)
	{

		return new Vector3(joint.Position.X, joint.Position.Y, joint.Position.Z);
	}

	/// <summary>
	/// Get positions of the joints as vector
	/// see the order @JointType
	/// </summary>
	/// <returns></returns>
	public Vector3[] GetJointsAsVector3()
	{
		if (_body == null) {
			return new Vector3[0];
		}


		Vector3[] joints = new Vector3[Body.JointCount];
		int index = 0;
		foreach (KeyValuePair<JointType, Joint> b in _body.Joints) {
			joints[index] = GetVectorFromJoint(b.Value);
			index++;
		}
		return joints;
	}
}
