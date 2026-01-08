using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public int id;
    public Sprite image;
    public string _name;
    public string description;
    public int hpGiven;
    public int atkBoost;
    public int armor_hp;
}
