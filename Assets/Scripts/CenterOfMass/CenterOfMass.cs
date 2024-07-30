using UnityEngine;

public class CenterOfMass
{
	/// <summary>
	/// Position of the mass´Center
	/// </summary>
	Vector3 center { get; set; }
	public Vector3 Center {
		get { return center; }
		set { center = value; }
	}
	MassIdentifier name { get; set; }

	public MassIdentifier Name {
		get { return name; }
		set { name = value; }
	}


	/// <summary>
	/// weight of the mass -> calculated out of percentage of whole body
	/// </summary>
	float weight { get; set; }
	public float Weight {
		get { return weight; }
		set { weight = value; }
	}

	public CenterOfMass(Vector3 position, MassIdentifier name, float weight)
	{
		center = position;
		this.name = name;
		this.weight = weight;
	}

	

}
