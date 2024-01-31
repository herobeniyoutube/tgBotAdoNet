using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    public class ResultTopic
    {
        public int message_thread_id { get; set; }
        public string name { get; set; }
        public int icon_color { get; set; }
    }

    public class RootTopic
    {
        public bool ok { get; set; }
        public ResultTopic result { get; set; }
    }
}
