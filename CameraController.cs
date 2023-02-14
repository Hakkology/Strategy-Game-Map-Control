using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform cameraTransform;
    public Transform followTransform;

    [SerializeField]
    float normalSpeed;

    [SerializeField]
    float fastSpeed;

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float moveTime;

    [SerializeField]
    float rotationAmount;

    [SerializeField]
    Vector3 zoomAmount;

    Vector3 newPosition;
    Quaternion newRotation;
    Vector3 newZoom;

    Vector3 dragStartPosition;
    Vector3 dragEndPosition;
    Vector3 rotateStartPosition;
    Vector3 rotateEndPosition;


    void Awake() => instance = this;

    // Start is called before the first frame update
    public void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    public void Update()
    {
        // If player wants to follow a specific object

        if (followTransform != null) {
            transform.position = followTransform.position;
        }
        else {
            HandleKeyboardInput();
            HandleMouseInput();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            followTransform = null;
        }
    }

    void HandleMouseInput() {

        // Mouse wheel zoom

        if (Input.mouseScrollDelta.y != 0) {
            
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }

        // Mouse drag controls

        if (Input.GetMouseButtonDown(0)) {

            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry)) {
                dragStartPosition = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(0)) {

            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry)) {
                
                dragEndPosition = ray.GetPoint(entry);
                newPosition = transform.position + dragStartPosition - dragEndPosition;
            }
        }

        // Mouse rotation by middle button

        if (Input.GetMouseButtonDown(2)) {

            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2)) {

            rotateEndPosition = Input.mousePosition;
            Vector3 difference = rotateStartPosition - rotateEndPosition;
            rotateStartPosition = rotateEndPosition;
            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }
    }

    void HandleKeyboardInput() {

        if (Input.GetKey(KeyCode.LeftShift)) {
            moveSpeed = fastSpeed;
        }
        else {
            moveSpeed= normalSpeed;
        }

        // Camera position controls - horizontal and vertical

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            newPosition += transform.forward * moveSpeed;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            newPosition += transform.forward * -moveSpeed;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            newPosition += transform.right * moveSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            newPosition += transform.right * -moveSpeed;
        }

        // Camera rotation controls

        if (Input.GetKey(KeyCode.Q)) {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        if (Input.GetKey(KeyCode.E)) {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        // Camera zooming controls

        if (Input.GetKey(KeyCode.R)) {
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.F)) {
            newZoom -= zoomAmount;
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * moveTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * moveTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * moveTime);

    }
}
