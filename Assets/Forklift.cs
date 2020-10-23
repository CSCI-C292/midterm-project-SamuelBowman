using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class Forklift : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _maxSteeringAngle;
    [SerializeField] float _steeringTorque;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") != 0) {
            //float steeringAngle = Input.GetAxis("Horizontal") * _maxSteeringAngle;
            //transform.Translate(PolarToVector3(_moveSpeed * Input.GetAxis("Vertical"), steeringAngle) * Time.deltaTime);
            //transform.Rotate(0, steeringAngle * Time.deltaTime, 0);
            GetComponent<Rigidbody>().AddForce(transform.forward * Input.GetAxis("Vertical") * _moveSpeed);
            GetComponent<Rigidbody>().AddTorque(transform.up * Input.GetAxis("Horizontal") * _steeringTorque);
        }
    }

    // Magic big brain math code from Arthur
    Vector3 PolarToVector3(float speed, float steeringAngle)
    {
        return Quaternion.AngleAxis(steeringAngle, Vector3.up) * transform.forward * speed;
    }
}
