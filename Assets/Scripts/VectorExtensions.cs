using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
	public static Vector2 xz(this Vector3 vector) => new Vector2(vector.x, vector.z);
	public static Vector2 xy(this Vector3 vector) => new Vector2(vector.x, vector.y);
	public static Vector3 x0y(this Vector2 vector) => new Vector3(vector.x, 0, vector.y);
	public static Vector3 x0y(this Vector3 vector) => new Vector3(vector.x, 0, vector.y);
	public static Vector3 xy0(this Vector3 vector) => new Vector3(vector.x, vector.y, 0);
	public static Vector3 xy0(this Vector2 vector) => new Vector3(vector.x, vector.y, 0);
    public static Vector3 x0z(this Vector3 vector) => new Vector3(vector.x, 0, vector.z);
}
