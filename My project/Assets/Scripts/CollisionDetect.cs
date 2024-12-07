using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    void OnCollisionEnter(Collision c) {
        if (c.gameObject.tag == "Ambiente") {
            print("Colidiu com o ambiente");
        }
    }
}
