using UnityEngine;

public class PlayerSoulControl : MonoBehaviour
{
    [Header("Objet à Suivre")]
    public GameObject player;

    [Header("Délai de temps pour rejoindre l'objet")]
    public float timeOffset;

    [Header("Decalage")]
    public Vector3 posOffset;
    private Vector3 velocity;

    private Animator _Animation;
    public int _Animation_spawn;
    public int _Animation_dispawn;
    public int _Animation_broke;
    public bool isSpawned;

    void Start()
    {
        gameObject.SetActive(false);
        _Animation = GetComponent<Animator>();
        _Animation_spawn = -1;
        _Animation_dispawn = -1;
        _Animation_broke = -1;
        isSpawned = false;
    }

    void Update(){

        // if == 1 : running; if == 0 : stop; if == -1 : not currently running

        if(_Animation_spawn == 1){
            _Animation.SetBool("spawn",_Animation_spawn == 1);
        }
        else if(_Animation_spawn == 0 &_Animation_dispawn == -1 & _Animation_broke == -1){
            isSpawned = true;
            _Animation_spawn = -1;
            _Animation.SetBool("spawn", false);
        }

        if(_Animation_dispawn == 1){
            _Animation.SetBool("dispawn",_Animation_dispawn == 1);
        }

        else if(_Animation_spawn == -1 &_Animation_dispawn == 0 & _Animation_broke == -1){
            isSpawned = false;
            _Animation_dispawn = -1;
            _Animation.SetBool("dispawn", false);
            gameObject.SetActive(false);
        }

        if(_Animation_broke == 1){
            _Animation.SetBool("broke",_Animation_broke == 1);
        }

        else if(_Animation_spawn == -1 &_Animation_dispawn == -1 & _Animation_broke == 0){
            isSpawned = false;
            _Animation_broke = -1;
            _Animation.SetBool("broke", false);
            gameObject.SetActive(false);
        }
        
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + posOffset, ref velocity, timeOffset);
        
    }

    public void spawn(){
        _Animation.SetBool("spawn",true);
    }

    public void dispawn(){
        _Animation.SetBool("dispawn",true);
    }

    public void broke(){
        _Animation.SetBool("broke",true);
    }
}