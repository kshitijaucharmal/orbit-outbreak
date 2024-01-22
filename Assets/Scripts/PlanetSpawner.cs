using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour {

    [SerializeField] private Vector2 bounds = Vector2.one * 200;
    [SerializeField] private int maxSize = 8;
    [SerializeField] private Vector2 n_Planets = new Vector2(50, 100);

    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private Sprite[] planetSprites;

    public Planet[] planets;
    // Start is called before the first frame update
    void Start() {
        SpawnPlanets();
    }

    void SpawnPlanets() {
        int n = Random.Range((int)n_Planets.x, (int)n_Planets.y);
        planets = new Planet[n];
        for(int i = 0; i < n; i++){
            var x = Random.Range(-bounds.x, bounds.x);
            var y = Random.Range(-bounds.y, bounds.y);
            var position = new Vector3(x, y, 0);
            var planet = Instantiate(planetPrefab, position, Quaternion.identity, transform).GetComponent<Planet>();
            var size = Random.Range(1, maxSize);
            planet.SetSize(size);
            var sr = planet.GetComponent<SpriteRenderer>();
            var img = planetSprites[Random.Range(0, planetSprites.Length)];
            sr.sprite = img;

            planets[i] = planet;
        }
    }
}
