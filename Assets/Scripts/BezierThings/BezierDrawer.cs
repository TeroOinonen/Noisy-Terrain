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


	private void OnDrawGizmos()
	{

		for (int i = 0; i < Beziers.Count; i++)
		{
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

			Handles.DrawBezier(Beziers[i].Anchor.position, target.Anchor.position, Beziers[i].Control1.position, target.Control0.position, Color.yellow, default, 5f);
		}




	}
}
