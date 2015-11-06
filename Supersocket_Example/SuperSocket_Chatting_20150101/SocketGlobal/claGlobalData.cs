using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CustomClass;
using SocketGlobal.SendData;

namespace SocketGlobal
{
	public class claGlobal
	{
		public static string g_SiteTitle = "Socket - SocketAsyncEventArgs";

		/// <summary>
		/// 명령어 구분용 문자1
		/// </summary>
		public static char g_Division1 = '▦';
		/// <summary>
		/// 명령어 구분용 문자2
		/// </summary>
		public static char g_Division2 = ',';

		/// <summary>
		/// 명령어의 크기
		/// </summary>
		public static int g_CommandSize = 4;
		/// <summary>
		/// 메시지의 데이터크기가 큰경우 자르기위한 크기.
		/// 보통 1개의 메시지는 1024byte크기인데
		/// 해더 4byte가 필요하므로 1002byte이하로 지정해야 한다.
		/// (하지만 안전한 전송을 위해 테스트를 하자.권장은 1000byte이하)
		/// 만약 별도해더가 더붙는 경우 별도해더의 크기를 생각해서 지정해야 한다.
		/// </summary>
		public static int g_CutByteSize = 995;
		/// <summary>
		/// 데이터에 별도의 헤더1을 붙이는 경우 해더의 크기
		/// </summary>
		public static int g_DataHeader1Size = 10;
		/// <summary>
		/// 데이터에 별도의 헤더2를 붙이는 경우 해더의 크기
		/// </summary>
		public static int g_DataHeader2Size = 10;

		/// <summary>
		/// 소켓에서 사용되는 변환 관련 유틸들
		/// </summary>
		public static claSocketUtile g_SocketUtile = new claSocketUtile();

		public static claCommand g_Command = new claCommand();
		public static claNumber g_Number = new claNumber();
		
	}
}
