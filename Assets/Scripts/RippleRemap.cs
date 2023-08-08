using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class RippleRemap : MonoBehaviour
{

    public Transform p1, p2;

    public int n;
	[Range(0, 1)] public float u;
	[Range(0, 1)] public float v;
	public float r;
	public static float shortestSignedDistanceBetweenCircularValues(float origin, float target)
	{
		if (target > origin)
		{
			float diff = target - origin;
			float diff2 = 1 - target + origin;

			if (diff < diff2)
			{
				return diff;
			}
			else
			{
				return -diff2;
			}
		}
		else
		{
			float diff = origin - target;
			float diff2 = 1 - origin + target;
			if (diff < diff2)
			{
				return -diff;
			}
			else
			{
				return diff2;
			}
		}
	}

	public static float Evaluate(float t, float target, float amount, bool wrap)
	{
		//\frac{1}{2}\left(\sin\left(2\pi x+t\right)+1\right)
		//return 0.5f * (Mathf.Sin(2 * Mathf.PI * t + u) + 1.0f);
		//return Mathf.Sin(t * u);

		//return t * t - t * t * 2 * u + 2 * t * u;


		//3(1-t)^{2}tP_{1}+3(1-t)t^{2}P_{2}+t^{3}P_{3}

		//return 3 * (1 - t) * (1 - t) * t * u + 3 * (1 - t) * t * t * (1 - u) + t * t * t;

		float difference = wrap ? shortestSignedDistanceBetweenCircularValues(t, target) : target - t;
		float absDifference = Mathf.Abs(difference);

		float result = t + amount * difference * (1 - absDifference) * (1 - absDifference) * (1 - absDifference);

		if (wrap)
		{
			result %= 1f;
			result += 1f;
			result %= 1f;
		}
		else
		{
			result = Mathf.Clamp01(result);
		}
		return result;
	}


	public void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawLine(p1.position, p2.position);
		for (int i = 0; i < n; i++)
		{
			float t = (float)i / (n - 1);
			Gizmos.DrawSphere(Vector3.LerpUnclamped(p1.position, p2.position, Evaluate(t, u, v, false)), 0.1f);
		}

		Gizmos.color = Color.red;
		Gizmos.DrawSphere(Vector3.LerpUnclamped(p1.position, p2.position, u), 0.2f);



		DrawCircle(transform.position, r);
		Gizmos.color = Color.white;
		for (int i = 0; i < n; i++)
		{
			float t = (float)i / (n - 1);
			DrawPointOnCircle(transform.position, Vector2.up, r, Evaluate(t, u, v, true));
		}

		Gizmos.color = Color.red;
		DrawPointOnCircle(transform.position, Vector2.up, r, u);
	}

	public static void DrawCircle(Vector2 center, float radius)
	{
		Handles.DrawWireDisc(center.x0y(), Vector3.up, radius);
	}

	public static void DrawPointOnCircle(Vector2 center, Vector2 from, float radius, float t)
	{
		Vector3 point = PointOnCircle(center.x0y(), Vector3.up, from.x0y(), radius, t);
		Gizmos.DrawSphere(point, 0.1f);
	}

	public static Vector3 PointOnCircle(Vector3 center, Vector3 normal, Vector3 forward, float radius, float t)
	{
		Vector3 point = center + Quaternion.AngleAxis(t * 360f, normal) * forward * radius;
		return point;
	}
}
