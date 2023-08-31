namespace Engine.Models
{
    public class World
    {
        private List<Location> _locations = new List<Location>();
        internal void AddLocation(int xCoordinate, int yCoordinate, string name, string description, string imageName)
        {
            _locations.Add(new Location(xCoordinate, yCoordinate, name, description, imageName));
        }
        public Location? LocationAt(int xCoordinate, int yCoordinate)
        {
            return _locations.FirstOrDefault(loc => loc.XCoordinate == xCoordinate && loc.YCoordinate == yCoordinate);
        }
    }
}
