namespace RodneysLegacy
{
    class RLCreature
    {
        public int ID;
        public RLBrain Brain;
        public RLTile Tile; //where am i
        public string Texture; //key in the textures dict
        public int Facing;

        public RLCreature()
        {
            ID = -1;
            Brain = new RLBrain(this);
        }

        public string CreateSaveString()
        {
            return
                ID.ToString()+";"+
                Tile.Map.ID.ToString()+";"+
                Texture+";"+
                Tile.X.ToString()+","+Tile.Y.ToString()+";"+
                Facing.ToString()
                ;
        }
    }
}
