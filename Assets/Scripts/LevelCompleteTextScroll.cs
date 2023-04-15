using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class LevelCompleteTextScroll : MonoBehaviour
{

    public float scrollSpeed;
    private RectTransform tf;
    void Start()
    {
        tf = gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (tf.position.x <= -2525) {
            tf.position = new Vector3(3669, tf.position.y, tf.position.z);
        }
        tf.position = Vector3.MoveTowards(tf.position, new Vector3(-2526, tf.position.y, tf.position.z), Time.deltaTime * scrollSpeed);
    }
}
