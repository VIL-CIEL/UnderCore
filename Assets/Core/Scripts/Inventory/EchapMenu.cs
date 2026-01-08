using UnityEngine;
using TMPro;

public class EchapMenu : MonoBehaviour
{
    public GameObject player;
    public GameObject EchapButton;
    public GameObject Side_Menu;
    public GameObject Item_Menu;
    public GameObject Stats_Menu;
    public GameObject Credits_Menu;
    public GameObject SideSelector;
    public GameObject ItemSelector;
    public GameObject MessageBox;

    public TextMeshProUGUI SideTopTextName;
    public TextMeshProUGUI SideTopTextStats;
    public TextMeshProUGUI Item;
    public TextMeshProUGUI Item2;
    public TextMeshProUGUI Stats1;
    public TextMeshProUGUI Stats2;

    public KeyCode MenuOpen_Close;

    private bool isMenuOpen;
    private PlayerController PlayerControl;

    public bool[] SideMenu_tab = {true,false,false};
    private int Side_Menu_index;
    private bool in_second_menu;
    public int Item_index;

    void Start()
    {
        PlayerControl = player.GetComponent<PlayerController>();
        EchapButton.SetActive(true);
        Side_Menu.SetActive(false);
        Item_Menu.SetActive(false);
        Stats_Menu.SetActive(false);
        Credits_Menu.SetActive(false);
        isMenuOpen = false;
        Side_Menu_index = 0;
        in_second_menu = false;
        Item_index = 0;
    }

    void Update()
    {
        if(!Globals.InCombat & !Globals.InMenu){
            if(Input.GetKeyDown(MenuOpen_Close)){

                if(isMenuOpen){
                    EchapButton.SetActive(true);
                    Side_Menu.SetActive(false);
                    Item_Menu.SetActive(false);
                    Stats_Menu.SetActive(false);
                    Credits_Menu.SetActive(false);
                    isMenuOpen = false;
                    in_second_menu = false;
                    player.GetComponent<PlayerController>().MoveAction.Enable();
                }

                else{
                    EchapButton.SetActive(false);
                    Side_Menu.SetActive(true);
                    Item_Menu.SetActive(false);
                    Stats_Menu.SetActive(false);
                    Credits_Menu.SetActive(false);
                    isMenuOpen = true;
                    player.GetComponent<PlayerController>().MoveAction.Disable();
                }
            }

            if(isMenuOpen){

                if(!in_second_menu){
                    SideSelector.GetComponent<Animator>().SetBool("isChoosing",true);

                    if(Input.GetKeyDown(KeyCode.RightArrow)){
                        in_second_menu = true;
                        switch(Side_Menu_index) 
                            {
                            case 0:
                                Item_Menu.SetActive(true);
                                Stats_Menu.SetActive(false);
                                Credits_Menu.SetActive(false);
                                break;
                            case 1:
                                Item_Menu.SetActive(false);
                                Stats_Menu.SetActive(true);
                                Credits_Menu.SetActive(false);
                                break;
                            case 2:
                                Item_Menu.SetActive(false);
                                Stats_Menu.SetActive(false);
                                Credits_Menu.SetActive(true);
                                break;
                            }
                    }
                    else if(Input.GetKeyDown(KeyCode.DownArrow)){
                        if(Side_Menu_index+1 < SideMenu_tab.Length){
                            Side_Menu_index +=1 ;
                            SideSelector.transform.position = new Vector3(SideSelector.transform.position.x,SideSelector.transform.position.y-77,SideSelector.transform.position.z);
                            SideMenu_tab[Side_Menu_index] = true;
                            SideMenu_tab[Side_Menu_index-1] = false;
                        }
                    }

                    else if(Input.GetKeyDown(KeyCode.UpArrow)){
                        if(Side_Menu_index-1 >= 0){
                            Side_Menu_index -= 1;
                            SideSelector.transform.position = new Vector3(SideSelector.transform.position.x,SideSelector.transform.position.y+77,SideSelector.transform.position.z);
                            SideMenu_tab[Side_Menu_index] = true;
                            SideMenu_tab[Side_Menu_index+1] = false;
                        }
                    }
                }
                else{
                    SideSelector.GetComponent<Animator>().SetBool("isChoosing",false);
                    if(Input.GetKeyDown(KeyCode.LeftArrow) & Item_index < 8){
                        Item_Menu.SetActive(false);
                        Stats_Menu.SetActive(false);
                        Credits_Menu.SetActive(false);
                        in_second_menu = false;
                    }
                }
            }
            
            if(Side_Menu.activeInHierarchy){
                SideTopTextName.text = $"{PlayerControl.getName()}";
                SideTopTextStats.text = $"LV {PlayerControl.getLevel()}\nHP {PlayerControl.getHP()}\nG {PlayerControl.getGold()}";
            }

            if(Item_Menu.activeInHierarchy){
                ItemSelector.GetComponent<Animator>().SetBool("isChoosing",true);
                var inv = PlayerControl.getInv();
                Item.text = "";
                Item2.text = "";
                if(inv.Count == 0){
                    Item.text = "No Items";
                    Item2.text = "";
                }
                else if(inv.Count <= 9 ){
                    foreach (var item in inv){
                        Item.text += $"{item.name}\n";
                    }
                }
                else{
                    for (int i = 0; i < inv.Count; i++)
                    {
                    if(i>17){
                            print($"ITEM OUT OF RANGE : item '{inv[i].name}' with inventory index {i}");
                    }
                    else if(i>8)
                            Item2.text += $"{inv[i].name}\n";
                        else
                            Item.text += $"{inv[i].name}\n";
                    
                    }
                }
                if(inv.Count != 0){

                    if(Input.GetKeyDown(KeyCode.DownArrow)){

                        if(Item_index+1 == 9 & inv.Count >= 9){
                            Item_index +=1 ;
                            ItemSelector.transform.position = new Vector3(ItemSelector.transform.position.x+630,ItemSelector.transform.position.y+8*59,ItemSelector.transform.position.z);
                        }
                        else if(Item_index+1 < 18 & inv.Count > Item_index+1){
                            Item_index +=1 ;
                            ItemSelector.transform.position = new Vector3(ItemSelector.transform.position.x,ItemSelector.transform.position.y-59,ItemSelector.transform.position.z);
                        }
                    }

                    else if(Input.GetKeyDown(KeyCode.UpArrow)){

                        if(Item_index-1 == 8){
                            Item_index -=1 ;
                            ItemSelector.transform.position = new Vector3(ItemSelector.transform.position.x-630,ItemSelector.transform.position.y-8*59,ItemSelector.transform.position.z);
                        }
                        else if(Item_index-1 > -1){
                            Item_index -=1 ;
                            ItemSelector.transform.position = new Vector3(ItemSelector.transform.position.x,ItemSelector.transform.position.y+59,ItemSelector.transform.position.z);
                        }
                    }

                    if(Input.GetKeyDown(KeyCode.F)){
                        if (Item_index != 0 & inv.Count == Item_index+1){
                            Item_index -=1 ;
                            ItemSelector.transform.position = new Vector3(ItemSelector.transform.position.x,ItemSelector.transform.position.y+59,ItemSelector.transform.position.z);
                        }
                        MessageBox.GetComponent<ShowMessage>().newMessage($"{inv[Item_index].name} used !",2);
                        PlayerControl.useItem(inv[Item_index]);
                        inv.RemoveAt(Item_index);
                    }
                    else if(Input.GetKeyDown(KeyCode.X)){
                        if (Item_index != 0 & inv.Count == Item_index+1){
                            Item_index -=1 ;
                            ItemSelector.transform.position = new Vector3(ItemSelector.transform.position.x,ItemSelector.transform.position.y+59,ItemSelector.transform.position.z);
                        }
                        MessageBox.GetComponent<ShowMessage>().newMessage($"{inv[Item_index].name} dropped",2);
                        inv.RemoveAt(Item_index);
                        PlayerControl.removeInvSlot(1);
                    }

                }
                else
                    ItemSelector.GetComponent<Animator>().SetBool("isChoosing",false);
            }

            if(Stats_Menu.activeInHierarchy){
                int playerLevel = PlayerControl.getLevel()-1;
                Item playerWeapon = PlayerControl.getWeapon();
                Item playerArmor = PlayerControl.getArmor();

                if(playerWeapon == null & playerArmor == null)
                    Stats1.text = $"-> '{PlayerControl.getName()}'\n\n○ Lv {playerLevel+1}        ○ AT {PlayerControl.getAT()} (+0)\n○ HP {PlayerControl.getHP()}/{Globals.conversionLeveltoHp[playerLevel]}   ○ DEF {PlayerControl.getDEF()} (+0)\n\n○ EXP: {PlayerControl.getExp()}\n";
                else if(playerWeapon == null)
                    Stats1.text = $"-> '{PlayerControl.getName()}'\n\n○ Lv {playerLevel+1}        ○ AT {PlayerControl.getAT()} (+0)\n○ HP {PlayerControl.getHP()}/{Globals.conversionLeveltoHp[playerLevel]}   ○ DEF {PlayerControl.getDEF()-playerArmor.armor_hp} (+{playerArmor.armor_hp})\n\n○ EXP: {PlayerControl.getExp()}\n";
                else if(playerArmor == null)
                    Stats1.text = $"-> '{PlayerControl.getName()}'\n\n○ Lv {playerLevel+1}        ○ AT {PlayerControl.getAT() - playerWeapon.atkBoost} (+{playerWeapon.atkBoost})\n○ HP {PlayerControl.getHP()}/{Globals.conversionLeveltoHp[playerLevel]}   ○ DEF {PlayerControl.getDEF()} (+0)\n\n○ EXP: {PlayerControl.getExp()}\n";
                else
                    Stats1.text = $"-> '{PlayerControl.getName()}'\n\n○ Lv {playerLevel+1}        ○ AT {PlayerControl.getAT() - playerWeapon.atkBoost} (+{playerWeapon.atkBoost})\n○ HP {PlayerControl.getHP()}/{Globals.conversionLeveltoHp[playerLevel]}   ○ DEF {PlayerControl.getDEF()-playerArmor.armor_hp} (+{playerArmor.armor_hp})\n\n○ EXP: {PlayerControl.getExp()}\n";
                
                if(Globals.EXPtoNextLevel[playerLevel] != 0)
                    Stats1.text += $"○ NEXT: {Globals.EXPtoNextLevel[playerLevel]}";
                else
                    Stats1.text += "○ NEXT: NONE";

                Stats2.text = $"○ INV. SLOTS: {PlayerControl.getCurrentInvSlots()}/{PlayerControl.getInvSlots()}\n\n";

                if(playerWeapon == null)
                    Stats2.text += "○ WEAPON: None\n";
                else
                    Stats2.text += $"○ WEAPON: {playerWeapon._name}\n";

                if(playerArmor == null)
                    Stats2.text += "○ ARMOR: None\n\n";
                else
                    Stats2.text += $"○ ARMOR: {playerArmor._name}\n\n";

                Stats2.text += $"○ MONEY: {PlayerControl.getGold()}\n○ KILLS: {PlayerControl.getKills()}";
            }

        }
        else
            EchapButton.SetActive(false);
    }
}
