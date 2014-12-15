using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CYS.IO.Socket.Core.Common;
using CYS.IO.Socket.Core.Interface;

namespace CYS.IO.Socket.Core.Base
{
    /// <summary>
    /// SocketBase
    /// </summary>
    public abstract class HSSocketBase : IHSSocket
    {
        #region Abstract 成员
        public abstract void InvokeExecuteProtocolCommand(CommunicateProtocol protocol, SocketState state);

        public abstract void SetConnection(bool p, SocketState state);

        public abstract void WriteLog(Exception SktEx, string p);

        public abstract void Mark(string p);
        #endregion

        #region ISocket 成员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="bytes"></param>
        public virtual void SyncBufferSend(SocketState ss, byte[] bytes)
        {
            if (bytes.Length == 0) return;
            var handler = ss.workSocket;

            int sendIndex = 0;
            int totalLen = bytes.Length;
            while (sendIndex < totalLen)
            {
                byte[] buffer = new byte[8 * 1024];
                Array.Copy(bytes, sendIndex, buffer, 0, totalLen - sendIndex >= buffer.Length ? buffer.Length : totalLen - sendIndex);
                int realsendLen = handler.Send(buffer, 0, totalLen - sendIndex >= buffer.Length ? buffer.Length : totalLen - sendIndex, SocketFlags.None);
                sendIndex += realsendLen;
            }
            Mark(string.Format("发送字节 {0}KB", bytes.Length / (float)1024));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public virtual byte[] SyncBufferReceive(SocketState ss, int len)
        {
            var handler = ss.workSocket;
            byte[] cache = new byte[len];
            int receiveIndex = 0;
            while (receiveIndex < len)
            {
                byte[] buffer = new byte[8 * 1024];
                int realreceiveLen = handler.Receive(buffer, 0, len - receiveIndex >= buffer.Length ? buffer.Length : len - receiveIndex, SocketFlags.None);
                Array.Copy(buffer, 0, cache, receiveIndex, realreceiveLen);
                receiveIndex += realreceiveLen;
            }
            Mark(string.Format("接收字节 {0}KB", len / (float)1024));
            return cache;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ss"></param>
        public virtual void ContinueAsyncReceive(SocketState ss)
        {
            // 如果接收线程不存在，则新建一个接收线程；否则不做任何事。
            if (!ss.IsReceiveThreadAlive)
            {
                try
                {
                    ss.IsReceiveThreadAlive = true;
                    ss.workSocket.BeginReceive(ss.buffer, 0, 2, 0, new AsyncCallback(AsyncReceivedCallback), ss);
                    Mark("继续监听");
                }
                catch
                {
                    SetConnection(false, ss);
                }
            }
        }

        /// <summary>
        /// AsyncReceivedCallback
        /// </summary>
        /// <param name="ar"></param>
        public void AsyncReceivedCallback(IAsyncResult ar)
        {
            SocketState state = (SocketState)ar.AsyncState;

            try
            {
                System.Net.Sockets.Socket client = state.workSocket;
                int bytesRead = client.EndReceive(ar);
                if (bytesRead > 0)
                {
                    Mark(string.Format("分析协议数据包"));
                    byte[] header_flag = new byte[2];
                    Array.Copy(state.buffer, 0, header_flag, 0, 2);
                    if (CommunicateProtocol.CheckFlag(header_flag))
                    {
                        client.BeginReceive(state.buffer, 0, 12, 0, new AsyncCallback(SyncExecuteProtocolCommand), state);
                    }
                    else
                    {
                        client.BeginReceive(state.buffer, 0, 2, 0, new AsyncCallback(AsyncReceivedCallback), state);
                    }
                }
                else
                {
                    WriteLog(null, "客户端" + IPAddress.Parse(((IPEndPoint)state.workSocket.RemoteEndPoint).Address.ToString()) + "连接异常");
                    SetConnection(false, state);
                }
            }
            catch (System.Net.Sockets.SocketException sktEx)
            {
                WriteLog(sktEx, sktEx.ErrorCode.ToString());
                SetConnection(false, state);
            }
            catch (Exception Ex)
            {
                WriteLog(Ex, Ex.Message);
                SetConnection(false, state);
            }
        }

        /// SyncExecuteProtocolCommand
        /// </summary>
        /// <param name="ar"></param>
        public virtual void SyncExecuteProtocolCommand(IAsyncResult ar)
        {
            SocketState state = (SocketState)ar.AsyncState;
            try
            {
                System.Net.Sockets.Socket handler = state.workSocket;
                int bytesRead = handler.EndReceive(ar);

                Mark(string.Format("解析协议数据头"));

                if (bytesRead > 0)
                {
                    var protocol = ProtocolMgmt.InitProtocolHeader(state.buffer);

                    ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
                    {
                        Mark(string.Format("执行协议编号：{0}", protocol.Command));
                        InvokeExecuteProtocolCommand(protocol, state);
                    }));
                }
            }
            catch (System.Net.Sockets.SocketException SktEx)
            {
                WriteLog(SktEx, SktEx.ErrorCode.ToString());
                SetConnection(false, state);
            }
            catch (System.IO.IOException IoEx)
            {
                WriteLog(IoEx, IoEx.Message);
                SetConnection(false, state);
            }
            catch (Exception Ex)
            {
                WriteLog(Ex, Ex.Message);
                SetConnection(false, state);
            }
        }

        /// <summary>
        /// 设置心跳
        /// </summary>
        /// <param name="tmpsock"></param>
        public void SetHeartBeat(SocketState ss)
        {
            var tmpsock = ss.workSocket;
            int keepAliveNum = -1744830460; // SIO_KEEPALIVE_VALS
            byte[] inValue = new byte[] { 1, 0, 0, 0, 0x20, 0x4e, 0, 0, 0xd0, 0x07, 0, 0 };// 首次探测时间20 秒, 间隔侦测时间2 秒
            tmpsock.IOControl(keepAliveNum, inValue, null);
        }

        #endregion
    }
}
