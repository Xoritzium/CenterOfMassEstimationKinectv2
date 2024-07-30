using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using Windows.Kinect;

public class JointAngleManager : MonoBehaviour
{

	[SerializeField] KinectSingleBodyManager sBodyManager;

	[SerializeField] int countedJoints;



	/// <summary>
	/// 0 - left shoulder
	///	1 - left ellbow
	///	2 - right shoulder
	/// 3 - right ellbow
	/// 4 - left hip
	/// 5 - left knee
	/// 6 - right hip
	/// 7 - right knee
	/// </summary>
	private AngledJoint[] _angledJoints { get; set; }

	public AngledJoint[] AngledJoints { get { return _angledJoints; } }


	// Start is called before the first frame update
	void Start()
	{
		_angledJoints = new AngledJoint[8];
		for (int i = 0; i < countedJoints; i++) {
			_angledJoints[i] = new AngledJoint(Vector3.zero, Vector3.zero, Vector3.zero, 0f);

		}
	}

	// Update is called once per frame
	void Update()
	{
		if (sBodyManager.TrackingState) {
			FillJointAngles(sBodyManager.GetJointsAsVector3());
		}

	}
	/// <summary>
	/// fill AngledJoint Array with its supposed values
	/// </summary>
	/// <param name="joints"></param>
	private void FillJointAngles(Vector3[] joints)
	{
		_angledJoints[0] = FillAngledJointData(joints[4], joints[5], joints[12]); //left shoulder
		_angledJoints[1] = FillAngledJointData(joints[5], joints[6], joints[4]); // left ellbow
		_angledJoints[2] = FillAngledJointData(joints[8], joints[9], joints[16]); // right shoulder
		_angledJoints[3] = FillAngledJointData(joints[9], joints[10], joints[8]); // right ellbow
		_angledJoints[4] = FillAngledJointData(joints[12], joints[13], joints[4]); //	left hip
		_angledJoints[5] = FillAngledJointData(joints[13], joints[14], joints[12]); // left knee
		_angledJoints[6] = FillAngledJointData(joints[16], joints[17], joints[8]); // right hip
		_angledJoints[7] = FillAngledJointData(joints[17], joints[18], joints[16]); // right knee
	}

	private AngledJoint FillAngledJointData(Vector3 pos, Vector3 a, Vector3 b)
	{

		Vector3 ab = a - pos;
		Vector3 ac = b - pos;
		float angle = Vector3.Angle(ab, ac);
		AngledJoint aj = new AngledJoint(pos, a, b, angle);
		return aj;
	}
}
