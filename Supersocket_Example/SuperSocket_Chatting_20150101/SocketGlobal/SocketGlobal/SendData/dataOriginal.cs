using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketGlobal.SendData
{
	public struct dataOriginal
	{
		/// <summary>
		/// 세팅된 데이터 크기
		/// </summary>
		public int Length
		{
			get
			{
				return this.m_nLength;
			}
			set
			{
				//데이터 길이를 저장하고
				this.m_nLength = value;
				//데이터 길이를 세팅한다
				this.m_byteData = new byte[this.m_nLength];
			}
		}
		/// <summary>
		/// 세팅된 데이터 크기(원본)
		/// </summary>
		private int m_nLength;

		/// <summary>
		/// 데이터
		/// </summary>
		public byte[] Data
		{
			get
			{
				return this.m_byteData;
			}
			set
			{
				m_byteData = value;
				//데이터 저장
				this.m_nLength = m_byteData.Length;
			}
		}
		/// <summary>
		/// 데이터(원본)
		/// </summary>
		private byte[] m_byteData;
	}
}
