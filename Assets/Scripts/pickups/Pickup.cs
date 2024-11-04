using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
     protected LevelManager levelManager;
    [SerializeField] protected GameObject takingParticles;
    [SerializeField] protected  Sound[] _takeSound;

    protected abstract void GetBonus();
    protected virtual void PlaySound()
    {
        levelManager.soundManager.PlaySound(_takeSound);
    }

    public virtual void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    protected virtual void SpawnTakingParticles()
    {
        if (takingParticles != null)
        Instantiate(takingParticles, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlaySound();
            GetBonus();
            SpawnTakingParticles();
            Destroy(gameObject);
        }
    }
}
