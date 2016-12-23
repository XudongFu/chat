using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsFormsApplication2.data
{
    class praseTxt:IDisposable
    {

        FileStream fileStream;
        bool reach;
        long length = 0;
        public praseTxt(string address)
        {
            fileStream = File.Open(address, FileMode.Open, FileAccess.Read);
            length = fileStream.Length;
            fileStream.Position = 0;
            reach = false;
        }


        public void close()
        {
            if (fileStream.CanRead)
                fileStream.Close();
        }

        public List<string> getNextLine()
        {
            List<string> temp = new List<string>();
            StringBuilder str = new StringBuilder(50);
            while (!EOF())
            {
                int shuzi = fileStream.ReadByte();
                if (shuzi != 9 && shuzi != 10)
                {
                    str.Append((char)shuzi, 1);
                }
                else if (shuzi == 9)
                {
                    temp.Add(str.ToString());
                    str.Clear();
                }
                else
                {
                    temp.Add(str.ToString());
                    return temp;
                }
            }
            if (!reach)
            {
                temp.Add(str.ToString());
                reach = true;
                return temp;
            }
            else
                throw new Exception("到达文件尾");
        }

        bool EOF()
        {
            if (p.Position == length - 1)
                return true;
            else
                return false;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    fileStream.Close();

                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~praseTxt() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion



    }
}
