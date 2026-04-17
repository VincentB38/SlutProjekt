using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [System.Serializable]
    public class LevelNode
    {
        public Transform startPoint; // to drag in every level 
    }

    public List<LevelNode> levels = new List<LevelNode>();

    public LineRenderer lineRenderer;

    public int segments = 20;
    public float amplitude = 0.5f;
    public float duration = 1.2f; // how long it should take
    public int maxLevelIndex = 0; // to show max

    private List<Vector3> points = new List<Vector3>();

    private void Start()
    {
        Play();
    }
    public void Play(System.Action onComplete = null)
    {
        GeneratePath();
        StartCoroutine(AnimateLine(onComplete));
    }

    void GeneratePath()
    {
        points.Clear();

        for (int i = 0; i < Mathf.Min(maxLevelIndex, levels.Count - 1); i++)
        {
            Vector3 start = levels[i].startPoint.position;
            Vector3 end = levels[i + 1].startPoint.position;

            Vector3 direction = (end - start).normalized;
            float distance = Vector3.Distance(start, end);
            Vector3 perpendicular = Vector3.Cross(direction, Vector3.up).normalized;

            for (int j = 0; j <= segments; j++)
            {
                float t = j / (float)segments;

                Vector3 basePoint = start + direction * distance * t;

                float offset = Mathf.Sin(t * Mathf.PI * segments) * amplitude;
                offset += Random.Range(-0.1f, 0.1f);

                points.Add(basePoint + perpendicular * offset);
            }
        }
    }

    IEnumerator AnimateLine(System.Action onComplete)
    {
        float time = 0f;

        int total = points.Count;

        if (total == 0)
        {
            onComplete?.Invoke();
            yield break;
        }

        while (time < duration)
        {
            time += Time.deltaTime;

            float progress = time / duration;

            int visible = Mathf.Clamp(
                Mathf.FloorToInt(progress * total),
                0,
                total
            );

            lineRenderer.positionCount = visible;

            for (int i = 0; i < visible; i++)
            {
                // extra safety (prevents ANY race condition crash)
                if (i >= points.Count) break;

                lineRenderer.SetPosition(i, points[i]);
            }

            yield return null;
        }

        // final snap
        lineRenderer.positionCount = total;

        for (int i = 0; i < total; i++)
            lineRenderer.SetPosition(i, points[i]);

        onComplete?.Invoke();
    }
}