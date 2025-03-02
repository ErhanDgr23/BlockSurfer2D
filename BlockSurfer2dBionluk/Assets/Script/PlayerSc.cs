using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayerSc : MonoBehaviour {

    [SerializeField] GameObject CubePre, Winpan, FailPan, Ground;
    [SerializeField] int CubeCounter;
    [SerializeField] TextMeshProUGUI TextCounter;
    [SerializeField] Transform CubeParent;
    [SerializeField] ObjectSpawner spawner;
    [SerializeField] Interstatialad Ads;
    [SerializeField] GameObject SoundObj;
    [SerializeField] AudioSource JumpSound;

    [SerializeField] Material[] CubeMats;
    [SerializeField] List<CubesSc> CubeList = new List<CubesSc>();

    Rigidbody2D rb;
    Vector3 target;
    bool GameStartB, move, pauseb, Clickable, DontDrop;
    int cubevalue;
    float speed = 2.5f;
    GameObject kamera;

    void Start()
    {
        if(PlayerPrefs.GetFloat("Sound") == 0f)
            SoundObj.gameObject.SetActive(true);
        else
            SoundObj.gameObject.SetActive(false);

        rb = GetComponent<Rigidbody2D>();
        target = transform.position;
        kamera = Camera.main.gameObject;
        TextCounter.text = CubeCounter.ToString();
    }

    private void FixedUpdate()
    {
        Ground.transform.position = new Vector2(transform.position.x, Ground.transform.position.y);
    }

    private void LateUpdate()
    {
        kamera.transform.position = Vector3.Lerp(kamera.transform.position, new Vector3(transform.position.x + 1.75f, 0.5f, -10f), 6f * Time.deltaTime);
    }

    public void clickpanbool(bool b)
    {
        Clickable = b;
    }

    public void LevelLoad(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void pauseF(bool b)
    {
        pauseb = b;
    }

    void Update()
    {
        if (GameStartB)
            if (!pauseb)
                transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (transform.position != target && move)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, target.y, transform.position.z), 5f * Time.deltaTime);
        else if(move)
            move = false;

        if (Input.GetButtonDown("Fire1") && Clickable && CubeCounter > 0f && GameStartB && cubevalue < 7f)
            CubeSpawn();
        else if (Input.GetButtonDown("Fire1") && Clickable)
            GameStartB = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Gain")
        {
            CubeCounter += int.Parse(collision.transform.name);
            Destroy(collision.gameObject);
            TextCounter.text = CubeCounter.ToString();
        }

        if (collision.gameObject.tag == "finito")
        {
            GameStartB = false;
            Winpan.gameObject.SetActive(true);
        }

        if (collision.gameObject.tag == "Block")
        {
            GameStartB = false;
            Ads.ShowInterstitialAd();
            FailPan.gameObject.SetActive(true);
        }

        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Grav")
        {
            GravityChanger(collision.gameObject);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Spawn")
        {
            spawner.TriggerSpawn();
        }
    }

    public void CloseOrOpenMusic()
    {
        if (SoundObj.gameObject.activeSelf)
        {
            SoundObj.SetActive(false);
            PlayerPrefs.SetFloat("Sound", 1f);
        }
        else
        {
            SoundObj.SetActive(true);
            PlayerPrefs.SetFloat("Sound", 0f);
        }
    }

    public void GravityChanger(GameObject collision)
    {
        int GravScale = int.Parse(collision.transform.GetChild(0).transform.name);
        transform.eulerAngles = new Vector3(0f, 0f, GravScale > 0 ? 0f : 180f);
        speed = GravScale;
        rb.gravityScale = GravScale;

        foreach (var item in CubeList)
        {
            //item.GetComponent<Rigidbody2D>().gravityScale = GravScale;

            if(item != null)
                item.BlockStop();
        }
    }

    public void CubeSpawn()
    {
        JumpSound.Play();
        transform.position += new Vector3(0f, 0.25f * speed, 0f);
        GameObject CloneCube = Instantiate(CubePre, CubeParent);
        CubesSc CloneCubeSc = CloneCube.GetComponent<CubesSc>();
        cubevalue++;
        CloneCube.transform.position = transform.position - new Vector3(0f, 0.25f * speed, 0f);
        CloneCube.GetComponent<Rigidbody2D>().gravityScale = speed;
        CloneCubeSc.id = cubevalue;
        CloneCubeSc.player = this;
        CloneCube.GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV();
        CubeList.Add(CloneCubeSc);
        CloneCube = null;
        CloneCubeSc = null;
        CubeCounter--;
        TextCounter.text = CubeCounter.ToString();
        //sirala();
    }

    public void sil(CubesSc target)
    {
        CubeList.Remove(target);
        cubevalue--;
        CancelInvoke("sirala");
        Invoke("sirala", 0.5f);
    }

    public void sirala()
    {
        //cubevalue = 0;
        //target = new Vector3(transform.position.x, -3.5f + (CubeList.Count * 0.5f), 1f);
        //move = true;

        //for (int i = 0; i < CubeList.Count; i++)
        //{
        //    CubesSc CubeScList = CubeList[i].GetComponent<CubesSc>();
        //    cubevalue++;
        //    CubeList[i].id = cubevalue;
        //    CubeScList.target = new Vector3(transform.position.x, -3.5f + (i * 0.5f), CubeList[i].transform.position.z);
        //    CubeScList.move = true;
        //}
    }
}
