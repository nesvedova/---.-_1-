using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Test
{
    public class Film
    {
        public int UUID_of_film { get; set; }
        public string Name_of_film { get; set; }
        public string Day { get; set; }
        public string Date { get; set; }
        public string Title_of_film { get; set; }
        public string Option { get; set; }
        public string Time { get; set; }
        public int Count_of_ticket { get; set; }
        public string MyProperty { get; set; }
        public List<Tiket> Tikets { get; set; }
    }
}
