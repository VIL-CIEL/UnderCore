using UnityEngine;
using System.Collections;
using TMPro;

public class DeathScreenManager : MonoBehaviour
{
    public GameObject Title;
    public GameObject descr;
    public GameObject fToQUit;
    public GameObject Soul;

    private bool isSoulBroken;
    private bool isTitlesShown;

    private int index;

    void Start()
    {
        index = 0;
        isSoulBroken = false;
        isTitlesShown = false;
        Soul.SetActive(false);
        fToQUit.SetActive(false);
        Title.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);
        descr.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);
    }

    void Update()
    {
        if(gameObject.activeInHierarchy){

            if(index == 0){
                StartCoroutine(SoulBreak());
            }
            if(index == 1 & isSoulBroken){
                StartCoroutine(ShowTitles());
            }

            if(Input.GetKeyDown(KeyCode.F) & isTitlesShown){

                isSoulBroken = false;
                isTitlesShown = false;
                Soul.SetActive(false);

                Title.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);
                descr.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);
                fToQUit.SetActive(false);


                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            }
        }
    }

    private IEnumerator ShowTitles(){
        isTitlesShown = false;
        index += 1;
        for (float i = 0; i <= 1; i += 0.01f)
        {
            Title.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, i);
            descr.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1);
        Title.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
        descr.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
        fToQUit.SetActive(true);
        isTitlesShown = true;
        
    }

    private IEnumerator SoulBreak(){
        index += 1;
        isSoulBroken = false;
        Soul.SetActive(true);
        yield return new WaitForSeconds(2.2f);
        Soul.SetActive(false);
        isSoulBroken = true;
    }
}
