using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public GameObject player;
    public GameObject menuSauvegarde;
    private bool playerIsClose;

    private void Start()
    {
        menuSauvegarde.SetActive(false);
    }

    private void Update()
    {
        if(playerIsClose & Input.GetKeyDown(KeyCode.F)){
            menuSauvegarde.SetActive(true);
        }
    }

    private void OnTriggerEnter2D()
    {
        if (player.GetComponent<Collider2D>().CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D()
    {
        if (player.GetComponent<Collider2D>().CompareTag("Player"))
        {
            playerIsClose = false;
        }
    }
}
