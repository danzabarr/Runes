using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CircleTests : MonoBehaviour
{
	public int innerCount, p;
	public float innerRadius, outerRadius, u;
	[Range(0, 1)] public float zed;
	[Range(0, 1)] public float outerSpacing;
	public float minS;
	[Range(0, 1)] public float rippleAmount;
	[Range(0, 1)] public float rippleTarget;

	public static Vector3 positionAlongArc(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius, float t)
	{
		Vector3 rotation = Quaternion.AngleAxis(angle * t, normal) * from;
		return center + rotation.normalized * radius;
	}

	public static Vector3[] distributePoints(int n, Vector3 center, Vector3 normal, Vector3 from, float angle, float radius)
	{
		
		Vector3[] points = new Vector3[n];
		float angleStep = angle / (n);
		Quaternion rot = Quaternion.AngleAxis(angleStep, normal);
		Vector3 dir = from.normalized * radius;
		dir = Quaternion.AngleAxis(angleStep / 2, normal) * dir;
		for (int i = 0; i < n; i++)
		{
			points[i] = center + dir;
			dir = rot * dir;
		}
		return points;
	}

	public delegate float Remap(float t);

	public float Ripple(float t)
	{
		return RippleRemap.Evaluate(t, rippleTarget, rippleAmount, false);
	}

	public float RippleWrapped(float t)
	{
		return RippleRemap.Evaluate(t, rippleTarget, rippleAmount, true);
	}

	public static Vector2 GetPosition(int i, int n, float r, float minS)
	{
		float s = Mathf.Max(r / Mathf.Sin(Mathf.PI / n), minS);
		float angle = 2 * Mathf.PI * i / n;
		float x = Mathf.Cos(angle) * s;
		float y = Mathf.Sin(angle) * s;
		return new Vector2(x, y);
	}

	public static Vector2 GetOuterPosition(int i, int n, float r, float r2, float minS, float t, float outerSpacing)
	{
		Vector2 position = GetPosition(i, n, r, minS);
		float s = Mathf.Max(r / Mathf.Sin(Mathf.PI / n), minS);
		Vector2 P1 = new Vector2(Mathf.Cos(2 * Mathf.PI * i / n), Mathf.Sin(2 * Mathf.PI * i / n)) * s;
		Vector2 P2 = new Vector2(Mathf.Cos(2 * Mathf.PI * (i + 1) / n), Mathf.Sin(2 * Mathf.PI * (i + 1) / n)) * s;

		int intersection = CircleCircleIntersection(P1, r2, P2, r2, out Vector2 p1, out Vector2 p2);
		Vector2 v1 = p2 - position;
		Vector2 v2 = position * r2;
		float u = Vector2.Angle(v1, v2) * 2;
		return positionAlongArc(position.x0y(), Vector3.up, p2.x0y() - position.x0y(), u, r2 * outerSpacing, t).xz();
	}

	public void OnDrawGizmos()
	{
		float s = innerRadius / Mathf.Sin(Mathf.PI / innerCount);

		s = Mathf.Max(s, minS);

		DrawCircle(transform.position, s);

		for (int i = 0; i < innerCount; i++)
		{
			Vector2 position = transform.position.xz() + GetPosition(i, innerCount, innerRadius, minS);
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(position.x0y(), 0.5f);

			Vector3 outer = transform.position + GetOuterPosition(i, innerCount, innerRadius, outerRadius, minS, zed, outerSpacing).x0y();
			Gizmos.color = Color.red;
			//Gizmos.DrawSphere(outer, 0.2f);

			Vector2 P1 = new Vector2(Mathf.Cos(2 * Mathf.PI * i / innerCount), Mathf.Sin(2 * Mathf.PI * i / innerCount)) * s;
			Vector2 P2 = new Vector2(Mathf.Cos(2 * Mathf.PI * (i + 1) / innerCount), Mathf.Sin(2 * Mathf.PI * (i + 1) / innerCount)) * s;

			int intersection = CircleCircleIntersection(P1, outerRadius, P2, outerRadius, out Vector2 p1, out Vector2 p2);
			Vector2 v1 = p2 - position;
			Vector2 v2 = position * outerRadius;
			float u = Vector2.Angle(v1, v2) * 2;

			Handles.DrawWireArc(position.x0y(), Vector3.up, p2.x0y() - position.x0y(), u, outerRadius);

			for (int j = 0; j < p; j++)
			{
				float t = (j + 0.5f) / (float) p;
				if (intersection == 2)
					t = Ripple(t);
				else
					t = RippleWrapped(t);

				Vector3 outerPosition = positionAlongArc(position.x0y(), Vector3.up, p2.x0y() - position.x0y(), u, outerRadius * outerSpacing, t);
				Gizmos.color = Color.green;
				Gizmos.DrawSphere(outerPosition, 0.1f);
			}

		}


		if (true)
			return;

		DrawCircle(transform.position, innerRadius);

		for (int i = 0; i < innerCount; i++)
		{
			Handles.color = Color.white;
			var angle1 = 2 * Mathf.PI * i / innerCount;
			var x1 = Mathf.Cos(angle1) * innerRadius;
			var y1 = Mathf.Sin(angle1) * innerRadius;
			var position = transform.position + new Vector3(x1, 0, y1);
			Handles.Label(position, i.ToString());
			//DrawCircle(position, t);

			var angle2 = 2 * Mathf.PI * (i + 0.5f) / innerCount;
			var x2 = Mathf.Cos(angle2) * innerRadius;
			var y2 = Mathf.Sin(angle2) * innerRadius;
			var position2 = transform.position + new Vector3(x2, 0, y2);
			//Gizmos.DrawLine(transform.position, position2);

			float theta = Mathf.PI / innerCount;
			float d = innerRadius * (2 * theta);

			var angle3 = angle1 + theta;
			var x3 = x2 + Mathf.Cos(angle3) * outerRadius;
			var y3 = y2 + Mathf.Sin(angle3) * outerRadius;
			var position3 = transform.position + new Vector3(x3, 0, y3);
			//Gizmos.DrawLine(transform.position, position3);


			var P1 = transform.position.xz() + new Vector2(Mathf.Cos(2 * Mathf.PI * i / innerCount), Mathf.Sin(2 * Mathf.PI * i / innerCount)) * innerRadius;
			var P2 = transform.position.xz() + new Vector2(Mathf.Cos(2 * Mathf.PI * (i + 1) / innerCount), Mathf.Sin(2 * Mathf.PI * (i + 1) / innerCount)) * innerRadius;

			int intersection = CircleCircleIntersection(P1, outerRadius, P2, outerRadius, out Vector2 p1, out Vector2 p2);

			if (intersection == 1)
			{
				//Gizmos.DrawSphere(p1.x0y(), 0.1f);
			}
			else if (intersection == 2)
			{
				//Gizmos.DrawSphere(p2.x0y(), 0.1f);
			}

			Vector2 v1 = p2 - position.xz();
			Vector2 v2 = new Vector2(x1, y1) * outerRadius;

			u = Vector2.Angle(v1, v2) * 2;

			Handles.color = Color.red;
			Handles.DrawWireArc(position, Vector3.up, p2.x0y() - position, u, outerRadius);
			
			
		}
	}

	/// <summary>
	/// Calculates the points of intersection between two circles.
	/// </summary>
	/// <param name="c1"></param> The center of the first circle.
	/// <param name="r1"></param> The radius of the first circle.
	/// <param name="c2"></param> The center of the second circle.
	/// <param name="r2"></param> The radius of the second circle.
	/// <param name="p1"></param> The first point of intersection.
	/// <param name="p2"></param> The second point of intersection.
	/// <returns></returns> The number of points of intersection.
	public static int CircleCircleIntersection(Vector2 c1, float r1, Vector2 c2, float r2, out Vector2 p1, out Vector2 p2)
	{
		// Initialize the output variables
		p1 = new Vector2();
		p2 = new Vector2();

		// Compute the vector and distance between the circle centers
		Vector2 d = c2 - c1;
		float dist = d.magnitude;

		// If the distance is zero, the circles are coincident and have infinite intersections
		if (dist == 0)
		{
			return int.MaxValue;
		}

		// If the distance is greater than the sum of the radii, or less than the absolute difference
		// of the radii, the circles don't intersect
		if (dist > r1 + r2 || dist < Mathf.Abs(r1 - r2))
		{
			return 0;
		}

		// If the distance equals the sum or the absolute difference of the radii, the circles
		// intersect in a single point
		if (dist == r1 + r2 || dist == Mathf.Abs(r1 - r2))
		{
			p1 = c1 + d * (r1 / dist);
			return 1;
		}

		// In all other cases, the circles intersect in two points
		float a = (r1 * r1 - r2 * r2 + dist * dist) / (2 * dist);
		float h = Mathf.Sqrt(r1 * r1 - a * a);

		Vector2 x2 = c1 + d * (a / dist);

		float rx = -d.y * (h / dist);
		float ry = d.x * (h / dist);

		p1 = new Vector2(x2.x + rx, x2.y + ry);
		p2 = new Vector2(x2.x - rx, x2.y - ry);

		return 2;
	}


	public static void DrawCircle(Vector2 position, float radius)
	{
		Handles.DrawWireDisc(position.x0y(), Vector3.up, radius);
	}	

	public static void DrawCircle(Vector3 position, float radius)
	{
		Handles.DrawWireDisc(position, Vector3.up, radius);
	}
}
