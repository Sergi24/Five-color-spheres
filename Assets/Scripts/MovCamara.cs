using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MovCamara : MonoBehaviour {

	public int velocitatCamara = 6;
    public int limitBaix = 12;
    public int limitDalt = 40;
    public int limitDreta = 70;
    public int limitEsquerra = 4;
    public int limitEndavant = -47;
    public int limitEndarrera = -100;

    // Update is called once per frame
    void Update () {

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && transform.position.y < limitDalt) transform.Translate(Vector3.up * Time.deltaTime * velocitatCamara);
        else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && transform.position.y > limitBaix) transform.Translate(Vector3.down * Time.deltaTime * velocitatCamara);
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && transform.position.x > limitEsquerra) transform.Translate(Vector3.left * Time.deltaTime * velocitatCamara);
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && transform.position.x < limitDreta) transform.Translate(Vector3.right * Time.deltaTime * velocitatCamara);

        if (Input.GetAxis("Mouse ScrollWheel") < 0f && gameObject.GetComponentInChildren<Camera>().orthographicSize < 14) // forward
            if (gameObject.GetComponentInChildren<Camera>() != null)  gameObject.GetComponentInChildren<Camera>().orthographicSize += 0.5f;
            else gameObject.GetComponent<Camera>().orthographicSize += 0.5f;
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f && gameObject.GetComponentInChildren<Camera>().orthographicSize > 4) // backward
            if (gameObject.GetComponentInChildren<Camera>() != null) gameObject.GetComponentInChildren<Camera>().orthographicSize -= 0.5f;
            else gameObject.GetComponentInChildren<Camera>().orthographicSize -= 0.5f;

        //    if (Input.GetAxis("Mouse ScrollWheel") > 0f && gameObject.GetComponentInChildren<Camera>().orthographicSize < 14) gameObject.GetComponentInChildren<Camera>().orthographicSize += 0.01f;
        //    else if (Input.GetAxis("Mouse ScrollWheel") < 0f && gameObject.GetComponentInChildren<Camera>().orthographicSize > 4) gameObject.GetComponentInChildren<Camera>().orthographicSize -= 0.01f;

        if (Input.GetKey(KeyCode.Q) && gameObject.GetComponentInChildren<Camera>().orthographicSize < 14)
            if (gameObject.GetComponentInChildren<Camera>()!=null) gameObject.GetComponentInChildren<Camera>().orthographicSize += 0.1f;
            else gameObject.GetComponent<Camera>().orthographicSize += 0.1f;
        else if (Input.GetKey(KeyCode.E) && gameObject.GetComponentInChildren<Camera>().orthographicSize > 4)
            if (gameObject.GetComponentInChildren<Camera>() != null) gameObject.GetComponentInChildren<Camera>().orthographicSize -= 0.1f;
            else gameObject.GetComponent<Camera>().orthographicSize -= 0.1f;
    }
}
