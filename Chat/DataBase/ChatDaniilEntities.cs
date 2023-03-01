using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DataBase
{
    public partial class ChatDaniilEntities
    {
        private static ChatDaniilEntities context;
        public static ChatDaniilEntities GetContext()
        {
            if (context == null)
                context = new ChatDaniilEntities();
            return context;
        }
    }
}
