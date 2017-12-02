using UnityEngine;

namespace DefaultNamespace
{
    public class Cell
    {
        public Vector2 Position { get; private set; }
        public Vector2 Size     { get; private set; }
        public Cell(Vector2 position, Vector2 size)
        {
            
            Position = position;
            Size = size;
        }
    }
}