using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

    /// <summary>
    /// Anchor point, central point of the Bezier
    /// </summary>
public class BezierPoint : MonoBehaviour
{
    /// <summary>
    /// Starting Control point, defines starting direction for the Bezier
    /// </summary>
    public Transform Control0;

    /// <summary>
    /// Ending Control point, defines the end direction for the Bezier
    /// </summary>
	public Transform Control1;

    public Transform Anchor { get { return gameObject.transform; } }

    private void OnDrawGizmos()
    {
	
		Gizmos.color = Color.green;
		Gizmos.DrawLine(Anchor.position, Control0.transform.position);

		Gizmos.color = Color.red;
		Gizmos.DrawLine(Anchor.position, Control1.transform.position);
      
	}
}
