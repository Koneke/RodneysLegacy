namespace RodneysLegacy
{
    class RLCreature
    {
        public int ID;
        public RLTile Tile; //where am i
        public string Texture; //key in the textures dict

        public RLCreature()
        {
            ID = -1;
        }
    }
}
