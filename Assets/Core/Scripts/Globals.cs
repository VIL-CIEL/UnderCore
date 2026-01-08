using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public static bool InCombat = false;
    public static bool InMenu = false;

    public static Bounds WorldLimit;
    public static List<Item> AllItems = new List<Item>();
    public static List<EnemyType> AllEnemy = new List<EnemyType>();

    public static void BuildItems(){
        AllItems.Clear();
        Item[] items = Resources.LoadAll<Item>("items");
        foreach (var item in items){
            AllItems.Add(item);
        }
        if(AllItems.Count != 0)
            Debug.Log("Items Loaded");
        else
            Debug.LogWarning("! No Items Loaded !");
    }

    public static void BuildEnemy(){
        AllEnemy.Clear();
        EnemyType[] enemies = Resources.LoadAll<EnemyType>("enemy");
        foreach (var onePerOne in enemies){
            AllEnemy.Add(onePerOne);
        }
        if(AllEnemy.Count != 0)
            Debug.Log("Enemy Loaded");
        else
            Debug.LogWarning("! No Enemy Loaded !");
    }
    
    public static List<int> conversionLeveltoHp = new List<int>{20,24,28,32,36,40,44,48,52,56,60,64,58,72,76,80,84,88,92,99};
    public static List<int> conversionLeveltoAT = new List<int>{2,2,4,6,8,10,12,14,16,18,20,22,24,26,28,30,32,34,36,38};
    public static List<int> conversionLeveltoDF = new List<int>{0,0,0,0,1,1,1,1,2,2,2,2,3,3,3,3,4,4,4,4};
    public static List<int> EXPtoNextLevel = new List<int>{10,20,40,50,80,100,200,300,400,500,800,1000,1500,2000,3000,5000,10000,25000,49999,0};
    public static List<int> conversionLeveltoInvSlots = new List<int>{4,4,6,6,8,8,10,10,12,12,14,14,16,16,18,18,18,18,18,18};
    public static List<int> TotalExpForEachLevel = new List<int>{0,10,30,70,120,200,300,500,800,1200,1700,2500,3500,5000,7000,10000,15000,25000,50000,99999};

    //LV   HP  AT  DF EXP/LV TOTAL
    // 1 	20 	0 	0 	10 	    0
    // 2 	24 	2 	0 	20 	    10
    // 3 	28 	4 	0 	40 	    30
    // 4 	32 	6 	0 	50 	    70
    // 5 	36 	8 	1 	80 	    120
    // 6 	40 	10 	1 	100 	200
    // 7 	44 	12 	1 	200 	300
    // 8 	48 	14 	1 	300 	500
    // 9 	52 	16 	2 	400 	800
    // 10 	56 	18 	2 	500 	1200
    // 11 	60 	20 	2 	800 	1700
    // 12 	64 	22 	2 	1000 	2500
    // 13 	68 	24 	3 	1500 	3500
    // 14 	72 	26 	3 	2000 	5000
    // 15 	76 	28 	3 	3000 	7000
    // 16 	80 	30 	3 	5000 	10000
    // 17 	84 	32 	4 	10000 	15000
    // 18 	88 	34 	4 	25000 	25000
    // 19 	92 	36 	4 	49999 	50000
    // 20 	99 	38 	4 	N/A 	99999 

    public class Enemy{

        public tuToriel tu_Toriel = new tuToriel();

        public class tuToriel{
                public static string name = "(Tu)Toriel";
                public static string main_description = "* Your first fight !\n* She is smiling at you.";
                public static string act_description = $"* {name} - ATK {AT} DEF {DEF}\n* She is smiling at you.";
                public static string act_flirt = "* She's looking embarassed.";
                public static string act_insult = "* She don't listen.";
                public static int level = 3;
                public static int exp = 0;
                public static int HP = 28;
                public static int AT = 4;
                public static int DEF = 0;
        }

    }
}   