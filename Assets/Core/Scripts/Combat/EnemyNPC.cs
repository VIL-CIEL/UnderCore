using UnityEngine;

public class EnemyNPC : MonoBehaviour
{
    public GameObject fightArea;
    public bool playerIsClose;
    public GameObject MessageBox;
    private FightAreaManager AreaScript;
    private PlayerController PlayerScript;

    void Start()
    {
        GetComponent<Animator>().SetBool("isAnim",false);
        AreaScript = fightArea.GetComponent<FightAreaManager>();
        PlayerScript = AreaScript.player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if(Globals.InCombat){
            if(AreaScript.PlayerTurn)
            {
                GetComponent<Animator>().SetBool("isAnim",false);
            }
            else
            {
                GetComponent<Animator>().SetBool("isAnim",true);
            }

            if(AreaScript.typeOfEnemy.HP <= 0){
                Globals.InCombat = false;
                Color color = fightArea.GetComponent<SpriteRenderer>().color;
                color.a = 0;
                fightArea.GetComponent<SpriteRenderer>().color = color;
                PlayerScript.MoveAction.Enable();
                AreaScript.player.GetComponent<SpriteRenderer>().sortingOrder = 2;
                PlayerScript.addEXP(AreaScript.typeOfEnemy.exp_won);
                MessageBox.GetComponent<ShowMessage>().newMessage($"{AreaScript.typeOfEnemy._name} defeated ! You won {AreaScript.typeOfEnemy.exp_won} EXP",4);
                gameObject.SetActive(false);
            }
        }
        else if(playerIsClose & Input.GetKeyDown(KeyCode.F)){
            Globals.InCombat = true;
            foreach (EnemyType item in Globals.AllEnemy)
                if (item.name == "tuToriel")
                    AreaScript.typeOfEnemy = item;
        }

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
        }
    }
}
