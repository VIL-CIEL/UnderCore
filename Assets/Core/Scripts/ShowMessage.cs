using UnityEngine;
using TMPro;
using System.Collections;

public class ShowMessage : MonoBehaviour
{
    private TextMeshProUGUI Message;

    private void Start()
    {
        gameObject.SetActive(false);
        Message = GetComponent<TextMeshProUGUI>();
    }

    public void newMessage(string mess, float time){
        Message.text = mess;
        gameObject.SetActive(true);
        StartCoroutine(Show_mess(time));
    }

    private IEnumerator Show_mess(float time){
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

}
