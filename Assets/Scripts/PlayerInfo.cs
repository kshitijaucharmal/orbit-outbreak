using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {

    private Transform planetSpawner;

    [SerializeField] private Transform otherPlayer;
    [SerializeField] private Transform playerDirectionArrow;
    [SerializeField] private SpriteRenderer playerArrowSprite;
    [SerializeField] private Transform planetDirectionArrow;
    [SerializeField] private SpriteRenderer planetArrowSprite;


    private PlayerMovement pm;

    void Start(){
        planetSpawner = FindObjectOfType<PlanetSpawner>().transform;
        pm = GetComponent<PlayerMovement>();
    }

    public float Map(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    
    // Update is called once per frame
    void Update() {
        (var closestPlanet, var closestDist) = GetClosestPlanet();

        var planetAlpha = Map(closestDist, 0, 2000, 1, 0);
        var col = planetArrowSprite.color;
        col.a = planetAlpha;
        planetArrowSprite.color = col;

        var playerDistance = GetOtherPlayerDistance();

        var playerAlpha = Map(playerDistance, 0, 2000, 1, 0);
        col = playerArrowSprite.color;
        col.a = playerAlpha;
        playerArrowSprite.color = col;

        // Arrows
        // Draw Arrow Pointing to closest planet
        if (planetDirectionArrow != null){
            var lookDir = closestPlanet.position - transform.position;
            var angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            planetDirectionArrow.rotation = Quaternion.Lerp(planetDirectionArrow.rotation, Quaternion.Euler(0, 0, angle), 0.08f);
        }
        // Draw Arrow Pointing to other player
        if (playerDirectionArrow != null){
            var lookDir = otherPlayer.position - transform.position;
            var angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            playerDirectionArrow.rotation = Quaternion.Lerp(playerDirectionArrow.rotation, Quaternion.Euler(0, 0, angle), 0.08f);
        }
    }

    float GetOtherPlayerDistance(){
        return Vector2.Distance(transform.position, otherPlayer.position);
    }

    (Transform, float) GetClosestPlanet(){
        // Calculate Closest Distance
        var closestPlanet = planetSpawner; 
        float minSDist = Mathf.Infinity; 
        foreach(Transform child in planetSpawner){
            var dist = Mathf.Pow(transform.position.x - child.position.x, 2) + 
                       Mathf.Pow(transform.position.y - child.position.y, 2);
            
            if (dist < minSDist){
                minSDist = dist;
                closestPlanet = child;
            }
        }

        return (closestPlanet, minSDist);
    }
}
