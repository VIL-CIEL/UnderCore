using UnityEngine;
using System;

public class CameraControl : MonoBehaviour
{
    // SUIVRE UN OBJET
    [Header("Objet à Suivre")]
    public GameObject player;

    [Header("Délai de temps pour rejoindre l'objet")]
    public float timeOffset;
    [Header("Decalage (z=-10 obligatoire)")]
    public Vector3 posOffset;
    private Vector3 velocity;

    void Update()
    {
        // LIMITATION DE LA CAMERA
        if(transitionToNewLimit)
        {   
            transition();
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + posOffset, ref velocity, timeOffset);
            transform.position = GetCamBounds(); 
        }
    }

    // LIMITATION DE LA CAMERA

    [Header("Limitation de la Camera")]
    public GameObject CurrentCamLimit;
    public Vector2 CurrentAnchor;
    private Camera mainCamera;
    public bool transitionToNewLimit;
    private Bounds camBounds;
    private void Awake() => mainCamera = Camera.main;

    private Single height;
    private Single width;
    public FadeControl fondu;
    public int FTB_FFB_run_index;
    private void Start()
    {
        height = mainCamera.orthographicSize;
        width = height * mainCamera.aspect;
        
        ChangeCamLimit();
        FTB_FFB_run_index = 0;
    }
    private void transition()
    {
        Vector3 targetPosition = new Vector3(CurrentAnchor.x, CurrentAnchor.y, -10);
        player.GetComponent<PlayerController>().MoveAction.Disable();

        if(!fondu.FTB_running & FTB_FFB_run_index == 0){
            fondu.FadeToBlack(1);
            player.transform.position = CurrentAnchor;
            FTB_FFB_run_index = 1;
            }

        else if(!fondu.FFB_running & !fondu.FTB_running & FTB_FFB_run_index == 1){
            transform.position = targetPosition;
            ChangeCamLimit();
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + posOffset, ref velocity, timeOffset);
            transform.position = GetCamBounds(); 
            fondu.FadeFromBlack(1);
            FTB_FFB_run_index = 2;
            }

        else if(!fondu.FFB_running & !fondu.FTB_running & FTB_FFB_run_index == 2){
            player.GetComponent<PlayerController>().MoveAction.Enable();
            transitionToNewLimit = false;
            }
        
    }

    private void ChangeCamLimit()
    {
        var cBounds = CurrentCamLimit.GetComponent<SpriteRenderer>().bounds;

        var minX = cBounds.min.x + width;
        var maxX = cBounds.max.x - width;
        var minY = cBounds.min.y + height;
        var maxY = cBounds.max.y - height;

        camBounds = new Bounds();
        camBounds.SetMinMax(new Vector3(minX, minY, 0), new Vector3(maxX, maxY, 0));
    }

    private Vector3 GetCamBounds()
    {
        return new Vector3(Mathf.Clamp(transform.position.x, camBounds.min.x, camBounds.max.x), Mathf.Clamp(transform.position.y, camBounds.min.y, camBounds.max.y), transform.position.z);
    }

}