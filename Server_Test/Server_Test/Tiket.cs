using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Test
{
   public class Tiket
    {
        public int Ticket_ID { get; set; }
        public int Row { get; set; }
        public int Place { get; set; }
        public int Price { get; set; }
        public bool Reserved { get; set; }
    }
}
