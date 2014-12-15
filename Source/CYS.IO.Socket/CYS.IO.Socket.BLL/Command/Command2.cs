using System;
using CYS.IO.Socket.Core.Common;
using CYS.IO.Socket.Core.Interface;

namespace CYS.IO.Socket.BLL.Command
{
    public class Command2 : ICommandFunc
    {
        #region ICommandFunc 成员
        public CommandProfile profile { get; set; }

        public SocketState Execute(CommunicateProtocol protocol, SocketState state, IHSSocket client)
        {
            if (protocol == null) throw new ArgumentNullException("Protocol");

            var p = ProtocolMgmt.InitProtocolHeader(1002, null, null);
            ProtocolMgmt.SendProtocol(state, p, client);
            state.OutPutMsg = string.Format("Command 2 ：向服务端请求下载文件");
            return state;
        }
        #endregion
    }
}
