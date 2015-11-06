using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomClass;

namespace SocketGlobal
{
	public class claSocketUtile : claEncoding
	{
		/// <summary>
		/// 지정한 숫자를 'g_DataHeader1Size'의 크기에 맞게 헤더를 생성합니다.
		/// </summary>
		/// <param name="intData"></param>
		/// <returns></returns>
		public byte[] IntToByte(int intData)
		{
			return claGlobal.g_SocketUtile.StringToByte(string.Format("{0:D10}", intData.GetHashCode()));
		}

		/// <summary>
		/// 지정한 바이트배열을 숫자로 바꿔준다.
		/// </summary>
		/// <param name="byteData"></param>
		/// <returns></returns>
		public int ByteToInt(byte[] byteData)
		{
			return claGlobal.g_Number.StringToInt(claGlobal.g_SocketUtile.ByteToString(byteData));
		}
	}
}
