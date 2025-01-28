using UnityEngine;

public class ExplodingBarrel : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer; //Pour éviter d'allow un sheat 
    [SerializeField] private MeshCollider meshCollider; 
    [SerializeField] private AudioSource audioSource;

    public void HitByPlayer()
    {
        //ScoreManager.instance.AddScore(1);
        audioSource.Play();
        Destroy(gameObject, 1);
        meshCollider.enabled = false;
    }
}

