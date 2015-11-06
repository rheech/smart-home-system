using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGlobal.SendData;

namespace SocketGlobal.LargeData
{
	/// <summary>
	/// 큰데이터를 보내기위한 클래스
	/// </summary>
	public class claLargeData_SendManager
	{
		/// <summary>
		/// 보내는 파일 인덱스
		/// 보내는 족에서 구분하기 위한 인덱스.
		/// </summary>
		public int m_nFileCount = 0;

		/// <summary>
		/// 큰데이터 클래스 생성
		/// </summary>
		private List<claLargeData_SendData> m_listLD_SendData
					= new List<claLargeData_SendData>();

		public dataSend Ready(claCommand.Command typeCommand
								, string sDir
								, typeLargeData typeLargeData)
		{
			//파일 인덱스를 늘려주고
			++this.m_nFileCount;

			//큰데이터 보내기 클래스
			claLargeData_SendData insLarge
				= new claLargeData_SendData();
			//큰데이터 구분용 인덱스
			insLarge.Index = this.m_nFileCount;
			//리스트에 만든 큰데이터 클래스를 넣어 준다.
			this.m_listLD_SendData.Add(insLarge);


			//파일을 부른다.
			insLarge.LoadFile(sDir);


			//서버로 보낼 메시지를 완성한다.
			dataSend insSend = new dataSend();
			insSend.Command = typeCommand;

			//큰데이터 보내기 준비를 한다.
			insSend.Data_List.Add(typeLargeData.GetHashCode().ToString());	//큰데이터 형태
			insSend.Data_List.Add(Path.GetFileName(sDir));					//받는쪽에서 저장할 파일이름
			insSend.Data_List.Add(insLarge.TotalLength.ToString());			//파일 크기
			insSend.Data_List.Add(insLarge.CutCount.ToString());			//파일 자른 갯수
			insSend.Data_List.Add(this.m_nFileCount.ToString());			//인덱스

			return insSend;
		}

		public void SetDBIndex(int nDBIndex, int nIndex)
		{
			//인덱스를 가지고 정보의 주인을 찾는다.
			claLargeData_SendData insLD = this.m_listLD_SendData.Find(x => x.Index == nIndex);

			//디비인덱스를 저장한다.
			insLD.DBIndex = nDBIndex;

			//저장되어있는 데이터를 자른다.
			insLD.CutLargeData();
			
		}

		/// <summary>
		/// 지정한 인덱스의 데이터를 리턴해줍니다.
		/// </summary>
		/// <param name="nDBIndex"></param>
		/// <param name="nSendIndex"></param>
		/// <returns></returns>
		public dataSend Send(int nDBIndex, int nSendIndex)
		{
			/* 큰데이터를 순차적으로 보내기위한 목적으로 사용됩니다.
			 * 이렇게 한개를 보내고 받는쪽으로 부터 'LargeData_Receive_Complete'을 받은 후
			 * 다음 데이터를 전송하게 되면 왠만해서는 데이터가 꼬이지 않습니다.
			 * 단지 속도가 느리고 네트워크사용량이 초큼 늘어난다는 단점이... ㅡ.-;;;
			 */

			//디비인덱스를 가지고 정보의 주인을 찾는다.
			claLargeData_SendData insLD = this.m_listLD_SendData.Find(x => x.DBIndex == nDBIndex);

			if (nSendIndex >= insLD.ListSendData.Count)
			{	//인덱스가 데이터 범위가 아니다.
				return null;
			}
			else
			{	//인덱스가 데이터의 범위 안이다.
				//지정한 인덱스의 데이터를 리턴해줍니다.
				return insLD.ListSendData[nSendIndex];
			}
		}

		/// <summary>
		/// 생성한 리스트를 리턴합니다.
		/// 사용자가 집적 리스트를 관리하려면 이 함수를 통해 리스트를 받아 전송 합니다.
		/// </summary>
		/// <param name="nDBIndex"></param>
		/// <returns></returns>
		public List<dataSend> SendList(int nDBIndex)
		{
			/* 리스트를 직접 전송할때 주의 사항
			 * 반복문을 사용하여 리스트를 보낼경우 받는쪽에서 받기가 완료되지 않은 상태에서
			 * 데이터를 보낼경우 (타이밍이 안좋다면)데이터가 꼬이게 되서 디스커낵트가 된다.
			 * 이런현상으 막기위한 가장 확실한 방법은 받는 쪽에서 
			 * 'LargeData_Receive_Complete'를 날리는 것을 확인하고 데이터를 보내는 것이다.
			 * 'this.Send(int, int)'이럴때 사용하는 함수고 이 함수는 이런 행동들을 직접 관리할때 사용한다.
			 */
			return this.m_listLD_SendData.Find(x => x.DBIndex == nDBIndex).ListSendData;
		}

	}
}
