using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Scriptable Objects/EnemyType")]
public class EnemyType : ScriptableObject
{
    public string _name ;
    public string main_description;
    public string[] actTextbuttons;
    public string[] actText;
    public string[] spareTextbuttons;
    public string[] spareText;
    public int level;
    public int exp;
    public int HP;
    public int fullHP;
    public int AT;
    public int DEF;
    public int exp_won;
}
