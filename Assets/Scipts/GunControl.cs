using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GunControl : MonoBehaviour
{

    public GameObject bulletPrefab;
    public ParticleSystem gunFx;
    public Transform barrelPos;
    public AudioClip gunSfx;
    
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void Shoot()
    {
        Instantiate(bulletPrefab, barrelPos.position, barrelPos.rotation);
        _audioSource.PlayOneShot(gunSfx, 1);
        gunFx.Play();
        print("Shoot!!");
    }
    
}
