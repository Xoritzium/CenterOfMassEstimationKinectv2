using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUILineRenderer : MonoBehaviour
{
	[SerializeField] UILineRenderer lineRenderer;
	[SerializeField] Vector2 a,b;

	// Start is called before the first frame update
	void Start()
	{
	

		Vector2[] next = new Vector2[2];
		next[0] = a;
		next[1] = b;
		lineRenderer = GetComponent<UILineRenderer>();
		lineRenderer.points = next;
	//	lineRenderer.SetAllDirty();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
