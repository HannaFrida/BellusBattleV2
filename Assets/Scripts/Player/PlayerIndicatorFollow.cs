using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicatorFollow : MonoBehaviour {

    [SerializeField] private Transform rigTransform;

    private bool follow;
    private void Awake() {
        follow = false;
    }

    // Update is called once per frame
    void Update() {
        
        //X and Y position of indicator is overriden from default parent-child to follow the rig
        if (follow) {
            var position = rigTransform.position;
            //print(rigTransform.position);
            gameObject.transform.position = new Vector3(position.x, position.y+2.2f, 0f);
        }

    }

    
    
    public void Follow() {
        follow = true;
    }

    public void UnFollow() {
        //print("unfollowed");
        follow = false;
        gameObject.transform.localPosition = new Vector3(0f, 2.2f, 0f);
        var position = rigTransform.position;
        
        
    }
    
}
