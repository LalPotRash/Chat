using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DataBase
{
    public partial class Chatroom
    {
        public DateTime LastMessageDate => (DateTime)ChatMessages?.Max(x => x?.Date);
    }
}
