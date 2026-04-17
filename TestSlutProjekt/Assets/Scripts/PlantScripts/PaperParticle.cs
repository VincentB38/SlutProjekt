using System.Collections;
using UnityEngine;

public class PaperParticle : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 heightInterval;
    [SerializeField] private Vector2 fallDistanceInterval;

    bool animHasPlayed;

    private float x;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        x = transform.localPosition.x;
    }

    // Update is called once per frame
    void Update() // This not working correctly when instanced from the plant flower
    {
        if (!animHasPlayed) JumpAnim();
    }

    void JumpAnim() // Fix everything
    {
        // Calculate X
        x += speed * Time.deltaTime;

        // Calculate y by using randomized height and randomized x-axis
        float y = -Mathf.Pow(x, 2) + (Random.Range(heightInterval.x, heightInterval.y) * Random.Range(-1f, 1f)) * x;

        // Update position
        transform.localPosition = new Vector2(x, y);

        if (transform.localPosition.y > Random.Range(fallDistanceInterval.x, fallDistanceInterval.y))
        {
            animHasPlayed = true;
        }
    }
}
