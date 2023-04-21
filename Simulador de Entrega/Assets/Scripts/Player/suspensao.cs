using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Suspensao : MonoBehaviour
{
    public Rigidbody rb;
    public float distanciaSuspensao;
    public float raioRoda;
    public float forcaMola;
    public float rigidezAmortecedor;
    float tamanhoInicialSuspensao;
    float tamanhoAtualSuspensao;
    public Transform vizRoda;
    public Transform rotRoda;//posição da roda visual

    public float fE; // força elastica
    public float fA; //velocidade de deslocamento * forca

    public float forcaSuspensao;

    Vector3 coleta;
    Vector3 velocidadeLocal;


    private void Start()
    {
        rotRoda.localPosition = new Vector3(rotRoda.localPosition.x, coleta.y, rotRoda.localPosition.z);

    }

    private void Updtade()
    {
      rotRoda.transform.Rotate(Mathf.Rad2Deg * velocidadeLocal.z/raioRoda * Time.deltaTime, 0, 0);
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit pow, distanciaSuspensao + raioRoda))
        {
            coleta = pow.point;
            velocidadeLocal = transform.InverseTransformDirection(rb.GetPointVelocity(pow.point));

            Debug.DrawLine(transform.position, pow.point);
            tamanhoInicialSuspensao = tamanhoAtualSuspensao;
            tamanhoAtualSuspensao = pow.distance - raioRoda;

            fE = forcaMola * (distanciaSuspensao - tamanhoAtualSuspensao);
            fA = rigidezAmortecedor * (tamanhoInicialSuspensao - tamanhoAtualSuspensao) / Time.fixedDeltaTime;

            if (pow.distance > raioRoda)
            {
                forcaSuspensao = fE + fA;
            }
            else if(pow.distance < (raioRoda + 0.0008f))
            {
                forcaSuspensao = (fE + fA) ;
            }
            rb.AddForceAtPosition((forcaSuspensao * transform.up),pow.point);
            vizRoda.transform.position = pow.point;
        }
    }
}
