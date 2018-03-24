using System.Collections.Generic;

namespace Model.Orders
{
    public class ShipOrder
    {
        public float Acceleration;

        public List<ComponentOrder> ComponentOrders;

        public int ShipId;
        public float Turn;
    }
}