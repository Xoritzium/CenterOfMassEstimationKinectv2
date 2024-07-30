using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateCenterOfMass : MonoBehaviour
{
	public GameObject visCenterOfMasses;
	private GameObject visualOfCenterOfMass;

	private Vector3 centerOfMass;

	List<CenterOfMass> individualMasses;


	[Header("Weight")]
	public float weight;
	/*	  head = 7%
		 *core = 73
		 *upperarm =3
		 *lowerarm =2
		 *hand=1
		 *upperleg =12
		 *lowerleg =5
		 *foot=2
		 */
	private float headWeight, coreWeight, upperArmWeigt,
		lowerArmWeight, handWeight, upperLegWeight, lowerLegWeight, footWeight;

	private void Start()
	{

		individualMasses = new List<CenterOfMass>();

		CalculateWeights();
		centerOfMass = new Vector3(0, 0, 0);


		visualOfCenterOfMass = Instantiate(visCenterOfMasses, centerOfMass, Quaternion.identity);

	}
	private void Update()
	{
		visualOfCenterOfMass.transform.position = centerOfMass;

	}


	/// <summary>
	/// Calucalte  weights of each body part, by apsolute, default values.
	/// </summary>
	private void CalculateWeights()
	{
		headWeight = (weight / 100) * 7;
		coreWeight = (weight / 100) * 43;
		upperArmWeigt = (weight / 100) * 3;
		lowerArmWeight = (weight / 100) * 2;
		handWeight = (weight / 100) * 1;
		upperLegWeight = (weight / 100) * 12;
		lowerLegWeight = (weight / 100) * 5;
		footWeight = (weight / 100) * 2;
	}

	/// <summary>
	/// Calculate a vector3 Position of the center of mass following
	/// COMx = (m*x1+m*x2....m*xn) / weight
	/// </summary>
	/// <returns></returns>
	private Vector3 CalculateApsoluteCenter()
	{

		float xValue = 0;
		float yValue = 0;
		float zValue = 0;
		float divisor = 0;
		for (int i = 0; i < individualMasses.Count; i++)
		{
			xValue += individualMasses[i].Center.x * individualMasses[i].Weight;
			yValue += individualMasses[i].Center.y * individualMasses[i].Weight;
			zValue += individualMasses[i].Center.z * individualMasses[i].Weight;
			divisor += individualMasses[i].Weight;
		}
		xValue /= divisor;
		yValue /= divisor;
		zValue /= divisor;

		return new(xValue, yValue, zValue); // return center of Mass
	}




	private float GetWeightOfMass(MassIdentifier mass)
	{
		switch (mass)
		{
			case MassIdentifier.Head: return headWeight;
			case MassIdentifier.Core: return coreWeight;
			case MassIdentifier.UpperArmLeft: case MassIdentifier.UpperArmRight: return upperArmWeigt;
			case MassIdentifier.ForArmRight: case MassIdentifier.ForArmLeft: return lowerArmWeight;
			case MassIdentifier.HandRight: case MassIdentifier.HandLeft: return handWeight;
			case MassIdentifier.UpperLegLeft: case MassIdentifier.UpperLegRight: return upperLegWeight;
			case MassIdentifier.LowerLegLeft: case MassIdentifier.LowerLegRight: return lowerLegWeight;
			case MassIdentifier.FootLeft: case MassIdentifier.FootRight: return footWeight;
			default: return 0;
		}
	}
	private int GetIndividualMassIndex(MassIdentifier mass)
	{
		for (int i = 0; i < individualMasses.Count; i++)
		{
			if (individualMasses[i].Name == mass)
			{
				return i;
			}
		}
		return -1;
	}




	#region public Methods
	public void RecalculateApsoluteCenter()
	{

		centerOfMass = CalculateApsoluteCenter();
	}

	public bool FillCenterOfMasses(List<CenterOfMass> fill)
	{
		if (fill != null)
		{
			individualMasses = fill;
			return true;
		}
		return false;
	}


	/// <summary>
	/// update single center of mass find by its name
	/// </summary>
	/// <param name="index"></param>
	/// <param name="newPosition"></param>
	public void UpdateSingleCenterOfMas(MassIdentifier index, Vector3 newPosition)
	{
		foreach (CenterOfMass c in individualMasses)
		{

			if (c.Name == index)
			{
				c.Center = newPosition;
			}
		}
		RecalculateApsoluteCenter();
	}
	/// <summary>
	/// update a single mass and returns its new position
	/// </summary>
	public Vector3 UpdateSingleMass(Vector3 a, Vector3 b, MassIdentifier identifier)
	{
		Vector3 mass = a + ((b - a) / 2);
		int index = GetIndividualMassIndex(identifier);
		if (index < 0) { throw new System.Exception("mass not found"); }
		individualMasses[index].Center = mass;
		return mass;
	}


	public void CalculateMassPositionAndAddToList(Vector3 a, Vector3 b, MassIdentifier identifier)
	{

		Vector3 mass = a + ((b - a) / 2);
		float weight = GetWeightOfMass(identifier);
		if (weight == 0) { throw new System.Exception("weight assignment gone wrong"); }
		individualMasses.Add(new CenterOfMass(mass, identifier, weight));
	}

	public Vector3 GetCenterOfMass()
	{
		return centerOfMass;
	}

	public void ApplyNewMasses(float weight)
	{
		this.weight = weight;
		CalculateWeights();
	}


	#endregion public Methods
}
