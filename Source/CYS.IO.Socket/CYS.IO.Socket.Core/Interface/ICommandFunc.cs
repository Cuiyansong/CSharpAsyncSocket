using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CYS.IO.Socket.Core.Common;

namespace CYS.IO.Socket.Core.Interface
{
    public interface ICommandFunc
    {
        CommandProfile profile { get; set; }

        SocketState Execute(CommunicateProtocol protocol, SocketState state, IHSSocket sobj);
    }
}
