using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XBuster : MonoBehaviour
{
    public List<Projectile> xBullets = new List<Projectile>();
    public List<Projectile> specialWeapons = new List<Projectile>();

    private Projectile currentWeapon;
    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = -1;
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame)
    //         Fire(0);
    // }

    public void FireBuster(int weaponIndex)
    {
        Instantiate(xBullets[weaponIndex], transform.position, Quaternion.identity);
    }
}
