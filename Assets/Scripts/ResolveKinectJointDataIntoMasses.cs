using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Windows.Kinect;
using Joint = Windows.Kinect.Joint;

public class ResolveKinectJointDataIntoMasses : MonoBehaviour
{
	[SerializeField] KinectSingleBodyManager kinectSingleBodyManager;
	[SerializeField] View rgbView;
	private CalculateCenterOfMass calculateManager;

	private bool startTracking = true;



	private Vector3[] _masses { get; set; }
	/// <summary>
	/// 3D Position of each center of mass
	/// </summary>
	public Vector3[] Masses
	{
		get { return _masses; }
	}


	// Start is called before the first frame update
	void Start()
	{

		calculateManager = GetComponent<CalculateCenterOfMass>();
		if (calculateManager == null)
		{
			throw new System.Exception("CalculateManager missing!");
		}


		_masses = new Vector3[14];
		for (int i = 0; i < _masses.Length; i++)
		{
			_masses[i] = Vector3.zero;
		}
	}

	// Update is called once per frame
	void Update()
	{

		if (!kinectSingleBodyManager.TrackingState) { return; } // break out if nothing tracked.

		//tracking 
		Vector3[] rawJointPositions = GetRawJointPositions(kinectSingleBodyManager.GetJointsAsVector3());
		//first iteration
		if (startTracking)
		{
			AddAllToMassCalculator(rawJointPositions);
			startTracking = false;
		}
		else
		{
			UpdateAllToMassCalculator(rawJointPositions);

			rgbView.UpdateCenterOfMassPosition(calculateManager.GetCenterOfMass());
		}

	}

	private Vector3[] GetRawJointPositions(Vector3[] rawJoints)
	{
		Vector3[] joints = new Vector3[15];
		joints[0] = rawJoints[3]; //head
		joints[1] = rawJoints[0]; //spine Base
		joints[14] = rawJoints[20]; //spine Shoulder

		joints[2] = rawJoints[4]; //shoulder left
		joints[3] = rawJoints[5]; //elbow left
		joints[4] = rawJoints[7]; //hand left

		joints[5] = rawJoints[8]; //shoulder right
		joints[6] = rawJoints[9]; //elbow right
		joints[7] = rawJoints[11]; //hand right

		joints[8] = rawJoints[12]; //hip left
		joints[9] = rawJoints[13]; //knee left
		joints[10] = rawJoints[15]; //foot left

		joints[11] = rawJoints[16]; //hip right
		joints[12] = rawJoints[17]; //knee right
		joints[13] = rawJoints[19]; //foot right
		return joints;

	}


	private void AddAllToMassCalculator(Vector3[] rawJoints)
	{
		calculateManager.CalculateMassPositionAndAddToList(rawJoints[0], rawJoints[14], MassIdentifier.Head);
		calculateManager.CalculateMassPositionAndAddToList(rawJoints[14], rawJoints[1], MassIdentifier.Core);

		calculateManager.CalculateMassPositionAndAddToList(rawJoints[2], rawJoints[3], MassIdentifier.UpperArmLeft);
		calculateManager.CalculateMassPositionAndAddToList(rawJoints[3], rawJoints[4], MassIdentifier.ForArmLeft);
		calculateManager.CalculateMassPositionAndAddToList(rawJoints[4], rawJoints[4], MassIdentifier.HandLeft);
		calculateManager.CalculateMassPositionAndAddToList(rawJoints[8], rawJoints[9], MassIdentifier.UpperLegLeft);
		calculateManager.CalculateMassPositionAndAddToList(rawJoints[9], rawJoints[10], MassIdentifier.LowerLegLeft);
		calculateManager.CalculateMassPositionAndAddToList(rawJoints[13], rawJoints[13], MassIdentifier.FootLeft);

		calculateManager.CalculateMassPositionAndAddToList(rawJoints[5], rawJoints[6], MassIdentifier.UpperArmRight);
		calculateManager.CalculateMassPositionAndAddToList(rawJoints[6], rawJoints[7], MassIdentifier.ForArmRight);
		calculateManager.CalculateMassPositionAndAddToList(rawJoints[7], rawJoints[7], MassIdentifier.HandRight);
		calculateManager.CalculateMassPositionAndAddToList(rawJoints[11], rawJoints[12], MassIdentifier.UpperLegRight);
		calculateManager.CalculateMassPositionAndAddToList(rawJoints[12], rawJoints[13], MassIdentifier.LowerLegRight);
		calculateManager.CalculateMassPositionAndAddToList(rawJoints[10], rawJoints[10], MassIdentifier.FootRight);
	}

	private void UpdateAllToMassCalculator(Vector3[] rawJoints)
	{
		//update each individual mass center´s position
		_masses[0] = calculateManager.UpdateSingleMass(rawJoints[0], rawJoints[14], MassIdentifier.Head);
		_masses[1] = calculateManager.UpdateSingleMass(rawJoints[14], rawJoints[1], MassIdentifier.Core);

		_masses[2] = calculateManager.UpdateSingleMass(rawJoints[2], rawJoints[3], MassIdentifier.UpperArmLeft);
		_masses[3] = calculateManager.UpdateSingleMass(rawJoints[3], rawJoints[4], MassIdentifier.ForArmLeft);
		_masses[4] = calculateManager.UpdateSingleMass(rawJoints[4], rawJoints[4], MassIdentifier.HandLeft);
		_masses[5] = calculateManager.UpdateSingleMass(rawJoints[8], rawJoints[9], MassIdentifier.UpperLegLeft);
		_masses[6] = calculateManager.UpdateSingleMass(rawJoints[9], rawJoints[10], MassIdentifier.LowerLegLeft);
		_masses[7] = calculateManager.UpdateSingleMass(rawJoints[13], rawJoints[13], MassIdentifier.FootLeft);

		_masses[8] = calculateManager.UpdateSingleMass(rawJoints[5], rawJoints[6], MassIdentifier.UpperArmRight);
		_masses[9] = calculateManager.UpdateSingleMass(rawJoints[6], rawJoints[7], MassIdentifier.ForArmRight);
		_masses[10] = calculateManager.UpdateSingleMass(rawJoints[7], rawJoints[7], MassIdentifier.HandRight);
		_masses[11] = calculateManager.UpdateSingleMass(rawJoints[11], rawJoints[12], MassIdentifier.UpperLegRight);
		_masses[12] = calculateManager.UpdateSingleMass(rawJoints[12], rawJoints[13], MassIdentifier.LowerLegRight);
		_masses[13] = calculateManager.UpdateSingleMass(rawJoints[10], rawJoints[10], MassIdentifier.FootRight);

		calculateManager.RecalculateApsoluteCenter(); // recalulate the actual Center of Mass
	}
}
