using UnityEngine;

public class KinectJointFilter : MonoBehaviour
{
	[SerializeField] private float smoothing = 0.5f;
	[SerializeField] private float correction = 0.5f;
	[SerializeField] private float prediction = 0.5f;
	[SerializeField] private float jitterRadius = 0.05f;
	[SerializeField] private float maxDeviationRadius = 0.04f;

	private Vector3 prevFilteredPos = Vector3.zero;
	private Vector3 prevTrend = Vector3.zero;
	private Vector3 rawPosition = Vector3.zero;


	public Vector3 ApplyFilter(Vector3 rawPos)
	{
		Vector3 filteredPos = Vector3.zero;
		Vector3 trend = Vector3.zero;

		rawPosition = rawPos;

		if (prevFilteredPos == Vector3.zero) {
			filteredPos = rawPosition;
			trend = Vector3.zero;
		} else {
			Vector3 diff = rawPosition - prevFilteredPos;
			if (diff.magnitude <= jitterRadius) {
				filteredPos = rawPosition * (diff.magnitude / jitterRadius) + prevFilteredPos * (1.0f - diff.magnitude / jitterRadius);
			} else {
				filteredPos = rawPosition;
			}

			trend = (filteredPos - prevFilteredPos) * correction + prevTrend * (1.0f - correction);
		}

		Vector3 predictedPos = filteredPos + trend * prediction;

		Vector3 deviation = predictedPos - rawPosition;
		if (deviation.magnitude > maxDeviationRadius) {
			predictedPos = predictedPos * (maxDeviationRadius / deviation.magnitude) + rawPosition * (1.0f - maxDeviationRadius / deviation.magnitude);
		}

		prevFilteredPos = filteredPos;
		prevTrend = trend;

		return predictedPos;
	}
}

