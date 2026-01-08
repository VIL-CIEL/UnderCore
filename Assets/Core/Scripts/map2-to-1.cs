using UnityEngine;

public class map2_to_1 : MonoBehaviour
{
    public bool playerIsClose;
    public map1_to_2 _map1_to_2;
    public GameObject NewCameLimit;
    public GameObject newAnchor;
    public CameraControl Camera;

    private void OnTriggerEnter2D()
    {
        playerIsClose = true;
        if(!_map1_to_2.playerIsClose){
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
