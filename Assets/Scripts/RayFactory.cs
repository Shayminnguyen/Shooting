using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayFactory : MonoBehaviour {
    public static RayFactory Instance { get; private set; }
    public GameObject rayPrefab;
    public int noToPreload = 3;
    public float rayLifetime = 0.45f;

    private List<GameObject> rays = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
        for(int i=0; i < noToPreload; i++)
        {
            GetNewRay().SetActive(false);
        }
    }

    public void NewRayFromTo(Vector2 from, Vector2 to)
    {
        GameObject ray = GetNewRay();
        ray.transform.position = Vector2.Lerp(from, to, 0.5f).WithZ(0);
        Vector2 direction = to - from;
        ray.transform.localScale = new Vector3(direction.magnitude * 0.01f, 0.07f, 1);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        ray.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        ray.SetActive(true);
        StartCoroutine(DespawnRay(ray));
    }

    private IEnumerator DespawnRay(GameObject ray)
    {
        yield return new WaitForSeconds(rayLifetime);
        ray.SetActive(false);
    }

    private GameObject GetNewRay()
    {
        foreach(GameObject ray in rays)
        {
            if (!ray.activeInHierarchy) return ray;
        }

        GameObject newRay = Instantiate(rayPrefab, transform);
        newRay.transform.localScale = Vector3.one;
        rays.Add(newRay);
        return newRay;
    }
}
