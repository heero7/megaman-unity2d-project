using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XBuster : MonoBehaviour
{
    public List<PlayerProjectile> xBullets = new List<PlayerProjectile>();
    public List<PlayerProjectile> specialWeapons = new List<PlayerProjectile>();

    private PlayerProjectile currentWeapon;
    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = -1;
    }

    public void FireBuster(int weaponIndex)
    {
        Instantiate(xBullets[weaponIndex], transform.position, Quaternion.identity);
    }
}
