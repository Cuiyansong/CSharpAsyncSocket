using System;
using System.IO;
using CYS.IO.Socket.Core.Common;
using CYS.IO.Socket.Core.Interface;
using CYS.IO.Socket.Core.Utility;


namespace CYS.IO.Socket.BLL.Command
{
    /// <summary>
    /// Command999 确认应答
    /// </summary>
    public class Command999 : ICommandFunc
    {
        #region ICommandFunc 成员

        public CommandProfile profile { get; set; }

        public SocketState Execute(CommunicateProtocol protocol, SocketState state, IHSSocket sobj)
        {
            state.IsReceiveThreadAlive = false; // 接收完数据后立即解锁

            if (protocol == null) throw new ArgumentNullException("Protocol");

            string Msg = string.Empty;
            if (protocol.Stream1Len > 0)
            {
                byte[] stream1 = ProtocolMgmt.ReceiveBytes(state, protocol.Stream1Len, sobj);
                Msg = string.Format("Command 999 ：成功信息 {0}", System.Text.Encoding.UTF8.GetString(stream1, 0, protocol.Stream1Len).Trim('\0'));
            }

            state.OutPutMsg = string.Format("Command 999 ：成功信息 {0}", Msg);

            return state;
        }

        #endregion
    }
}
