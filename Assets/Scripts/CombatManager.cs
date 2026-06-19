using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public Map Map;
    public SpinAgent Player;
    public SpinAgent Enemy;

    private void Start()
    {
        Player.Setup(this);
        Enemy.Setup(this);
    }
}
