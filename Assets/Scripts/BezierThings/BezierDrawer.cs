using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierDrawer : MonoBehaviour
{
	/// <summary>
	/// Starting Control point, defines starting direction for the Bezier
	/// </summary>
	[SerializeField]
	private List<BezierPoint> Beziers;

	[SerializeField]
	private bool IsLooping = false;

	[SerializeField]
	[Range(0f, 1f)]
	private float t = 0f; // Placement position on the bezier track

	[SerializeField]
	private Mesh2D road2D;

	private void OnDrawGizmos()
	{

		for (int i = 0; i < Beziers.Count; i++)
		{
			BezierPoint source = Beziers[i];
			BezierPoint target;
			if (Beziers.Count > i + 1)
			{
				target = Beziers[i + 1];
			}
			else
			{
				if (IsLooping)
				{
					target = Beziers[0];
				}
				else
				{
					break;
				}

			}

			Handles.DrawBezier(source.Anchor.position, target.Anchor.position, source.Control1.position, target.Control0.position, Color.yellow, default, 5f);

			Vector3 tPos = GetBezierPositionWhenT(t, source, target); // Calculate position when t is x
			Vector3 tDir = GetBezierDirectionWhenT(t, source, target);

			Gizmos.color = Color.red;
			Gizmos.DrawSphere(tPos, 0.2f);
			
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(tPos, tPos + tDir * 3);

			Quaternion rot = Quaternion.LookRotation(tDir);

			Handles.PositionHandle(tPos, rot);

			for (int iz = 0; iz < 10; iz++)
			{
				float calcT = (float)iz * 0.1f;

				Vector3 tCalcPos = GetBezierPositionWhenT(calcT, source, target); // Calculate position when t is x
				Vector3 tCalcDir = GetBezierDirectionWhenT(calcT, source, target);
				Quaternion Calcrot = Quaternion.LookRotation(tCalcDir);

				//Gizmos.color = Color.yellow;
				//Gizmos.DrawSphere(tCalcPos + (Calcrot * Vector3.right * 0.3f), 0.1f);
				//Gizmos.DrawSphere(tCalcPos + (Calcrot * Vector3.left * 0.3f), 0.1f);
				//Gizmos.color = Color.green;
				//Gizmos.DrawSphere(tCalcPos + (Calcrot * Vector3.up * 0.3f), 0.1f);
				//Gizmos.DrawSphere(tCalcPos + (Calcrot * Vector3.down * 0.3f), 0.1f);

				for (int ix = 0; ix < road2D.vertices.Length; ix++)
				{
					Vector3 roadPoint = road2D.vertices[ix].point;
					Gizmos.DrawSphere(tCalcPos + (Calcrot * roadPoint), 0.1f);
				}
			}
		}
	}

	private Vector3 GetBezierPositionWhenT(float tValue, BezierPoint source, BezierPoint target)
	{
		// First Lerp
		Vector3 PtX = (1 - tValue) * source.Anchor.position + tValue * source.Control1.position;
		Vector3 PtY = (1 - tValue) * source.Control1.position + tValue * target.Control0.position;
		Vector3 PtZ = (1 - tValue) * target.Control0.position + tValue * target.Anchor.position;

		// Second Lerp
		Vector3 PtR = (1 - tValue) * PtX + tValue * PtY;
		Vector3 PtS = (1 - tValue) * PtY + tValue * PtZ;

		// Third Lerp
		Vector3 PtO = (1 - tValue) * PtR + tValue * PtS;

		return PtO;
	}

	private Vector3 GetBezierDirectionWhenT(float tValue, BezierPoint source, BezierPoint target)
	{
		// First Lerp
		Vector3 PtX = (1 - tValue) * source.Anchor.position + tValue * source.Control1.position;
		Vector3 PtY = (1 - tValue) * source.Control1.position + tValue * target.Control0.position;
		Vector3 PtZ = (1 - tValue) * target.Control0.position + tValue * target.Anchor.position;

		// Second Lerp
		Vector3 PtR = (1 - tValue) * PtX + tValue * PtY;
		Vector3 PtS = (1 - tValue) * PtY + tValue * PtZ;

		// Direction forward is the vector between R and S points
		return (PtS - PtR).normalized;
	}

}
