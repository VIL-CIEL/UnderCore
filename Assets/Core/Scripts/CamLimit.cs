using UnityEngine;

public class CamLimit : MonoBehaviour
{
    private void Awake()
    {
        var bounds = GetComponent<SpriteRenderer>().bounds;
        Globals.WorldLimit = bounds;
    }
}
