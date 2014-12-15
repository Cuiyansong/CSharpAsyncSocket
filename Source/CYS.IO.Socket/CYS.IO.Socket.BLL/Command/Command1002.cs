using System.IO;
using CYS.IO.Socket.Core.Common;
using CYS.IO.Socket.Core.Interface;
using CYS.IO.Socket.Core.Utility;

namespace CYS.IO.Socket.BLL.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class Command1002 : ICommandFunc
    {
        #region ICommandFunc 成员
        public CommandProfile profile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="state"></param>
        /// <param name="sobj"></param>
        /// <returns></returns>
        public SocketState Execute(CommunicateProtocol protocol, SocketState state, IHSSocket sobj)
        {
            state.IsReceiveThreadAlive = false;

            // Check File
            if (!FileHelper.IsFileExist(profile.UpLoadPath + profile.UpLoadFName))
            {
                var p = ProtocolMgmt.InitProtocolHeader(998, System.Text.Encoding.UTF8.GetBytes("服务端文件不存在"), null);
                ProtocolMgmt.SendProtocol(state, p, sobj);
                state.OutPutMsg = string.Format("Command 1002 ：服务端文件不存在");
                return state;
            }
            if (!FileHelper.CanRead(profile.UpLoadPath + profile.UpLoadFName))
            {
                var p = ProtocolMgmt.InitProtocolHeader(998, System.Text.Encoding.UTF8.GetBytes("文件已被打开或占用，请稍后重试"), null);
                ProtocolMgmt.SendProtocol(state, p, sobj);
                state.OutPutMsg = string.Format("Command 1002 ：文件已被打开或占用");
                return state;
            }

            FileInfo fi = new FileInfo(profile.UpLoadPath + profile.UpLoadFName);
            using (FileStream fs = new FileStream(profile.UpLoadPath + profile.UpLoadFName, FileMode.Open))
            {
                var p = ProtocolMgmt.InitProtocolHeader(2002, System.Text.Encoding.UTF8.GetBytes(fi.Name), fs);
                ProtocolMgmt.SendProtocol(state, p, sobj);
                state.OutPutMsg = string.Format("Command 1002 ：发送文件 {0} 成功。", fi.FullName);
            }

            return state;
        }


        #endregion
    }
}
