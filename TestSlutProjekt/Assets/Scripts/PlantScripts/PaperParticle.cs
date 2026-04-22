using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaperParticle : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 heightInterval;
    [SerializeField] private Vector2 widthInterval;

    [SerializeField] private float aliveTimer = 10f;

    [SerializeField] private int moneyGain;
    [SerializeField] private LayerMask paperParticleLayer;

    private float x;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        x = transform.localPosition.x;

        StartCoroutine(JumpAnim()); // Start jump animation

        StartCoroutine(DeathTimer()); // Sets death timer
    }

    IEnumerator DeathTimer() // Wait for set time and then destroy
    {
        yield return new WaitForSeconds(aliveTimer);

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update() // This not working correctly when instanced from the plant flower
    {
        SelectPaper();
    }

    void SelectPaper()
    {
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // Convert mouse position
        mousePos3D.z = 0f;
        Vector2 mousePos2D = mousePos3D;
        Collider2D tileCol = Physics2D.OverlapPoint(mousePos2D, paperParticleLayer);// Checks if mouse is above particle

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (tileCol != null)
            {
                // Gets this specific script
                PaperParticle paper = tileCol.GetComponent<PaperParticle>();

                if (paper != null)
                {
                    paper.CollectPaper(); // Calls this function specifically on this object
                }
            }
        }
    }

    void CollectPaper()
    {
        Debug.Log("Collected Paper");
        PlayerStats.Instance.ChangeMoney(moneyGain);

        GameObject.Destroy(gameObject);
    }

    IEnumerator JumpAnim() // Used math instead of rigidbody because it gives a bit more control
    {
        float duration = Random.Range(0.8f, 1.5f) / speed; // Higher speed lowers duration which later increases speed in the movement
        float time = 0f;

        Vector2 startPos = transform.localPosition; // Starting pos

        float distanceX = Random.Range(widthInterval.x, widthInterval.y); // Which direction on the x axis as well as how far

        // Make it so it can not be smaller than -0.5 to 0.5 on the x axis, so it always lands beside

        float height = Random.Range(heightInterval.x, heightInterval.y); // How high the particle will go

        while (time < duration)
        {
            time += Time.deltaTime; // Makes it possible to move across an equation
            float t = time / duration; // 0 - 1

            float x = Mathf.Lerp(0, distanceX, t); // Moves x on a linear path

            // Using Equation to calculate where it will be
            float y = 4 * height * t * (1 - t);

            transform.localPosition = startPos + new Vector2(x, y); // Sets new position based on the equation

            yield return null;
        }
    }
}
