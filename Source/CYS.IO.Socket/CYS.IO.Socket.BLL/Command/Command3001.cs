using System;
using System.IO;
using CYS.IO.Socket.Core.Common;
using CYS.IO.Socket.Core.Interface;
using CYS.IO.Socket.Core.Utility;

namespace CYS.IO.Socket.BLL.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class Command3001 : ICommandFunc
    {
        #region ICommandFunc 成员

        public CommandProfile profile { get; set; }

        public SocketState Execute(CommunicateProtocol protocol, SocketState state, IHSSocket sobj)
        {
            state.IsReceiveThreadAlive = false;

            if (protocol == null) throw new ArgumentNullException("Protocol");

            byte[] body_stream1 = new byte[protocol.Stream1Len];

            var fName = "XXX.dat";

            try
            { 
                if (protocol.Stream1Len > 0)
                {
                    body_stream1 = ProtocolMgmt.ReceiveBytes(state, protocol.Stream1Len, sobj);
                    fName = System.Text.Encoding.UTF8.GetString(body_stream1, 0, protocol.Stream1Len).Trim('\0');
                }

                // TODO: Create Folder
                // FileOperator.CreateFolder("");

                if (protocol.Stream2Len > 0)
                {
                    using (FileStream fs = new FileStream(profile.DownLoadPath + fName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, 5))
                    {
                        foreach (var bytes in ProtocolMgmt.ReceiveProtocolStream(state, protocol, sobj))
                        {
                            fs.Write(bytes, 0, bytes.Length);
                        }
                    }
                }
            }
            catch (System.Net.Sockets.SocketException SktEx)
            {
                FileHelper.DeleteFile(profile.DownLoadPath + fName);

                state.OutPutMsg = string.Format("Command 3001 ：客户端连接异常,Socket Error Code : {0}", SktEx.ErrorCode);
                throw new Exception(state.OutPutMsg);
            }
            catch (Exception Ex)
            {
                FileHelper.DeleteFile(profile.DownLoadPath + fName);

                var ErrorAck = ProtocolMgmt.InitProtocolHeader(998, System.Text.Encoding.UTF8.GetBytes(Ex.Message), null);
                ProtocolMgmt.SendProtocol(state, ErrorAck, sobj);

                state.OutPutMsg = string.Format("Command 3001 ：接收客户端上传文件失败,{0}", Ex.Message);
                throw new Exception(state.OutPutMsg);
            }

            var Ack = ProtocolMgmt.InitProtocolHeader(999, System.Text.Encoding.UTF8.GetBytes("OK"), null);
            ProtocolMgmt.SendProtocol(state, Ack, sobj);
            state.OutPutMsg = string.Format("Command 3001 ：接收客户端上传文件{0}", "成功");

            return state;
        }

        #endregion
    }
}
