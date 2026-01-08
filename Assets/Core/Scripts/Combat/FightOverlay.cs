using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class FightOverlay : MonoBehaviour
{
    public GameObject player;
    public GameObject infoPanel;
    public GameObject verticalButtons;
    public GameObject fightPanel;
    public GameObject actPanel;
    public GameObject itemPanel;
    public GameObject sparePanel;
    public GameObject leftArrow;
    public GameObject rightArrow;
    public GameObject DeathScreen;
    public TextMeshProUGUI[] fightText;
    public TextMeshProUGUI[] actText;
    public TextMeshProUGUI[] itemText;
    public TextMeshProUGUI[] spareText;
    public TextMeshProUGUI HPText;
    public GameObject HPBar;
    public TextMeshProUGUI LVText;
    public TextMeshProUGUI ActDialogue;
    public TextMeshProUGUI SpareDialogue;


    public float duration;

    private Vector3 verticalButtonsStartPos;
    private Vector3 verticalButtonsEndPos;
    private Vector3 infoPanelStartPos;
    private Vector3 infoPanelEndPos;

    private int index = 0;
    private int indexTab = 0;

    private bool inSecondPanel = false;

    public GameObject fightSelector;
    public GameObject actSelector;
    public GameObject itemSelector;
    public GameObject spareSelector;

    private Vector3 fightSelectorStartPos;
    private Vector3 actSelectorStartPos;
    private Vector3 itemSelectorStartPos;
    private Vector3 spareSelectorStartPos;

    public GameObject FightArea;
    private FightAreaManager fightAreaScript;
    private PlayerController playerScript;
    private bool CoroutineRunning;
    private int lastIndex = 0;
    private bool itemInSecondPage;
    private bool isTyping = false;
    
    public GameObject hitLine;
    public GameObject selectHitLine;
    public GameObject dmgIndicator;
    private bool isFighting;
    private bool fightClicked;
    public bool isDamaged;

    void Start()
    {
        infoPanel.SetActive(false);
        verticalButtons.SetActive(false);
        fightPanel.SetActive(false);
        actPanel.SetActive(false);
        itemPanel.SetActive(false);
        sparePanel.SetActive(false);
        rightArrow.SetActive(false);
        leftArrow.SetActive(false);
        hitLine.SetActive(false);
        selectHitLine.SetActive(false);
        dmgIndicator.SetActive(false);
        DeathScreen.SetActive(false);
        playerScript = player.GetComponent<PlayerController>();
        CoroutineRunning = false;
        itemInSecondPage = false;
        isFighting = false;
        fightClicked = false;
        isDamaged = false;

        fightSelectorStartPos = fightSelector.transform.position;
        actSelectorStartPos = actSelector.transform.position;
        itemSelectorStartPos = itemSelector.transform.position;
        spareSelectorStartPos = spareSelector.transform.position;

        fightAreaScript = FightArea.GetComponent<FightAreaManager>();

        for (int i = 0; i < fightText.Length; i++)
        {
            fightText[i].text = "";
        }

        for (int i = 0; i < actText.Length; i++)
        {
            actText[i].text = "";
        }

        for (int i = 0; i < itemText.Length; i++)
        {
            itemText[i].text = "";
        }

        reloadTextItem();

        for (int i = 0; i < spareText.Length; i++)
        {
            spareText[i].text = "";
        }

        // Store the end positions
        verticalButtonsEndPos = verticalButtons.transform.localPosition;
        infoPanelEndPos = infoPanel.transform.localPosition;

        // Set the start positions off-screen
        verticalButtonsStartPos = new Vector3(-Screen.width / 2 + 150, verticalButtonsEndPos.y, verticalButtonsEndPos.z);
        infoPanelStartPos = new Vector3(infoPanelEndPos.x, -Screen.height / 2 + 80, infoPanelEndPos.z);

        // Set the initial positions
        verticalButtons.transform.localPosition = verticalButtonsStartPos;
        infoPanel.transform.localPosition = infoPanelStartPos;
    }

    void Update()
    {
        float maxHP = Globals.conversionLeveltoHp[playerScript.getLevel()-1];
        float currentHP = playerScript.getHP();

        HPText.text = $"{currentHP} / {maxHP} HP";
        LVText.text = $"LV {playerScript.getLevel()}";

        float newWidth = currentHP / maxHP * 98;
        RectTransform hpBarRectTransform = HPBar.GetComponent<RectTransform>();
        hpBarRectTransform.sizeDelta = new Vector2(newWidth, hpBarRectTransform.sizeDelta.y);


        if(playerScript.getHP() <= 0){
            DeathScreen.SetActive(true);
        }

        if(verticalButtons.activeInHierarchy & playerScript.getHP() > 0){

            if(!inSecondPanel){
                if(Input.GetKeyDown(KeyCode.UpArrow) & index-1 >= 0){
                    index -= 1;
                    SetAnimatorBools(index);
                }
                if(Input.GetKeyDown(KeyCode.DownArrow) & index+1 <= 3){
                    index += 1;
                    SetAnimatorBools(index);
                }
                if(Input.GetKeyDown(KeyCode.F) | Input.GetKeyDown(KeyCode.RightArrow)){
                    inSecondPanel = true;
                    indexTab = 0;
                    switch (index)
                    {
                        case 0:
                            fightPanel.SetActive(true);
                            fightSelector.transform.position = fightSelectorStartPos;
                            fightText[0].text = fightAreaScript.typeOfEnemy._name;
                            fightText[1].text = "";
                            fightText[2].text = "";
                            isDamaged = false;
                            break;
                        
                        case 1:
                            ReplaceText(true);
                            actPanel.SetActive(true);
                            actSelector.transform.position = actSelectorStartPos;
                            break;
                        
                        case 2:
                            itemPanel.SetActive(true);
                            itemSelector.transform.position = itemSelectorStartPos;
                            break;
                        
                        case 3:
                            ReplaceText(false);
                            sparePanel.SetActive(true);
                            spareSelector.transform.position = spareSelectorStartPos;
                            break;
                    }
                }
            }
            else{
                if(Input.GetKeyDown(KeyCode.LeftArrow) & (indexTab == 0 | indexTab == 3 | indexTab == 6) & !isTyping){
                    inSecondPanel = false;

                    switch (index)
                    {
                        case 0:
                            fightPanel.SetActive(false);
                            break;
                        
                        case 1:
                            actPanel.SetActive(false);
                            break;
                        
                        case 2:
                            itemPanel.SetActive(false);
                            break;
                        
                        case 3:
                            sparePanel.SetActive(false);
                            break;
                    }
                }

                if(fightPanel.activeInHierarchy){

                    if(!isFighting){
                        if(Input.GetKeyDown(KeyCode.RightArrow) & indexTab+1 < 1){
                            indexTab += 1;
                            fightSelector.transform.position += new Vector3(390,0,0);
                        }

                        if(Input.GetKeyDown(KeyCode.LeftArrow) & indexTab-1 >= 0){
                            indexTab -= 1;
                            fightSelector.transform.position -= new Vector3(390,0,0);
                        }

                        if(Input.GetKeyDown(KeyCode.F)){ 
                            fightSelector.SetActive(false);
                            fightText[0].text = "";
                            fightPanel.GetComponent<Outline>().effectColor = new Color(0,0.63529411764f,0.90980392156f,1); // conversion du code hexa 00A2E8 avec A2 = 162 et E8 = 232 en rgba : r = 0/255 = 0, g = 162/255 = 0.63529411764, b = 232/255 = 0.90980392156, a = 255/255 = 1
                            hitLine.SetActive(true);
                            selectHitLine.SetActive(true);
                            hitLine.transform.localPosition = new Vector3(UnityEngine.Random.Range(-180,397.8f),0,0);
                            selectHitLine.transform.localPosition = new Vector3(-397.8f,0,0);
                            isFighting = true;
                        }
                    }
                    else{
                        
                        // On attaque
                        if(Input.GetKeyDown(KeyCode.F) & !fightClicked){
                            fightClicked = true;
                            dmgIndicator.GetComponent<TextMeshProUGUI>().text = "";
                            selectHitLine.GetComponent<Animator>().SetBool("isClick",true);
                            selectHitLine.GetComponent<HitLineManager>().isHitLineActive = true;
                            
                            dmgIndicator.SetActive(true);
                            dmgIndicator.GetComponent<DamageIndicManager>().isDamageActive = true;
                        }
                        if(fightClicked){
                            float distance = Mathf.Abs(hitLine.transform.localPosition.x - selectHitLine.transform.localPosition.x);
                            float damage = 1 - (distance / (2 * 397.8f));
                            if(damage > 0.99f){
                                damage = playerScript.AT - fightAreaScript.typeOfEnemy.DEF/2;
                                dmgIndicator.GetComponent<TextMeshProUGUI>().color = new Color(0.99607843137f ,0.99607843137f ,0 ,1);
                                dmgIndicator.GetComponent<TextMeshProUGUI>().text = $"-{(int)damage} HP [CRITICAL]";
                            }
                            else{
                                damage = playerScript.AT * damage - fightAreaScript.typeOfEnemy.DEF;
                                dmgIndicator.GetComponent<TextMeshProUGUI>().color = Color.red;
                                dmgIndicator.GetComponent<TextMeshProUGUI>().text = $"-{(int)damage} HP";
                            }
                            if(!isDamaged){
                                fightAreaScript.typeOfEnemy.HP -= (int)damage;
                                isDamaged = true;
                            }
                            
                            
                            selectHitLine.GetComponent<Animator>().SetBool("isClick", selectHitLine.GetComponent<HitLineManager>().isHitLineActive);

                            if(!dmgIndicator.GetComponent<DamageIndicManager>().isDamageActive)
                                dmgIndicator.SetActive(false);

                            if(!selectHitLine.GetComponent<HitLineManager>().isHitLineActive){
                                fightClicked = false;
                                isFighting = false;
                                hitLine.SetActive(false);
                                selectHitLine.SetActive(false);
                                inSecondPanel = false;
                                fightPanel.SetActive(false);
                                fightText[0].text = fightAreaScript.typeOfEnemy._name;
                                fightSelector.SetActive(true);
                                fightPanel.GetComponent<Outline>().effectColor = new Color(0.99607843137f ,0.99607843137f ,0 ,1); // FEFE00
                                StartCoroutine(waitforNsecondsFight_Items(0.5f));
                            }
                        }

                        // En dehors de la bordure
                        else if(selectHitLine.transform.localPosition.x >= 394 & selectHitLine.transform.localPosition.x <= 397 ){
                            dmgIndicator.GetComponent<TextMeshProUGUI>().text = "";
                            selectHitLine.GetComponent<Animator>().SetBool("isClick",true);
                            selectHitLine.GetComponent<HitLineManager>().isHitLineActive = true;
                            
                            dmgIndicator.SetActive(true);
                            dmgIndicator.GetComponent<DamageIndicManager>().isDamageActive = true;

                            selectHitLine.transform.localPosition += new Vector3(4,0,0);
                        }
                        else if(selectHitLine.transform.localPosition.x > 397.8f){
                            int damage = 1;
                            if(!isDamaged){
                                fightAreaScript.typeOfEnemy.HP -= damage;
                                isDamaged = true;
                            }

                            dmgIndicator.GetComponent<TextMeshProUGUI>().text = $"-{damage} HP";
                            selectHitLine.GetComponent<Animator>().SetBool("isClick", selectHitLine.GetComponent<HitLineManager>().isHitLineActive);

                            if(!dmgIndicator.GetComponent<DamageIndicManager>().isDamageActive)
                                dmgIndicator.SetActive(false);

                            if(!selectHitLine.GetComponent<HitLineManager>().isHitLineActive){
                                fightClicked = false;
                                isFighting = false;
                                hitLine.SetActive(false);
                                selectHitLine.SetActive(false);
                                inSecondPanel = false;
                                fightPanel.SetActive(false);
                                fightText[0].text = fightAreaScript.typeOfEnemy._name;
                                fightSelector.SetActive(true);
                                fightPanel.GetComponent<Outline>().effectColor = new Color(0.99607843137f ,0.99607843137f ,0 ,1); // FEFE00
                                StartCoroutine(waitforNsecondsFight_Items(0.5f));
                            }
                            
                        }

                        // Il avance normalement
                        else{
                            selectHitLine.transform.localPosition += new Vector3(4,0,0);
                        }
                    }
                }

                if(actPanel.activeInHierarchy & !isTyping){
                    
                    if(Input.GetKeyDown(KeyCode.UpArrow) & indexTab-3 >= 0){
                        indexTab -= 3;
                        actSelector.transform.position += new Vector3(0,120,0);
                    }

                    if(Input.GetKeyDown(KeyCode.DownArrow) & indexTab+3 <= fightAreaScript.typeOfEnemy.actText.Length-1){
                        indexTab += 3;
                        actSelector.transform.position -= new Vector3(0,120,0);
                    }
                    
                    if(Input.GetKeyDown(KeyCode.RightArrow) & indexTab+1 <= fightAreaScript.typeOfEnemy.actText.Length-1){
                        if(indexTab == 2){
                            indexTab += 1;
                            actSelector.transform.position -= new Vector3(0,120,0);
                            actSelector.transform.position -= new Vector3(390*2,0,0);
                        }
                        else{
                            indexTab += 1;
                            actSelector.transform.position += new Vector3(390,0,0);
                        }
                    }

                    if(Input.GetKeyDown(KeyCode.LeftArrow) & indexTab-1 >= 0){
                        indexTab -= 1;
                        actSelector.transform.position -= new Vector3(390,0,0);
                    }

                    if(Input.GetKeyDown(KeyCode.F)){
                        fightAreaScript.typeOfEnemy.actText[0] = $"* (Tu)Toriel - {fightAreaScript.typeOfEnemy.HP}/{fightAreaScript.typeOfEnemy.fullHP} HP, ATK {fightAreaScript.typeOfEnemy.AT}, DEF {fightAreaScript.typeOfEnemy.DEF}\n* Your first enemy !";
                        actSelector.SetActive(false);
                        ActDialogue.text = "";
                        isTyping = true;
                        HideText(actText);
                        StartCoroutine(Typing(ActDialogue,fightAreaScript.typeOfEnemy.actText[indexTab], true));
                    }

                }
                if(itemPanel.activeInHierarchy){

                    if(playerScript.inventory.Count != 0){
                        
                        if(playerScript.inventory[indexTab].hpGiven != 0)
                            HPText.text += "+ " + playerScript.inventory[indexTab].hpGiven + " HP";

                        if(playerScript.inventory.Count > 8 & !itemInSecondPage){
                            rightArrow.SetActive(true);
                            leftArrow.SetActive(false);
                        }
                        else
                            rightArrow.SetActive(false);
                        
                        if(itemInSecondPage){
                            leftArrow.SetActive(true);
                            rightArrow.SetActive(false);
                        }


                        if(!itemInSecondPage & playerScript.inventory[indexTab].name.Length > 7 & !CoroutineRunning){
                            StartCoroutine(ItemCoroutine(indexTab, itemText[indexTab], playerScript.inventory[indexTab].name, 0.3f));
                        }
                        else if(itemInSecondPage & playerScript.inventory[indexTab].name.Length > 7 & !CoroutineRunning){
                            StartCoroutine(ItemCoroutine(indexTab, itemText[indexTab-9], playerScript.inventory[indexTab].name, 0.3f));
                        }
                        if(indexTab != lastIndex & CoroutineRunning){
                            StopAllCoroutines();
                            CoroutineRunning = false;
                            reloadTextItem();
                        }
                        
                        if(Input.GetKeyDown(KeyCode.UpArrow)){
                            if(!itemInSecondPage & indexTab-3 >= 0){
                                indexTab -= 3;
                                itemSelector.transform.position += new Vector3(0,83,0);
                            }
                            else if(itemInSecondPage & indexTab-3 >= 9){
                                indexTab -= 3;
                                itemSelector.transform.position += new Vector3(0,83,0);
                            }
                        }

                        if(Input.GetKeyDown(KeyCode.DownArrow)){
                            if(!itemInSecondPage & indexTab+3 <= 8){
                                indexTab += 3;
                                itemSelector.transform.position -= new Vector3(0,83,0);
                            }
                            else if(itemInSecondPage & indexTab+3 <= playerScript.inventory.Count-1){
                                indexTab += 3;
                                itemSelector.transform.position -= new Vector3(0,83,0);
                            }
                        }
                        
                        if(Input.GetKeyDown(KeyCode.RightArrow) & indexTab+1 <= playerScript.inventory.Count-1){
                            if(itemInSecondPage){
                                // si dans la seconde page et est a droite de la deuxieme page, retour a la ligne
                                if(indexTab == 11 | indexTab == 14 | indexTab == 17){
                                    indexTab += 1;
                                    itemSelector.transform.position -= new Vector3(390*2,0,0);
                                    itemSelector.transform.position -= new Vector3(0,83,0);
                                }
                                else{
                                // va a droite normalement
                                indexTab += 1;
                                itemSelector.transform.position += new Vector3(390,0,0);
                                }
                            }
                            else if((indexTab == 2 | indexTab == 5 | indexTab == 8 ) & playerScript.inventory.Count > 9){
                                // si a droite de la premiere page, passe a la seconde page
                                indexTab += 7;
                                itemSelector.transform.position -= new Vector3(390*2,0,0);
                                itemInSecondPage = true;

                                reloadTextItem();
                            }
                            else if(playerScript.inventory.Count <= 9 & indexTab == 2 | indexTab == 5 | indexTab == 8){
                                    indexTab += 1;
                                    itemSelector.transform.position -= new Vector3(390*2,0,0);
                                    itemSelector.transform.position -= new Vector3(0,83,0);
                                }
                            else{
                                // va a droite normalement
                                indexTab += 1;
                                itemSelector.transform.position += new Vector3(390,0,0);
                            }
                        }

                        if(Input.GetKeyDown(KeyCode.LeftArrow) & indexTab-1 >= 0){

                            if(itemInSecondPage & (indexTab == 9 | indexTab == 12 | indexTab == 15)){
                                // si dans la seconde page et qu'il est a gauche, retour a la premiere page (element le plus a droite)
                                indexTab -= 7;
                                itemSelector.transform.position += new Vector3(390*2,0,0);
                                itemInSecondPage = false;

                                reloadTextItem();
                            }
                            else{
                                // gauche normal
                                indexTab -= 1;
                                itemSelector.transform.position -= new Vector3(390,0,0);
                            }
                        }

                        if(Input.GetKeyDown(KeyCode.F)){
                            playerScript.useItem(playerScript.inventory[indexTab]);
                            playerScript.inventory.RemoveAt(indexTab);
                            indexTab = 0;
                            itemInSecondPage = false;
                            reloadTextItem();
                            inSecondPanel = false;
                            itemPanel.SetActive(false);
                            StartCoroutine(waitforNsecondsFight_Items(0.5f));
                        }
                    }
                    else{
                        itemText[0].text = "Empty";
                    }

                }
                if(sparePanel.activeInHierarchy & !isTyping){
                    
                    if(Input.GetKeyDown(KeyCode.RightArrow) & indexTab+1 <= fightAreaScript.typeOfEnemy.spareText.Length-1){
                        indexTab += 1;
                        spareSelector.transform.position += new Vector3(390,0,0);
                    }

                    if(Input.GetKeyDown(KeyCode.LeftArrow) & indexTab-1 >= 0){
                        indexTab -= 1;
                        spareSelector.transform.position -= new Vector3(390,0,0);
                    }

                    if(Input.GetKeyDown(KeyCode.F)){
                        spareSelector.SetActive(false);
                        SpareDialogue.text = "";
                        isTyping = true;
                        HideText(spareText);
                        StartCoroutine(Typing(SpareDialogue,fightAreaScript.typeOfEnemy.spareText[indexTab], false));
                    }
                }
            }
            
        }
    }

    private void SetAnimatorBools(int index)
    {
        Animator[] animators = verticalButtons.GetComponentsInChildren<Animator>();
        animators[0].SetBool("Fight", index == 0);
        animators[1].SetBool("Act", index == 1);
        animators[2].SetBool("Item", index == 2);
        animators[3].SetBool("Spare", index == 3);
        
    }

    public void Show()
    {
        infoPanel.SetActive(true);
        verticalButtons.SetActive(true);
        StartCoroutine(AnimShow(duration));
    }

    public void Hide()
    {
        StartCoroutine(AnimHide(duration));
    }

    public void ShowVerticalBar()
    {
        verticalButtons.SetActive(true);
        StartCoroutine(AnimShowVerticalBar(duration));
    }

    public void HideVerticalBar()
    {
        StartCoroutine(AnimHideVerticalBar(duration));
    }

    private IEnumerator AnimShow(float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Interpolate positions
            verticalButtons.transform.localPosition = Vector3.Lerp(verticalButtonsStartPos, verticalButtonsEndPos, t);
            infoPanel.transform.localPosition = Vector3.Lerp(infoPanelStartPos, infoPanelEndPos, t);

            yield return null;
        }

        // Ensure the final positions are set
        verticalButtons.transform.localPosition = verticalButtonsEndPos;
        infoPanel.transform.localPosition = infoPanelEndPos;
    }

    private IEnumerator AnimHide(float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Interpolate positions
            verticalButtons.transform.localPosition = Vector3.Lerp(verticalButtonsEndPos, verticalButtonsStartPos, t);
            infoPanel.transform.localPosition = Vector3.Lerp(infoPanelEndPos, infoPanelStartPos, t);

            yield return null;
        }

        // Ensure the final positions are set
        verticalButtons.transform.localPosition = verticalButtonsStartPos;
        infoPanel.transform.localPosition = infoPanelStartPos;

        // Deactivate panels after hiding
        infoPanel.SetActive(false);
        verticalButtons.SetActive(false);
    }

    private IEnumerator AnimShowVerticalBar(float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Interpolate position
            verticalButtons.transform.localPosition = Vector3.Lerp(verticalButtonsStartPos, verticalButtonsEndPos, t);

            yield return null;
        }

        // Ensure the final position is set
        verticalButtons.transform.localPosition = verticalButtonsEndPos;
    }

    private IEnumerator AnimHideVerticalBar(float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Interpolate position
            verticalButtons.transform.localPosition = Vector3.Lerp(verticalButtonsEndPos, verticalButtonsStartPos, t);

            yield return null;
        }

        // Ensure the final position is set
        verticalButtons.transform.localPosition = verticalButtonsStartPos;

        // Deactivate vertical buttons after hiding
        verticalButtons.SetActive(false);
    }

    private IEnumerator ItemCoroutine(int index, TextMeshProUGUI selectedItem, string textToPlace, float speed)
    {
        CoroutineRunning = true;

        lastIndex = index;
        int maxVisibleCharacters = 7; // Nombre maximum de caractères visibles
        int textLength = textToPlace.Length;
        int startIndex = 0;

        while (CoroutineRunning)
        {
            // Calculer l'index de fin en fonction de l'index de début et du nombre maximum de caractères visibles
            int endIndex = startIndex + maxVisibleCharacters;

            // Si l'index de fin dépasse la longueur du texte, ajuster l'index de début pour boucler
            if (endIndex > textLength)
            {
                endIndex = maxVisibleCharacters;
                startIndex = 0;
                CoroutineRunning = false;
            }

            // Extraire la sous-chaîne du texte à afficher
            string visibleText = textToPlace.Substring(startIndex, endIndex - startIndex);

            // Ajouter des points de suspension si le texte est tronqué
            if (endIndex < textLength)
            {
                visibleText += "...";
            }

            // Mettre à jour le texte de selectedItem
            selectedItem.text = visibleText;

            // Attendre avant de passer à la prochaine itération
            yield return new WaitForSeconds(speed);

            // Incrémenter l'index de début pour faire défiler le texte
            startIndex++;
        }
        yield return new WaitForSeconds(speed+0.6f);
    }

    private void reloadTextItem(){
        int reloadindex = 0;

        if(itemInSecondPage){reloadindex = 9;}

        for (int i = reloadindex; i < reloadindex+9; i++){

            if(i < playerScript.inventory.Count){

                if(playerScript.inventory[i].name.Length > 7){
                    itemText[i-reloadindex].text = playerScript.inventory[i].name.Substring(0,7);
                    itemText[i-reloadindex].text += "...";
                }
                else{
                    itemText[i-reloadindex].text = playerScript.inventory[i].name;
                }
            }
            else{
                itemText[i-reloadindex].text = "";
            }
        }
    }

    private void ReplaceText(bool isAct)
    {
        if(isAct){

            for (int i = 0; i < fightAreaScript.typeOfEnemy.actText.Length; i++)
            {
                if(fightAreaScript.typeOfEnemy.actText[i] == null){
                    actText[i].text = "";
                }
                else{
                    actText[i].text = fightAreaScript.typeOfEnemy.actTextbuttons[i];
                }
            }
        }
        else{

            for (int i = 0; i < fightAreaScript.typeOfEnemy.spareText.Length; i++)
            {
                if(fightAreaScript.typeOfEnemy.spareText[i] == null){
                    spareText[i].text = "";
                }
                else{
                    spareText[i].text = fightAreaScript.typeOfEnemy.spareTextbuttons[i];
                }
            }
        }
    }

    private IEnumerator waitforNsecondsThenHide(float n, bool isAct){

        yield return new WaitForSeconds(n);
        if(isAct){
            inSecondPanel = false;
            actPanel.SetActive(false);
            ActDialogue.text = "";
            ReplaceText(isAct);
            actSelector.SetActive(true);
            isTyping = false;
            fightAreaScript.PlayerTurn = false;
        }
        else{
            inSecondPanel = false;
            sparePanel.SetActive(false);
            SpareDialogue.text = "";
            ReplaceText(isAct);
            spareSelector.SetActive(true);
            isTyping = false;
            fightAreaScript.PlayerTurn = false;
        }
    }
    private IEnumerator Typing(TextMeshProUGUI holder, string dialogue, bool isAct)
        {
            for (int i = 0; i < dialogue.Length; i++)
            {
                holder.text += dialogue[i];
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(waitforNsecondsThenHide(2, isAct));
        }

    private void HideText(TextMeshProUGUI[] holder)
    {
        for (int i = 0; i < holder.Length; i++)
        {
            holder[i].text = "";
        }
    }
    private IEnumerator waitforNsecondsFight_Items(float n){yield return new WaitForSeconds(n);fightAreaScript.PlayerTurn = false;}
}