using System.Collections;
using UnityEngine;

public class FightAreaManager : MonoBehaviour
{
    public GameObject player;
    public GameObject FightOverlay;
    public GameObject EnemyNPC;
    public GameObject playerSoul;
    public GameObject exitBlocker;
    public EnemyType typeOfEnemy;

    public GameObject positionWhenPlayerTurn;

    private PlayerSoulControl soulScript;
    private FightOverlay overlayScript;
    private PlayerController playerScript;
    private SpriteRenderer spriteRenderer;

    public bool PlayerTurn;
    private bool isFightOverlayShown = false;
    private bool isVerticalBarShown = false;

    public GameObject TopDownBullet;
    public GameObject LeftRightBullet;
    public GameObject TopDownSpawn;
    public GameObject LeftRightSpawn;

    private GameObject[] tabTopDown;
    private GameObject[] tabLeftRight;

    private bool isPlayerDead;
    void Start()
    {
        playerScript = player.GetComponent<PlayerController>();
        soulScript = playerSoul.GetComponent<PlayerSoulControl>();
        overlayScript = FightOverlay.GetComponent<FightOverlay>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerTurn = true;
        isPlayerDead = false;

        tabTopDown = new GameObject[TopDownSpawn.transform.childCount];
        tabLeftRight = new GameObject[LeftRightSpawn.transform.childCount];

        for (int i = 0; i < TopDownSpawn.transform.childCount; i++)
        {
            tabTopDown[i] = TopDownSpawn.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < LeftRightSpawn.transform.childCount; i++)
        {
            tabLeftRight[i] = LeftRightSpawn.transform.GetChild(i).gameObject;
        }
    }

    void Update()
{
    if (Globals.InCombat &!isPlayerDead)
    {
        if(playerScript.getHP() <= 0){
            isPlayerDead = true;
            StopAllCoroutines();
        }
        
        if (!gameObject.activeInHierarchy || !exitBlocker.activeInHierarchy)
        {
            typeOfEnemy.HP = typeOfEnemy.fullHP;
            exitBlocker.SetActive(true);
            playerSoul.SetActive(true);
            soulScript.spawn();
            player.transform.position = positionWhenPlayerTurn.transform.position;
            playerScript.MoveAction.Disable();
            StartCoroutine(FadeCoroutine(0, 0.8f, 1));
        }

        if (soulScript.isSpawned)
        {
            if (!isFightOverlayShown && !isVerticalBarShown)
            {
                overlayScript.Show();
                isFightOverlayShown = true;
                isVerticalBarShown = true;
                player.GetComponent<SpriteRenderer>().sortingOrder = 3;
                EnemyNPC.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            else if (PlayerTurn)
            {
                if (!isVerticalBarShown)
                {
                    overlayScript.ShowVerticalBar();
                    isVerticalBarShown = true;
                    player.transform.position = positionWhenPlayerTurn.transform.position;
                    player.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    EnemyNPC.GetComponent<SpriteRenderer>().sortingOrder = 1;
                }
                playerScript.MoveAction.Disable();
            }
            else
            {
                if (isVerticalBarShown)
                {
                    overlayScript.HideVerticalBar();
                    isVerticalBarShown = false;
                    StartCoroutine(ShootBullets());
                    player.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    EnemyNPC.GetComponent<SpriteRenderer>().sortingOrder = 3;
                }
                playerScript.MoveAction.Enable();
            }
        }
    }
    else
    {
        if (gameObject.activeInHierarchy || exitBlocker.activeInHierarchy)
        {
            soulScript.dispawn();
            exitBlocker.SetActive(false);
        }

        if (!soulScript.isSpawned && isFightOverlayShown)
        {
            overlayScript.Hide();
            isFightOverlayShown = false;
        }
    }
}

    public IEnumerator FadeCoroutine(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0;
        Color color = spriteRenderer.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            spriteRenderer.color = color;
            yield return null;
        }
        color.a = endAlpha;
        spriteRenderer.color = color;
    }

    private IEnumerator ShootBullets(){

        for(int y = 0; y < 20; y++){
            for (int i = 0; i < tabTopDown.Length; i++)
            {
                Instantiate(TopDownBullet, tabTopDown[i].transform.position, Quaternion.Euler(0,0,-90));
                yield return new WaitForSeconds(0.08f);
            }
            for (int i = 0; i < tabLeftRight.Length; i++)
            {
                Instantiate(LeftRightBullet, tabLeftRight[i].transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.08f);
            }
        }
        yield return new WaitForSeconds(6);
        PlayerTurn = true;
    }
}