using UnityEngine;

public class TeamIdentifier : MonoBehaviour
{
    public SpriteRenderer diamondRenderer;

    public void SetTeam(bool team)
    {
        diamondRenderer.color = team ? Color.blue : Color.red;
    }
}