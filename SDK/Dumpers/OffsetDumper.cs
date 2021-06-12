using System.Collections.Generic;
using System.Net;

namespace Wolf_Hack.SDK.Dumpers
{
    public unsafe class OffsetDumper
    {
        private static Dictionary<string, int> m_OffsetsDictionary;
        private const string UrlOffset = "https://raw.githubusercontent.com/frk1/hazedumper/master/csgo.toml";

        /// <summary>
        /// Чтение смещений
        /// </summary>
        /// <param name="Url">Адрес скачивания смещений</param>
        /// <returns></returns>
        public static Dictionary<string, int> GetOffset()
        {
            if (m_OffsetsDictionary == null)
            {
                m_OffsetsDictionary = new Dictionary<string, int>();

                string GetValueUrl = new WebClient().DownloadString(UrlOffset);

                foreach (var Value in GetValueUrl.Split('\n'))
                {
                    try
                    {
                        m_OffsetsDictionary.Add(Value.Split('=')[0].TrimEnd(' '), int.Parse(Value.Split('=')[1].TrimStart(' ')));
                    }
                    catch
                    {
                        continue;
                    }
                }

                return m_OffsetsDictionary;
            }
            else
            {
                return m_OffsetsDictionary;
            }
        }
    }
}