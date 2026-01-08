using UnityEngine;

public class GiveItem : MonoBehaviour
{
    public GameObject player;
    public GameObject MessageBox;
    public Item _item;

    private bool playerIsClose;

    private void Update()
    {
        if(playerIsClose & Input.GetKeyDown(KeyCode.F) & player.GetComponent<PlayerController>().getCurrentInvSlots()+1 <= player.GetComponent<PlayerController>().getInvSlots()){
            player.GetComponent<PlayerController>().giveItem(_item);
            gameObject.SetActive(false);
            MessageBox.GetComponent<ShowMessage>().newMessage($"{_item.name} added to the inventory",2);
        }
        else if(playerIsClose & Input.GetKeyDown(KeyCode.F))
            MessageBox.GetComponent<ShowMessage>().newMessage($"Can't add to inventory, it is full !",2);
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
