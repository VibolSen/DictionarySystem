using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamTemplate
{
    public class Translation
    {
        public string English { get; set; }
        public string German { get; set; }

        public Translation(string english, string german)
        {
            English = english;
            German = german;
        }
    }
}
