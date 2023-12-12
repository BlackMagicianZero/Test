using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public GameObject past_save_point;
    public GameObject past_save_point1;
    public GameObject past_save_point2;
    public GameObject past_save_point3;
    public GameObject past_save_point4;
    public GameObject past_save_point5;
    public GameObject past_save_point6;
    public GameObject past_save_point7;
    public GameObject past_save_point8;
    public GameObject past_save_point9;
    public GameObject past_save_point10;
    public GameObject past_save_point11;
    public GameObject past_save_point12;
    public GameObject pre_save_point;
    void Start()
    {

    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            past_save_point.SetActive(false);
            past_save_point1.SetActive(false);
            past_save_point2.SetActive(false);
            past_save_point3.SetActive(false);
            past_save_point4.SetActive(false);
            past_save_point5.SetActive(false);
            past_save_point6.SetActive(false);
            past_save_point7.SetActive(false);
            past_save_point8.SetActive(false);
            past_save_point9.SetActive(false);
            past_save_point10.SetActive(false);
            past_save_point11.SetActive(false);
            past_save_point12.SetActive(false);
            pre_save_point.SetActive(true);
        }
    }
}
