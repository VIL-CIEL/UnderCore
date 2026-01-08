using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();

    // STATISTIQUES
    public string p_name;
    public int level;
    public int exp;
    public int HP;
    public int AT;
    public int DEF;
    public int currentInvSlots;
    public int maxInvSlots;
    public Item equippedWeapon;
    public Item equippedArmor;
    public int gold;
    public int killCounter;
    public float clock;

    public string getName(){return p_name;}
    public int getLevel(){return level;}
    public int getExp(){return exp;}
    public int getHP(){return HP;}
    public int getAT(){return AT;}
    public int getDEF(){return DEF;}
    public int getCurrentInvSlots(){return currentInvSlots;}
    public int getInvSlots(){return maxInvSlots;}
    public Item getWeapon(){return equippedWeapon;}
    public Item getArmor(){return equippedArmor;}
    public int getGold(){return gold;}
    public int getKills(){return killCounter;}
    public float getClock(){return clock;}
    public List<Item> getInv(){return inventory;}

    public void useItem(Item used_item){

        if(used_item.hpGiven != 0)
            if(HP + used_item.hpGiven > Globals.conversionLeveltoHp[level-1])
                HP = Globals.conversionLeveltoHp[level-1];
            else
                HP += used_item.hpGiven;

        else if(used_item.atkBoost != 0){
            AT += used_item.atkBoost;
            equippedWeapon = used_item;
        }

        else if(used_item.armor_hp != 0){
            DEF += used_item.armor_hp;
            equippedArmor = used_item;
        }
        currentInvSlots -=1;
    }

    public void giveItem(Item new_item){
        if(currentInvSlots+1 <= maxInvSlots){
            inventory.Add(new_item);
            currentInvSlots +=1;
        }
    }

    public void removeInvSlot(int n){currentInvSlots -=n;}

    public void removeHP(int minusHp){HP -= minusHp;}
    
    public void addEXP(int plusXP){exp += plusXP;}

    [Serializable]
    public class Sauvegarde
    {
        public List<int> itemIds = new List<int>();
        public string p_name;
        public int level;
        public int exp;
        public int HP;
        public int AT;
        public int DEF;
        public int currentInvSlots;
        public int maxInvSlots;
        public Item equippedWeapon;
        public Item equippedArmor;
        public int gold;
        public int killCounter;
        public float clock;
    }

    public void save()
    {
        Sauvegarde _save = new Sauvegarde();

        _save.p_name = p_name;
        _save.level = level;
        _save.exp = exp;
        _save.HP = HP;
        _save.AT = AT;
        _save.DEF = DEF;
        _save.currentInvSlots = currentInvSlots;
        _save.maxInvSlots = maxInvSlots;
        _save.equippedWeapon = equippedWeapon;
        _save.equippedArmor = equippedArmor;
        _save.gold = gold;
        _save.killCounter = killCounter;
        _save.clock = clock;

        foreach (var item in inventory)
        {
            _save.itemIds.Add(item.id);
        }
        string inventoryData = JsonUtility.ToJson(_save);
        string filePath = Application.persistentDataPath + "/InventoryData.json";
        System.IO.File.WriteAllText(filePath, inventoryData);

        Debug.Log("Inventory saved to " + filePath);
    }

    public void load()
    {
        string filePath = Application.persistentDataPath + "/InventoryData.json";
        if (System.IO.File.Exists(filePath))
        {
            string inventoryData = System.IO.File.ReadAllText(filePath);
            Sauvegarde _save = JsonUtility.FromJson<Sauvegarde>(inventoryData);

            p_name = _save.p_name;
            level = _save.level;
            exp = _save.exp;
            HP = _save.HP;
            AT =  _save.AT;
            DEF = _save.DEF;
            currentInvSlots = _save.currentInvSlots;
            maxInvSlots = _save.maxInvSlots;
            equippedWeapon = _save.equippedWeapon;
            equippedArmor = _save.equippedArmor;
            gold = _save.gold;
            killCounter = _save.killCounter;
            clock = _save.clock;

            inventory.Clear();
            foreach (var index in _save.itemIds)
            {
                foreach (var item in Globals.AllItems)
                {
                    if (item.id == index)
                    {
                        inventory.Add(item);
                        break;
                    }
                }
            }

            Debug.Log("Inventory loaded from " + filePath);
        }
        else
        {
            Debug.LogWarning("No inventory file found at " + filePath);
        }
    }

    // PLAYER CONTROL
    [Header("Configuration des entrées de mouvement")]
    public InputAction MoveAction;
    [Header("Vitesse de déplacement")]
    public float speed;

    private Rigidbody2D rb2d;
    private Vector2 move;
    private Animator AnimationManager;

    void Start()
    {
        Globals.BuildItems();
        Globals.BuildEnemy();
        MoveAction.Enable();
        rb2d = GetComponent<Rigidbody2D>();
        AnimationManager = GetComponent<Animator>();
        equippedArmor = null;
        equippedWeapon = null;

        currentInvSlots = inventory.Count;

        p_name = "Frisk";
        level = 1;
        exp = 0;
        HP = Globals.conversionLeveltoHp[level-1];
        AT = Globals.conversionLeveltoAT[level-1];
        DEF = Globals.conversionLeveltoDF[level-1];
        currentInvSlots = 0;
        maxInvSlots = Globals.conversionLeveltoInvSlots[level-1];
        gold = 0;
        killCounter = 0;
        clock = 0;

    }

    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
        if (move != Vector2.zero)
        {
            AnimationManager.SetBool("isWalking", true);
            AnimationManager.SetFloat("InputX", move.x);
            AnimationManager.SetFloat("InputY", move.y);
            AnimationManager.SetFloat("LastInputX", move.x);
            AnimationManager.SetFloat("LastInputY", move.y);
        }
        else
            AnimationManager.SetBool("isWalking", false);
            
        move.Normalize();
        rb2d.linearVelocity = move * speed;
        clock += Time.deltaTime;

        if(exp >= Globals.EXPtoNextLevel[level-1] & level+1 <= 20){
            exp -= Globals.EXPtoNextLevel[level-1];
            level += 1;
            HP = Globals.conversionLeveltoHp[level-1];
            maxInvSlots = Globals.conversionLeveltoInvSlots[level-1];

            if(equippedWeapon != null)
                AT = equippedWeapon.atkBoost + Globals.conversionLeveltoAT[level-1];
            else
                AT = Globals.conversionLeveltoAT[level-1];
            
            if(equippedArmor != null)
                DEF = equippedArmor.armor_hp + Globals.conversionLeveltoDF[level-1];
            else
                DEF = Globals.conversionLeveltoDF[level-1];
        }

        if(Input.GetKeyDown(KeyCode.M)){
            exp +=  Globals.EXPtoNextLevel[level-1];
        }

    }
}