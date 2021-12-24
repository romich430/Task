using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Task{
    public class Cube
    {
        public List<Vector3> vertices;

        public Cube(){}

        public Cube(Vector3 center, float width){
            vertices = new List<Vector3>{
                new Vector3 (center.x - width, center.y - width, 0 - width),
                new Vector3 (center.x - width, center.y - width, 0 + width),
                new Vector3 (center.x + width, center.y - width, 0 + width),
                new Vector3 (center.x + width, center.y - width, 0 - width),
                new Vector3 (center.x - width, center.y + width, 0 - width),
                new Vector3 (center.x - width, center.y + width, 0 + width),
                new Vector3 (center.x + width, center.y + width, 0 + width),
                new Vector3 (center.x + width, center.y + width, 0 - width),
            };
        }
    }
}