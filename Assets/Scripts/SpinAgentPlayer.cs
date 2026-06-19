using UnityEngine;

public class SpinAgentPlayer : SpinAgent
{
    [Header("Player")]
    public float FuelMax;
    public float FuelRegenRate;
    [Space]
    public float DashSpeedMax;
    public float DashSpeedMin;

    [SerializeField]
    private float fuelCurrent;

    protected override void SpinnerUpdate()
    {
        base.SpinnerUpdate();

        if (fuelCurrent < FuelMax)
        {
            fuelCurrent += FuelRegenRate * Time.deltaTime;
            fuelCurrent = Mathf.Min(fuelCurrent, FuelMax);
        }
    }

    public override void Dash(Vector2 direction, float strength)
    {
        strength = Mathf.Min(strength, fuelCurrent / FuelMax);
        fuelCurrent -= strength * FuelMax;

        strength = Mathf.Clamp(strength * DashSpeedMax, DashSpeedMin, DashSpeedMax);

        base.Dash(direction, strength);
    }
}
