using UnityEngine;
public struct AngledJoint
{
	/// <summary>
	/// 0-position of joint angle
	/// 1-position first joint
	/// 2-positioon second joint
	/// </summary>
	public Vector3[] data;

	public float angle; 

	public AngledJoint(Vector3 pos, Vector3 frstPos, Vector3 secPos, float a)
	{
		data = new Vector3[3];
		data[0] = frstPos;
		data[1] = pos;
		data[2] = secPos;
		angle = a;
	}
}
