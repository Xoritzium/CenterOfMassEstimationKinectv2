using UnityEngine;
using Windows.Kinect;
using Joint = Windows.Kinect.Joint;
public class AvatarController : MonoBehaviour
{
	public GameObject BodySourceManager;
	private BodySourceManager _BodyManager;
	private Body _Body;

	[SerializeField] KinectJointFilter jointFilter;

	[SerializeField] private float multiplier, yOffset, xOffset;
	[Header("Bones \n")]
	// Assign the corresponding bones from the rigged model
	public Transform SpineBase;
	public Transform SpineMid;
	public Transform Neck;
	public Transform Head;
	public Transform ShoulderLeft;
	public Transform ElbowLeft;
	public Transform WristLeft;
	public Transform HandLeft;
	public Transform ShoulderRight;
	public Transform ElbowRight;
	public Transform WristRight;
	public Transform HandRight;
	public Transform HipLeft;
	public Transform KneeLeft;
	public Transform AnkleLeft;
	public Transform FootLeft;
	public Transform HipRight;
	public Transform KneeRight;
	public Transform AnkleRight;
	public Transform FootRight;

	void Start()
	{
		if (BodySourceManager == null) {
			Debug.LogError("BodySourceManager not Assigned!");
			return;
		}
		if (jointFilter == null) {
			Debug.LogError("KinectJointFilter not Assigned!");
			return;
		}

		_BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
	}

	void Update()
	{
		if (_BodyManager == null) {
			return;
		}

		Body[] data = _BodyManager.GetData();
		if (data == null) {
			return;
		}

		foreach (Body body in data) {
			if (body.IsTracked) {
				_Body = body;
				break;
			}
		}

		if (_Body == null) {
			return;
		}

		UpdateAvatar(_Body);
	}

	private void UpdateAvatar(Body body)
	{

		
		// Update the avatar's bones based on the Kinect joints
		SpineMid.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.SpineMid]));
		Neck.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.Neck]));
		Head.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.Head]));
		ShoulderLeft.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.ShoulderLeft]));
		ElbowLeft.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.ElbowLeft]));
		WristLeft.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.WristLeft]));
		HandLeft.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.HandLeft]));
		ShoulderRight.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.ShoulderRight]));
		ElbowRight.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.ElbowRight]));
		WristRight.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.WristRight]));
		HandRight.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.HandRight]));
		HipLeft.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.HipLeft]));
		KneeLeft.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.KneeLeft]));
		AnkleLeft.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.AnkleLeft]));
		FootLeft.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.FootLeft]));
		HipRight.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.HipRight]));
		KneeRight.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.KneeRight]));
		AnkleRight.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.AnkleRight]));
		FootRight.position = jointFilter.ApplyFilter(GetVector3FromJoint(body.Joints[JointType.FootRight]));
		
		SpineBase.position = GetVector3FromJoint(body.Joints[JointType.SpineBase]);
		SpineMid.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.SpineMid]);
		Neck.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.Neck]);
		Head.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.Head]);
		ShoulderLeft.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.ShoulderLeft]);
		ElbowLeft.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.ElbowLeft]);
		WristLeft.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.WristLeft]);
		HandLeft.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.HandLeft]);
		ShoulderRight.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.ShoulderRight]);
		ElbowRight.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.ElbowRight]);
		WristRight.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.WristRight]);
		HandRight.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.HandRight]);
		HipLeft.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.HipLeft]);
		KneeLeft.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.KneeLeft]);
		AnkleLeft.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.AnkleLeft]);
		FootLeft.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.FootLeft]);
		HipRight.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.HipRight]);
		KneeRight.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.KneeRight]);
		AnkleRight.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.AnkleRight]);
		FootRight.rotation = GetQuaternionFromJoint(body.JointOrientations[JointType.FootRight]);
		




	}
	private Quaternion GetQuaternionFromJoint(JointOrientation joint)
	{
		return new Quaternion(
			joint.Orientation.X,
			joint.Orientation.Y,
			joint.Orientation.Z,
			joint.Orientation.W);
	}



	private Vector3 GetVector3FromJoint(Joint joint)
	{
		if (multiplier == 0) {
			throw new System.Exception("multiplier cant be 0");
		}
		return new Vector3(joint.Position.X * multiplier + xOffset, joint.Position.Y * multiplier + yOffset, joint.Position.Z * multiplier);
	}
}

