using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float input_horizontal;
    private float input_vertical;
    private float angulo_direccion;
    private float fuerza_breack_actual;
    private bool is_braking;
    private bool is_paused = false;

    [SerializeField] private float fuerza_motor;
    [SerializeField] private float fuerza_break;
    [SerializeField] private float angulo_direccion_maximo;
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;
    [SerializeField] private GameObject boton_pausa;
    [SerializeField] private GameObject boton_reanudar;
    [SerializeField] private GameObject boton_next;
    [SerializeField] private GameObject panel_general;
    [SerializeField] private Text text_title;
    [SerializeField] private Text text_message;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            this.TogglePause();
        //print(this.frontLeftWheelCollider.CompareTag("Win"));
        //if(this.frontLeftWheelCollider.CompareTag("Win") && this.frontRightWheelCollider.CompareTag("Win") && this.rearLeftWheelCollider.CompareTag("Win") && this.rearRightWheelCollider.CompareTag("Win"))
        //{
        //    print("Winnadfad");
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            ToggleGameOver();
        }
        if(collision.gameObject.CompareTag("Win"))
        {
            ToggleWin();
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }

    public void ToggleGameOver()
    {
        this.text_title.text = "Game Over";
        this.text_message.text = "No golpee otros autos u obstáculos. \nEvite salirse de la carretera.";
        TogglePanel();
    }

    public void ToggleWin()
    {
        this.text_title.text = "Win";
        this.text_message.text = "Felicidades has ganado.s";
        TogglePanel();
    }

    public void TogglePause()
    {
        this.text_title.text = "Pausa";
        this.text_message.text = "";
        this.boton_reanudar.SetActive(true);
        this.boton_next.SetActive(false);
        TogglePanel();
    }

    public void TogglePanel()
    {
        this.is_paused = !this.is_paused;
        this.boton_pausa.SetActive(!this.is_paused);
        this.panel_general.SetActive(this.is_paused);

        Time.timeScale = this.is_paused ? 0.0f : 1.0f;
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        this.input_horizontal = Input.GetAxis(HORIZONTAL);
        this.input_vertical = Input.GetAxis(VERTICAL);
        this.is_braking = !Input.GetButton(VERTICAL);
    }

    private void HandleMotor()
    {
        this.frontLeftWheelCollider.motorTorque = this.input_vertical * this.fuerza_motor;
        this.frontRightWheelCollider.motorTorque = this.input_vertical * this.fuerza_motor;
        this.fuerza_breack_actual = this.is_braking ? this.fuerza_break : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        this.frontRightWheelCollider.brakeTorque = this.fuerza_breack_actual;
        this.frontLeftWheelCollider.brakeTorque = this.fuerza_breack_actual;
        this.rearLeftWheelCollider.brakeTorque = this.fuerza_breack_actual;
        this.rearRightWheelCollider.brakeTorque = this.fuerza_breack_actual;
    }

    private void HandleSteering()
    {
        this.angulo_direccion = this.angulo_direccion_maximo * this.input_horizontal;
        this.frontLeftWheelCollider.steerAngle = this.angulo_direccion;
        this.frontRightWheelCollider.steerAngle = this.angulo_direccion;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(this.frontLeftWheelCollider, this.frontLeftWheelTransform);
        UpdateSingleWheel(this.frontRightWheelCollider, this.frontRightWheelTransform);
        UpdateSingleWheel(this.rearLeftWheelCollider, this.rearLeftWheelTransform);
        UpdateSingleWheel(this.rearRightWheelCollider, this.rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) 
    {
        Vector3 posicion;
        Quaternion rotacion;
        wheelCollider.GetWorldPose(out posicion, out rotacion);
        wheelTransform.rotation = rotacion;
        wheelTransform.position = posicion;
    }

}
