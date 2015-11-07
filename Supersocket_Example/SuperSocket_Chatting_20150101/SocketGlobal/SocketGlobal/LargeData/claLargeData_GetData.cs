using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketGlobal.LargeData
{
	/// <summary>
	/// 큰데이터 분류
	/// </summary>
	public enum typeLargeData
	{
		/// <summary>
		/// 큰 메시지
		/// </summary>
		BigMessage = 0,
		/// <summary>
		/// 파일
		/// </summary>
		File,
	}


	/// <summary>
	/// 큰 데이터를 받기위한 클래스
	/// </summary>
	public class claLargeData_GetData
	{
		/// <summary>
		/// 디비에서 넘어온 인덱스
		/// </summary>
		public int DBIndex { get; set; }
		/// <summary>
		/// 받은 데이터
		/// </summary>
		public byte[] Data { get; set; }

		/// <summary>
		/// 지금까지 받은 데이터의 크기
		/// </summary>
		public int Length_Now { get; set; }

		/// <summary>
		/// 받아야될 전체 데이터 크기
		/// </summary>
		public int Length_Total { get; set; }

		/// <summary>
		/// 지금까지 받은 데이터의 갯수
		/// </summary>
		public int Count_Now { get; set; }
		/// <summary>
		/// 받아야할 전체 데이터의 갯수
		/// </summary>
		public int Count_Total { get; set; }

		/// <summary>
		/// 큰데이터 데이터 분류
		/// </summary>
		public typeLargeData TypeLargeData { get; set; }

		/// <summary>
		/// 저장할 파일명
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// 저장할 경로
		/// </summary>
		public string Dir { get; set; }

		/// <summary>
		/// 파일업로드가 끝나고나서 완료처리를 했는지 여부.
		/// 중복 완료처리를 막기위해 사용한다.
		/// </summary>
		public bool DataCompleteRead { get; set; }

		/// <summary>
		/// 이 클래스를 통해 받을 데이터를 세팅한다.
		/// </summary>
		/// <param name="typeLD"></param>
		/// <param name="nDBIndex"></param>
		/// <param name="nTotalLength"></param>
		/// <param name="nTotalCount"></param>
		public void SettingData(typeLargeData typeLD
								, string sFileName
								, int nTotalLength
								, int nTotalCount
								, int nDBIndex)
		{
			this.TypeLargeData = typeLD;
			this.FileName = sFileName;

			this.Length_Total = nTotalLength;
			this.Length_Now = 0;
			//데이터 공간을 잡아둔다.
			this.Data = new byte[this.Length_Total];

			this.Count_Total = nTotalCount;
			this.Count_Now = 0;
			this.DataCompleteRead = false;

			this.DBIndex = nDBIndex;
		}


	}
}
