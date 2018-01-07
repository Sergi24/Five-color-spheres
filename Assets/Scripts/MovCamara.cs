using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.UpArrow)) && transform.position.y > limitBaix) transform.Translate(Vector3.down * Time.deltaTime * velocitatCamara);
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && transform.position.x > limitEsquerra) transform.Translate(Vector3.left * Time.deltaTime * velocitatCamara);
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && transform.position.x < limitDreta) transform.Translate(Vector3.right * Time.deltaTime * velocitatCamara);

        if (Input.GetAxis("Mouse ScrollWheel") > 0f && transform.position.z < limitEndavant) // forward
            transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + velocitatCamara * .08f);
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && transform.position.z > limitEndarrera) // backward
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - velocitatCamara * .08f);

        if (Input.GetKey(KeyCode.Q) && transform.position.z > limitEndarrera) transform.Translate(Vector3.back * Time.deltaTime * velocitatCamara);
        else if (Input.GetKey(KeyCode.E) && transform.position.z < limitEndavant) transform.Translate(Vector3.forward * Time.deltaTime * velocitatCamara);
    }
}
