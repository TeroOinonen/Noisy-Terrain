using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPath : MonoBehaviour
{
	public GameObject A;
	public GameObject B;
	public GameObject C;
	public GameObject D;

	[Range(0f, 1f)]
	public float T;

	private void OnDrawGizmos()
	{
		Vector3 vA = A.transform.position;
		Vector3 vB = B.transform.position;
		Vector3 vC = C.transform.position;
		Vector3 vD = D.transform.position;

		Gizmos.color = Color.red;
		Gizmos.DrawLine(vA, vB);
		Gizmos.DrawLine(vB, vC);
		Gizmos.DrawLine(vC, vD);

		Vector3 PtX = (1 - T) * vA + T * vB;
		Vector3 PtY = (1 - T) * vB + T * vC;
		Vector3 PtZ = (1 - T) * vC + T * vD;

		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(PtX, 0.02f);
		Gizmos.DrawSphere(PtY, 0.02f);
		Gizmos.DrawSphere(PtZ, 0.02f);

		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(PtX, PtY);
		Gizmos.DrawLine(PtY, PtZ);

		Vector3 PtR = (1 - T) * PtX + T * PtY;
		Vector3 PtS = (1 - T) * PtY + T * PtZ;

		Gizmos.DrawLine(PtR, PtS);

		Gizmos.color = Color.green;

		Gizmos.DrawSphere(PtR, 0.02f);
		Gizmos.DrawSphere(PtS, 0.02f);

		Vector3 PtO = (1 - T) * PtR + T * PtS;

		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(PtO, 0.02f);

	}

	private void DrawVector(Vector3 from, Vector3 to, Color selectColor)
	{
		Color curr = Gizmos.color;
		Gizmos.color = selectColor;
		Gizmos.DrawLine(from, to);
		// Compute a location from "to towards from with 30degs"
		Vector3 loc = -(to - from);
		loc = Vector3.ClampMagnitude(loc, 0.1f);
		Quaternion rot30 = Quaternion.Euler(0, 0, 30);
		Vector3 loc1 = rot30 * loc;
		rot30 = Quaternion.Euler(0, 0, -30);
		Vector3 loc2 = rot30 * loc;
		Gizmos.DrawLine(to, to + loc1);
		Gizmos.DrawLine(to, to + loc2);
		Gizmos.color = curr;
	}
}
