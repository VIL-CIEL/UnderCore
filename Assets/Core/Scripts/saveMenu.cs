using UnityEngine;
using TMPro;

public class saveMenu : MonoBehaviour
{
    public GameObject player;
    public GameObject showMess;
    public TextMeshProUGUI Name_lv;
    public TextMeshProUGUI name_land;
    public TextMeshProUGUI save;
    public TextMeshProUGUI _return;

    private bool isSave = true;

    private void Update()
    {
        if(gameObject.activeInHierarchy){
            int minute = (int)player.GetComponent<PlayerController>().getClock()/60;
            int heure = (int)player.GetComponent<PlayerController>().getClock()/3600;
            Name_lv.text = $"{player.GetComponent<PlayerController>().getName()}                                     LV{player.GetComponent<PlayerController>().getLevel()}     {heure}:{minute}";
            name_land.text = "Land Of Big Good";
            save.color = Color.white;
            _return.color = Color.white;
            Globals.InMenu = true;
        }

        player.GetComponent<PlayerController>().MoveAction.Disable();
        if(isSave){
            save.color = Color.yellow;
            _return.color = Color.white;
        }
        else{
            save.color = Color.white;
            _return.color = Color.yellow;
        }

        if(isSave & Input.GetKeyDown(KeyCode.RightArrow))
            isSave = false;

        else if(!isSave & Input.GetKeyDown(KeyCode.LeftArrow))
            isSave = true;

        else if(Input.GetKeyDown(KeyCode.F) & isSave){
            player.GetComponent<PlayerController>().save();
            showMess.GetComponent<ShowMessage>().newMessage("Game Saved !",2);
            player.GetComponent<PlayerController>().MoveAction.Enable();
            gameObject.SetActive(false);
            Globals.InMenu = false;
        }
        else if(Input.GetKeyDown(KeyCode.F) & !isSave){
            player.GetComponent<PlayerController>().MoveAction.Enable();
            gameObject.SetActive(false);
            Globals.InMenu = false;
        }
        else if(Input.GetKeyDown(KeyCode.L)){
            player.GetComponent<PlayerController>().load();
            showMess.GetComponent<ShowMessage>().newMessage("Game Loaded !",2);
            player.GetComponent<PlayerController>().MoveAction.Enable();
            gameObject.SetActive(false);
            Globals.InMenu = false;
        }
    }

}
