using System;
using CYS.IO.Socket.Core.Common;
using CYS.IO.Socket.Core.Interface;

namespace CYS.IO.Socket.BLL.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class Command1001 : ICommandFunc
    {
        #region ICommandFunc 成员

        public CommandProfile profile { get; set; }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public SocketState Execute(CommunicateProtocol protocol, SocketState state, IHSSocket sobj)
        {

            state.IsReceiveThreadAlive = false;

            if (protocol == null) throw new ArgumentNullException("Protocol");

            //var pdaId = string.Empty;
            //if (protocol.Stream1Len > 0)
            //{
            //    body_stream1 = ProtocolFactory.ReceiveBytes(state, protocol.Stream1Len, sobj);
            //    pdaId = System.Text.Encoding.UTF8.GetString(body_stream1, 0, protocol.Stream1Len).Trim('\0');
            //}

            //if (string.IsNullOrEmpty(pdaId))
            //{
            //    var pACK = ProtocolFactory.InitProtocolHeader(998, Encoding.UTF8.GetBytes("解析PDA编号失败"), null);
            //    ProtocolFactory.SendProtocol(state, pACK, sobj);
            //    state.OutPutMsg = string.Format("Command 1001 ：接收客户端上传请求{0}", "失败");
            //}
            //else
            //{
            //    var pACK = ProtocolFactory.InitProtocolHeader(2001, Encoding.UTF8.GetBytes("OK"), null);
            //    ProtocolFactory.SendProtocol(state, pACK, sobj);
            //    state.OutPutMsg = string.Format("Command 1001 ：接收客户端上传请求{0}", "成功");
            //}

            var pACK = ProtocolMgmt.InitProtocolHeader(2001, System.Text.Encoding.UTF8.GetBytes("OK"), null);
            ProtocolMgmt.SendProtocol(state, pACK, sobj);
            state.OutPutMsg = string.Format("Command 1001 ：接收客户端上传请求{0}", "成功");

            return state;
        }

        #endregion
    }
}
