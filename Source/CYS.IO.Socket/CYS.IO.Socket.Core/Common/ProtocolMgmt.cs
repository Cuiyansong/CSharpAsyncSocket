using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CYS.IO.Socket.Core.Interface;
using System.IO;

namespace CYS.IO.Socket.Core.Common
{
    /// <summary>
    ///  Protocol Managment
    /// </summary>
    /// <remarks>
    /// Create By CYS
    /// </remarks>
    public class ProtocolMgmt
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static CommunicateProtocol InitProtocolHeader(byte[] source)
        {
            return CommunicateProtocol.InitProtocolHeader(source);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static CommunicateProtocol InitProtocolHeader(CommunicateProtocol p, byte[] s1, Stream s2)
        {
            if (s1 != null && s1.Length > 0)
            {
                p.Stream1Len = (int)s1.Length;
                p.SetStream1(s1);
            }
            if (s2 != null && s2.Length > 0)
            {
                p.Stream2Len = (int)s2.Length;
                p.SetStream2(s2);
            }
            return p;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static CommunicateProtocol InitProtocolHeader(int cmd, byte[] s1, Stream s2)
        {
            return InitProtocolHeader(new CommunicateProtocol { Command = cmd, }, s1, s2);
        }
        /// <summary>
        /// ReceiveBytes
        /// </summary>
        /// <param name="state"></param>
        /// <param name="length"></param>
        /// <param name="sobj"></param>
        /// <returns></returns>
        public static byte[] ReceiveBytes(SocketState state, int length, IHSSocket sobj)
        {
            return sobj.SyncBufferReceive(state, length);
        }
        /// <summary>
        /// ReceiveProtocolStream
        /// </summary>
        /// <param name="state"></param>
        /// <param name="protocol"></param>
        /// <param name="sobj"></param>
        /// <returns></returns>
        public static IEnumerable<byte[]> ReceiveProtocolStream(SocketState state, CommunicateProtocol protocol, IHSSocket sobj)
        {
            if (protocol == null) throw new Exception("Protocol should not be null.");
            if (state == null) throw new Exception("StateClient should not be null.");
            if (sobj == null) throw new Exception("client should not be null.");

            int readIndex = 0;
            int fileLen = protocol.Stream2Len;
            while (readIndex < fileLen)
            {
                int bufferLen = GlobalConfig.Protocol_Buffer_Len;
                int readLen = fileLen - readIndex >= bufferLen ? bufferLen : fileLen - readIndex;
                var cache = sobj.SyncBufferReceive(state, readLen);
                readIndex += cache.Length;
                yield return cache;
            }
        }
        /// <summary>
        /// SendProtocol
        /// </summary>
        /// <param name="state"></param>
        /// <param name="protocol"></param>
        /// <param name="sobj"></param>
        public static void SendProtocol(SocketState state, CommunicateProtocol protocol, IHSSocket sobj)
        {
            if (protocol == null) throw new Exception("Protocol should not be null.");
            if (state == null) throw new Exception("StateClient should not be null.");
            if (sobj == null) throw new Exception("client should not be null.");

            sobj.SyncBufferSend(state, protocol.GetHeaderBytes());

            if (protocol.GetStream1().Length > 0)
                sobj.SyncBufferSend(state, protocol.GetStream1());

            if (protocol.GetStream2() != null)
            {
                var fs = protocol.GetStream2() as Stream;
                if (fs != null)
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    int rIndex = 0;
                    int fileLen = (int)fs.Length;
                    while (rIndex < fs.Length)
                    {
                        byte[] cache = new byte[GlobalConfig.Protocol_Buffer_Len];
                        int len = fs.Read(cache, 0, fileLen - rIndex >= cache.Length ? cache.Length : fileLen - rIndex);
                        if (len < cache.Length)
                        {
                            byte[] bytes = new byte[len];
                            Array.Copy(cache, 0, bytes, 0, bytes.Length);
                            sobj.SyncBufferSend(state, bytes);
                        }
                        else
                            sobj.SyncBufferSend(state, cache);
                        rIndex += len;
                    }
                }
            }
        }
    }
}
