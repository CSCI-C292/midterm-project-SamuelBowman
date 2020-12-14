using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    LevelManager levelManager;
    bool destroyed = false;

    void Start()
    {
        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
    }

    public IEnumerator DestroyObject()
    {
        if (!destroyed)
        {
            destroyed = true;
            GetComponent<AudioSource>().volume = levelManager.runtimeData.sfxVolume;
            GetComponent<AudioSource>().Play();
            if (GetComponent<ParticleSystem>() != null)
            {
                GetComponent<ParticleSystem>().Play();
            }
            yield return new WaitForSeconds(1);
            levelManager.DestroyObject();
            Destroy(gameObject);
        }
    }
}
