using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CYS.IO.Socket.Core.Common;
using CYS.IO.Socket.Core.Interface;
using CYS.IO.Socket.BLL.Command;

namespace CYS.IO.Socket.BLL.Common
{
    public abstract class ContractAdapter
    {
        #region Public Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        protected void ExecuteProtocolCommand(CommunicateProtocol p, SocketState s)
        {
            if (p == null) throw new ArgumentNullException("CommunicateProtocol is null");
            switch (p.Command)
            {
                case 0: CommandWrapper(((ICommandFunc)new Command0()), p, s); break;
                case 1: CommandWrapper(((ICommandFunc)new Command1()), p, s); break;
                case 2: CommandWrapper(((ICommandFunc)new Command2()), p, s); break;
                case 998: CommandWrapper(((ICommandFunc)new Command998()), p, s); break;
                case 999: CommandWrapper(((ICommandFunc)new Command999()), p, s); break;
                // 
                case 1001: CommandWrapper(((ICommandFunc)new Command1001()), p, s); break;
                case 1002: CommandWrapper(((ICommandFunc)new Command1002()), p, s); break;
                //
                case 2001: CommandWrapper(((ICommandFunc)new Command2001()), p, s); break;
                case 2002: CommandWrapper(((ICommandFunc)new Command2002()), p, s); break;
                //
                case 3001: CommandWrapper(((ICommandFunc)new Command3001()), p, s); break;

                default: throw new Exception("Protocol type does not exist.");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <param name="p"></param>
        /// <param name="s"></param>
        protected abstract void CommandWrapper(ICommandFunc func, CommunicateProtocol p, SocketState s);
        #endregion Public Method
    }
}
