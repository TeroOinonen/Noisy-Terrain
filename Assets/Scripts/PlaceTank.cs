using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTank : MonoBehaviour
{
	public GameObject Tank;

    private void OnDrawGizmos()
    {
        Vector3 lookfrom = transform.position;
        Vector3 direction = transform.forward;

        Ray ray = new Ray(lookfrom, direction);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
			DrawVector(lookfrom, hit.point, Color.magenta);

			DrawVector(hit.point, hit.point + hit.normal, Color.green);

			Vector3 vRight = Vector3.Cross(hit.normal,direction);

			DrawVector(hit.point, hit.point + vRight, Color.red);

			Vector3 vForward = Vector3.Cross(vRight, hit.normal);

			DrawVector(hit.point, hit.point + vForward, Color.blue);

			Quaternion quat = Quaternion.LookRotation(vForward, hit.normal);

			Tank.transform.rotation = quat;
			Tank.transform.position = hit.point;
		}
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


	private void DrawVectorDir(Vector3 from, Vector3 dir, Color selectColor)
	{
		Vector3 to = from + dir;
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
