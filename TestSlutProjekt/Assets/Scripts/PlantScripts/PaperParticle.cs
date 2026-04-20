using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaperParticle : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 heightInterval;
    [SerializeField] private Vector2 fallDistanceInterval;
    [SerializeField] private LayerMask paperParticleLayer;

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

    void SelectPaper()
    {
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // Convert mouse position
        mousePos3D.z = 0f;
        Vector2 mousePos2D = mousePos3D;
        Collider2D tileCol = Physics2D.OverlapPoint(mousePos2D, paperParticleLayer);// Checks if mouse is above particle
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
