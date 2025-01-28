using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public GameObject pickupEffect;
    public GameObject ghostEffect;
    public float pickupEffectTime;
    public float ghostEffectTime;
    private bool canInstantiate = true;
    private bool isInvincible = false;
    public AudioClip hitSound;
    public AudioClip killSound;
    public AudioClip pageSound;
    public AudioClip bookSound;
    private AudioSource audioSource;

    public SkinnedMeshRenderer rend;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Page") //On a récupéré une page
        {
            audioSource.PlayOneShot(pageSound);
            GameObject go = Instantiate(pickupEffect, other.transform.position, Quaternion.identity);
            Destroy(go, pickupEffectTime);
            Destroy(other.gameObject);
            ScoreManager.instance.AddScorePages(1);
        }
        if (other.gameObject.tag == "Book") //On a récupéré un grimoire
        {
            audioSource.PlayOneShot(bookSound);
            GameObject go = Instantiate(pickupEffect, other.transform.position, Quaternion.identity);
            Destroy(go, pickupEffectTime);
            Destroy(other.gameObject);
            ScoreManager.instance.AddScoreBooks(1);
        }
        if (other.gameObject.tag == "Fall") //On tombe
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (collision.gameObject.tag == "Hurt" && !isInvincible) //Si le fantome me touche
        {
            audioSource.PlayOneShot(hitSound);
            //Je suis blessé
            isInvincible = true;
            ScoreManager.instance.SetHealth(-1);
            StartCoroutine("ResetInvincible");
        }

        if (collision.gameObject.tag == "Ghost" && canInstantiate) //Si je saute sur le fantome
        {
            audioSource.PlayOneShot(killSound);
            //collision.gameObject.transform.parent.gameObject.GetComponent<Collider>().enabled = false;
            canInstantiate = false;
            GameObject go = Instantiate(ghostEffect, collision.transform.position, Quaternion.identity);
            Destroy(go, ghostEffectTime);

            Destroy(collision.gameObject.transform.parent.gameObject, 0.5f);
            StartCoroutine("ResetInstantiate");
        }
    }

    #region Coroutine
    IEnumerator ResetInstantiate()
    {
        yield return new WaitForSeconds(0.8f);
        canInstantiate = true;
    }

    IEnumerator ResetInvincible()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.2f);
            rend.enabled = !rend.enabled;
        }
        yield return new WaitForSeconds(0.2f);
        rend.enabled = true;
        isInvincible = false;
    }
    #endregion
}
