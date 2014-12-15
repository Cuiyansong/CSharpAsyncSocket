using System;
using CYS.IO.Socket.Core.Common;
using CYS.IO.Socket.Core.Interface;

namespace CYS.IO.Socket.BLL.Command
{
    public class Command1 : ICommandFunc
    {
        #region ICommandFunc 成员

        public CommandProfile profile { get; set; }


        public SocketState Execute(CommunicateProtocol protocol, SocketState state, IHSSocket sobj)
        {
            if (protocol == null) throw new ArgumentNullException("Protocol");

            var p = ProtocolMgmt.InitProtocolHeader(1001, null, null);
            ProtocolMgmt.SendProtocol(state, p, sobj);
            state.OutPutMsg = string.Format("Command 1 ：请求发送本地文件 {0}。", profile.UpLoadPath + profile.UpLoadFName);

            return state;
        }

        #endregion
    }
}
