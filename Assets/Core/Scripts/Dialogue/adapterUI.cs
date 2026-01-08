using UnityEngine;

public class adapterUI : MonoBehaviour
{
    public bool use;
    [Header("Manage Dialogue Box")]
    public int paddingDialogueBox;
    [Header("Manage Skip Button")]
    public int paddingButtonX;
    public int paddingButtonY;

    void Update()
    {
        if (use){
            RectTransform rect = transform.GetComponent<RectTransform>();
            for (int i = 0; i < transform.childCount; i++)
                {
                    var _child = transform.GetChild(i);
                    if (_child.tag == "DialogueBox")
                    {
                        _child.GetComponent<RectTransform>().position = new Vector3(rect.position.x, rect.position.y + rect.sizeDelta.y/1.8f - rect.position.x + paddingDialogueBox, 0);
                    }
                    if (_child.tag == "skipB")
                    {
                        _child.GetComponent<RectTransform>().position = new Vector3(rect.sizeDelta.x - paddingButtonX, rect.sizeDelta.y - paddingButtonY, 0);
                    }
                }
            }
    }
}
