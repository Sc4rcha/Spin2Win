using UnityEngine;

public class SpinAgent : MonoBehaviour
{
    public bool ClockwiseSpin;

    private SpinStats stats;

    // movement variables
    private Vector2 direction;
    private Vector2 directionSmoothing;
    private const float directionDeceleration = 0.5f;

    // background direction
    private Vector2 backgroundRandom;
    private Vector2 backgroundCentre;

    // collision
    private Vector2 collisionAccumulated;

    private Vector2 velocity;
    private Vector2 velocityTarget;
    private Vector2 velocitySmoothing;
    private const float velocityDeceleration = 1;


    protected CombatManager manager;

    public void Setup(CombatManager manager) 
    {
        this.manager = manager;
        stats = GetComponent<SpinStats>();
    }

    private void FixedUpdate()
    {
        collisionAccumulated = Vector2.zero;

        SpinnerUpdate();
    }
    private void LateUpdate()
    {
        CollisionApply();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SpinAgent otherSpinner = collision.rigidbody.GetComponent<SpinAgent>();

        if (otherSpinner == null)
            return;

        // Send collisions
        otherSpinner.CollisionAdd(velocity, velocity.magnitude);
        CollisionAdd(-velocity, velocity.magnitude/2);
    }

    protected virtual void SpinnerUpdate() 
    {
        CheckCollisions();
        MoveUpdate();
    }

    private void MoveUpdate() 
    {
        direction = Vector2.SmoothDamp(direction, BackgroundDirection(), ref directionSmoothing, directionDeceleration);

        velocityTarget = direction.normalized * stats.MoveSpeedBase;
        velocity = Vector2.SmoothDamp(velocity, velocityTarget, ref velocitySmoothing, velocityDeceleration);
        velocity = Vector2.ClampMagnitude(velocity, stats.MoveSpeedMax);

        transform.position += (Vector3)velocity * Time.deltaTime;
    }
    private void CheckCollisions() 
    {
        // map
        if (manager.Map.CheckCollision (transform))
        {
            Vector2 spinnerToCentre = (manager.Map.transform.position - transform.position).normalized;
            Vector2 perpendicular = ClockwiseSpin ? Vector2.Perpendicular(spinnerToCentre) : -Vector2.Perpendicular(spinnerToCentre);
            Dash(Vector3.Slerp(spinnerToCentre, perpendicular, Random.value), manager.Map.StrengthCollision);
        }
    }

    public virtual void Dash(Vector2 direction, float strength) 
    {
        this.direction = direction.normalized * Mathf.Max(velocity.magnitude, strength);
        velocity = direction.normalized * Mathf.Max(velocity.magnitude, strength);
        velocitySmoothing = Vector2.zero;
    }
    public virtual void CollisionAdd(Vector2 direction, float strength) 
    {
        collisionAccumulated += direction.normalized * strength;
        velocitySmoothing = Vector2.zero;
    }

    private Vector2 BackgroundDirection() 
    {
        backgroundRandom = Random.insideUnitCircle.normalized;
        backgroundCentre = (manager.Map.Centre - (Vector2)transform.position).normalized * manager.Map.StrengthCentre;

        return (backgroundRandom + backgroundCentre) / 2;
    }
    private void CollisionApply()
    {
        this.direction += collisionAccumulated;
        velocity += collisionAccumulated;
    }

}