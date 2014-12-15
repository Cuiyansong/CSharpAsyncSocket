using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CYS.IO.Socket.Core.Common
{
    /// <summary>
    /// 通讯二进制协议，此协议基于变长的流传输。
    /// 注：扩展此方法成员时，请重写相关方法。
    /// </summary>
    /// <remarks>
    /// Create By CYS
    /// </remarks>
    public class CommunicateProtocol : IDisposable
    {
        #region Public Properties
        /// <summary>
        /// Byte array length of flag
        /// </summary>
        public const int ByteLength_HeaderFlag = 2;
        /// <summary>
        /// Byte array length of command
        /// </summary>
        public const int ByteLength_HeaderCmd = 4;
        /// <summary>
        /// Byte array length of header stream1
        /// </summary>
        public const int ByteLength_HeaderStream1Len = 4;
        /// <summary>
        /// Byte array length of header stream2
        /// </summary>
        public const int ByteLength_HeaderStream2Len = 4;
        /// <summary>
        /// 协议头长度
        /// </summary>
        public static int FlagLen
        {
            get { return ByteLength_HeaderFlag; }
        }
        /// <summary>
        /// 命令（Int32）
        /// </summary>
        public int Command
        {
            get
            {
                return BitConverter.ToInt32(header_Cmd, 0);
            }
            set
            {
                BitConverter.GetBytes(value).CopyTo(header_Cmd, 0);
            }
        }
        /// <summary>
        /// 流1长度
        /// </summary>
        /// <returns></returns>
        public int Stream1Len
        {
            get
            {
                return BitConverter.ToInt32(header_Stream1Len, 0);
            }
            set
            {
                BitConverter.GetBytes(value).CopyTo(header_Stream1Len, 0);
            }
        }
        /// <summary>
        /// 流2长度
        /// </summary>
        /// <returns></returns>
        public int Stream2Len
        {
            get
            {
                return BitConverter.ToInt32(header_Stream2Len, 0);
            }
            set
            {
                BitConverter.GetBytes(value).CopyTo(header_Stream2Len, 0);
            }
        }
        #endregion Public Properties

        #region Private Properties
        private static byte[] header_Flag = new byte[ByteLength_HeaderFlag];
        private byte[] header_Cmd = new byte[ByteLength_HeaderCmd];
        private byte[] header_Stream1Len = new byte[ByteLength_HeaderStream1Len];
        private byte[] header_Stream2Len = new byte[ByteLength_HeaderStream1Len];
        private byte[] body_Stream1 = new byte[0];
        private Stream body_Stream2;
        #endregion Private Properties

        #region Constructor
        /// <summary>
        /// Static constructor
        /// </summary>
        static CommunicateProtocol()
        {
            header_Flag = new byte[ByteLength_HeaderFlag] { 0xFF, 0x7E };
        }

        #endregion Constructor

        #region Public Method
        /// <summary>
        /// 判断是否是协议头标志
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool CheckFlag(byte[] bytes)
        {
            if (bytes.Length != header_Flag.Length)
                return false;
            if (bytes.Length != 2)
                return false;
            if (!bytes[0].Equals(header_Flag[0]) || !bytes[1].Equals(header_Flag[1]))
                return false;
            return true;
        }
        /// <summary>
        /// SetStream1
        /// </summary>
        /// <param name="sm"></param>
        public void SetStream1(byte[] sm)
        {
            body_Stream1 = sm;
        }
        /// <summary>
        /// GetStream1
        /// </summary>
        /// <returns></returns>
        public byte[] GetStream1()
        {
            return body_Stream1;
        }
        /// <summary>
        /// SetStream2
        /// </summary>
        /// <param name="sm"></param>
        public void SetStream2(Stream sm)
        {
            body_Stream2 = sm;
        }
        /// <summary>
        /// body_Stream2
        /// </summary>
        /// <returns></returns>
        public Stream GetStream2()
        {
            return body_Stream2;
        }
        /// <summary>
        /// GetHeaderBytes
        /// </summary>
        /// <returns></returns>
        public byte[] GetHeaderBytes()
        {
            int offset = 0;
            byte[] bytes = new byte[ByteLength_HeaderFlag + ByteLength_HeaderCmd + ByteLength_HeaderStream1Len + ByteLength_HeaderStream2Len];

            Array.Copy(header_Flag, 0, bytes, 0, ByteLength_HeaderFlag); offset += ByteLength_HeaderFlag;
            Array.Copy(header_Cmd, 0, bytes, offset, ByteLength_HeaderCmd); offset += ByteLength_HeaderCmd;
            Array.Copy(header_Stream1Len, 0, bytes, offset, ByteLength_HeaderStream1Len); offset += ByteLength_HeaderStream1Len;
            Array.Copy(header_Stream2Len, 0, bytes, offset, ByteLength_HeaderStream2Len); offset += ByteLength_HeaderStream2Len;

            return bytes;
        }
        /// <summary>
        /// InitProtocolHeader
        /// </summary>
        /// <returns></returns>
        public static CommunicateProtocol InitProtocolHeader(byte[] source)
        {
            if (source.Length < ByteLength_HeaderCmd + ByteLength_HeaderStream1Len + ByteLength_HeaderStream2Len)
            {
                throw new Exception("byte length is illegal");
            }

            byte[] header_cmd = new byte[ByteLength_HeaderCmd];
            byte[] header_stream1len = new byte[ByteLength_HeaderStream1Len];
            byte[] header_stream2len = new byte[ByteLength_HeaderStream2Len];
            Array.Copy(source, 0, header_cmd, 0, ByteLength_HeaderCmd);
            Array.Copy(source, ByteLength_HeaderCmd, header_stream1len, 0, ByteLength_HeaderStream1Len);
            Array.Copy(source, ByteLength_HeaderCmd + ByteLength_HeaderStream1Len, header_stream2len, 0, ByteLength_HeaderStream2Len);

            return new CommunicateProtocol
            {
                Command = BitConverter.ToInt32(header_cmd, 0),
                Stream1Len = BitConverter.ToInt32(header_stream1len, 0),
                Stream2Len = BitConverter.ToInt32(header_stream2len, 0),
            };
        }
        #endregion Public Method

        #region Private Method

        #endregion Private Method

        #region IDisposable 成员
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            header_Cmd = null;
            header_Stream1Len = null;
            header_Stream2Len = null;
            body_Stream1 = null;
            body_Stream2 = null;
        }

        #endregion
    }
}
