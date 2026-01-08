using UnityEngine;

public class map1_to_2 : MonoBehaviour
{
    public bool playerIsClose;
    public map2_to_1 _map2_to_1;
    public GameObject NewCameLimit;
    public GameObject newAnchor;
    public CameraControl Camera;

    private void OnTriggerEnter2D()
    {
        playerIsClose = true;
        if(!_map2_to_1.playerIsClose){
            Camera.transitionToNewLimit = true;
            Camera.CurrentCamLimit = NewCameLimit;
            Camera.CurrentAnchor = newAnchor.transform.position;
            Camera.FTB_FFB_run_index = 0;
            }
    }

    private void OnTriggerExit2D()
    {
        playerIsClose = false;
    }
}
