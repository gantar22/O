using UnityEngine;
using System.Collections;

public class BezierCurve : MonoBehaviour {

	public static float[] bezier( float[] x, float[] y,float t)
    {
        if(t < 0 || t > 1)
        {
            return null;
        }
        float[] result = new float[2];

        result[0] = x[0] * Mathf.Pow((1 - t), 3) + x[1] * 3 * Mathf.Pow((1 - t), 2) * t + x[2] * 3 * Mathf.Pow((1 - t), 1) * Mathf.Pow(t, 2) + x[3] * Mathf.Pow(t, 3);
        result[1] = y[0] * Mathf.Pow((1 - t), 3) + y[1] * 3 * Mathf.Pow((1 - t), 2) * t + y[2] * 3 * Mathf.Pow((1 - t), 1) * Mathf.Pow(t, 2) + y[3] * Mathf.Pow(t, 3);
        return result;
    }
}
