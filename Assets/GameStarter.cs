using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStarter : MonoBehaviour
{

    private float counter;
    [SerializeField] float timer;
    private bool counting = false;
    [SerializeField] Canvas canvas;

    [SerializeField] Image progressBar;

    [SerializeField] PlayerInput controls;

    private Rigidbody2D rb;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        progressBar.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            counter = 0;
            counting = true;
            Debug.Log("E");
            progressBar.transform.position = transform.position;
            Time.timeScale = 0;
            controls.DeactivateInput();
            rb.velocity = Vector3.zero;

        }
        if (counting)
        {
            counter += Time.unscaledDeltaTime;
            progressBar.fillAmount =counter / timer;
            if (counter > timer)
            {
                Debug.Log("Start");
                SceneManager.LoadScene("Level1");
                EndCount();
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            Debug.Log("Start canceled");
            EndCount();
        }


    }
    void EndCount()
    {
        rb.velocity = Vector3.zero;
        Time.timeScale = 1;
        counting = false;
        progressBar.fillAmount=0;
        controls.ActivateInput();
    }
}
