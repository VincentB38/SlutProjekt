using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour // make it wavy if we got time 
{
    [System.Serializable]
    public class LevelNode
    {
        public Transform startPoint; // to drag in every level 
    }

    public List<LevelNode> levels = new List<LevelNode>();

    public LineRenderer lineRenderer;

    public int segments = 20; // amount of lines
    public float duration = 1.2f; // how long it should take
    public int maxLevelIndex = 0; // to show max

    private List<Vector3> points = new List<Vector3>(); // list of all the points

    private void Start()
    {
        Play(); // the initial start
    }

    public void Play()
    {
        GeneratePath(); // Generates the path 
        StartCoroutine(AnimateLine()); // make the lines visible
    }

    void GeneratePath()
    {
        points.Clear(); // clear the points

        for (int i = 0; i < Mathf.Min(maxLevelIndex, levels.Count - 1); i++)
        {
            Vector3 start = levels[i].startPoint.position; // check the start position
            Vector3 end = levels[i + 1].startPoint.position; // check the end position that the line will be drawn too

            Vector3 direction = (end - start).normalized; // get the direction of the line 
            float distance = Vector3.Distance(start, end); // get the distance so we know how long the line has to be
            Vector3 perpendicular = Vector3.Cross(direction, Vector3.up).normalized; // calculates a unit vector pointing sideways relative to the path

            for (int j = 0; j <= segments; j++) // loops depending on amount of segments
            {
                float t = j / (float)segments; // converts index to a 0-1 range to blend between start adn end

                Vector3 basePoint = start + direction * distance * t; // smoothly moves from the start pos to the end pos based of t

                points.Add(basePoint + perpendicular); // offset the point to the side
            }
        }
    }

    IEnumerator AnimateLine()
    {
        float time = 0f; // the amount of time

        int total = points.Count; // checks the total

        if (total == 0)
        {
            yield break; // if there is no totals then stop
        }

        while (time < duration) // loop for when the time is under the duration
        {
            time += Time.deltaTime; // increase time

            float progress = time / duration; // caclulate the progress 

            int visible = Mathf.Clamp(Mathf.FloorToInt(progress * total),0,total); // converts the progress which is 0-1 into number of points to show

            lineRenderer.positionCount = visible; // tells the line renderer how many points to show

            for (int i = 0; i < visible; i++)
            {
                if (i >= points.Count) break; // just mkaing sure it doesn't get an out of range error 

                lineRenderer.SetPosition(i, points[i]); // assign each point to the line renderer
            }

            yield return null;
        }

        // final snap
        lineRenderer.positionCount = total; // make sure it shows all

        for (int i = 0; i < total; i++)
            lineRenderer.SetPosition(i, points[i]); // double checking that they are all assigned
    }
}