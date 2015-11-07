using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketGlobal.SendData
{
	/// <summary>
	/// 서버와 클라이언트간의 메시지를 분석하기 위한 베이스
	/// </summary>
	public class baseSendData
	{
		/// <summary>
		/// 명령어
		/// </summary>
		public claCommand.Command Command { get; set; }


		public baseSendData()
		{
			this.Command = claCommand.Command.None;
		}

		public baseSendData(claCommand.Command typeCommand)
		{
			this.Command = typeCommand;
		}
	}
}
