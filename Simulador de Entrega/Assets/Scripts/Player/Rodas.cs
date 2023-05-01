using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rodas : MonoBehaviour
{
    public RodaRotacao carroceria;
    public Rigidbody rb;
    public Transform vizRoda; //Vizualisacao da roda
    public Transform rotRoda; //Posi��o da roda visual

    public float distanciaSusp;
    public float raioRoda = 0.515f; //raio = diamentro/2, o modelo do gabriel possue uma roda com 1.03 de diametro
    public float forcamola; // constante
    public float rigidezAmortecedor;
    float tamanhoInicialSusp;
    float tamanhoAtualSusp;

    public float fE; // For�a el�stica  lei de hooke
    public float fA; // For�a Amortecedor = velocidade de deslocamento * for�a

    public float forcasus;
    float forcaAtritoLateral;
    float forcaAtritoLongitudinal;
    public float resRolamento;

    public AnimationCurve coefAtrito;

    Vector3 coleta;
    Vector3 velocidadeLocal;

     void Update()
    {
        rotRoda.transform.Rotate(Mathf.Rad2Deg * velocidadeLocal.z / raioRoda * Time.deltaTime, 0,0);
    }
     void Start()
    {
        rotRoda.localPosition = new Vector3(rotRoda.localPosition.x, coleta.y + raioRoda, rotRoda.localPosition.z);
    }
    void FixedUpdate()
    {
        if(Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, distanciaSusp + raioRoda))
        {
            coleta = hit.point;
            velocidadeLocal = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));

            Debug.DrawLine(transform.position, hit.point);
            tamanhoInicialSusp = tamanhoAtualSusp; 
            tamanhoAtualSusp = hit.distance - raioRoda;

            //Resultado da for�a el�stica da malo do ve�culo
            fE = forcamola * (distanciaSusp - tamanhoAtualSusp);

            //resultado da for�a gerada pelo amortecedor
            fA = rigidezAmortecedor * (tamanhoInicialSusp - tamanhoAtualSusp) / Time.fixedDeltaTime;
           
           //n�o deixar a roda passar a lataria do carro
           if(hit.distance > raioRoda)
            {
                forcasus = fE + fA;
            }
           else if(hit.distance < (raioRoda + 0.08f))
            {
                forcasus = fE + fA;
            }
           //forca de resistencia ao rolamento do pneu
            resRolamento = carroceria.fArrasto * 30 * Mathf.Abs(velocidadeLocal.z);

            forcaAtritoLateral = (coefAtrito.Evaluate(forcasus) * forcasus) * Mathf.Sign(velocidadeLocal.x);

            forcaAtritoLongitudinal = forcasus * Input.GetAxis("Vertical");

            //forca sendo aplicada
            rb.AddForceAtPosition((forcasus * transform.up) + (forcaAtritoLateral * -transform.right) + ((forcaAtritoLongitudinal + resRolamento) * transform.forward), hit.point);

            //visualiza��o do movimento que a suspens�o faz na roda
            vizRoda.transform.position = hit.point;
        }
       
    }
}
