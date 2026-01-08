using System.Collections;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public Vector3 Movement;
    public float timeToDeath = 6;
    private Coroutine Dispawning;
    public int damage;

    void Start()
    {
        Dispawning = StartCoroutine(Despawn());
        Movement = new Vector3(Movement.x,Movement.y,0);
    }

    void Update()
    {
        transform.position += Movement;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("playerSoul"))
        {
            other.gameObject.GetComponent<PlayerSoulControl>().player.GetComponent<PlayerController>().removeHP(damage);
            StopCoroutine(Dispawning);
            Destroy(gameObject);
        }
    }

    private IEnumerator Despawn(){
        yield return new WaitForSeconds(timeToDeath);
        Destroy(gameObject);
    }
}
