using CYS.IO.Socket.Core.Common;
using CYS.IO.Socket.Core.Interface;

namespace CYS.IO.Socket.BLL.Command
{
    public class Command0 : ICommandFunc
    {
        #region ICommandFunc 成员

        public CommandProfile profile { get; set; }

        public SocketState Execute(CommunicateProtocol protocol, SocketState state, IHSSocket sobj)
        {
            state.IsReceiveThreadAlive = false; // 接收完数据后立即解锁
            state.OutPutMsg = string.Format("Command 0 ：命令已弃用");
            return state;
        }
        #endregion
    }
}
