using System;
using System.IO;
using CYS.IO.Socket.Core.Common;
using CYS.IO.Socket.Core.Interface;
using CYS.IO.Socket.Core.Utility;

namespace CYS.IO.Socket.BLL.Command
{
    /// <summary>
    /// Command2001
    /// </summary>
    public class Command2001 : ICommandFunc
    {

        #region ICommandFunc 成员

        public CommandProfile profile { get; set; }

        public SocketState Execute(CommunicateProtocol protocol, SocketState state, IHSSocket sobj)
        {
            state.IsReceiveThreadAlive = false;

            if (protocol == null) throw new ArgumentNullException("Protocol");

            string fullName = profile.UpLoadPath + profile.UpLoadFName;
            if (!FileHelper.IsFileExist(fullName))
            {
                state.OutPutMsg = string.Format("Command 2001 ：发送本地文件失败，找不到所需文件{0}{1}", profile.UpLoadPath, profile.UpLoadFName);
                return state;
            }
            if (!FileHelper.CanRead(fullName))
            {
                state.OutPutMsg = string.Format("Command 2001 ：读取本地文件失败 {0}{1}", profile.UpLoadPath, profile.UpLoadFName);
                return state;
            }

            FileInfo fi = new FileInfo(fullName);

            try
            {
                using (FileStream fs = new FileStream(fi.FullName, FileMode.Open))
                {
                    var p = ProtocolMgmt.InitProtocolHeader(3001, System.Text.Encoding.UTF8.GetBytes(profile.PID + fi.Name), fs);
                    ProtocolMgmt.SendProtocol(state, p, sobj);
                    state.OutPutMsg = string.Format("Command 2001 ：发送本地文件 {0} 成功。", fullName);
                }
            }
            catch (Exception Ex)
            {
                state.OutPutMsg = string.Format("Command 2001 ：读取本地文件失败 {0}", Ex.Message);
            }

            return state;
        }

        #endregion
    }
}
