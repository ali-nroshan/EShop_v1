namespace EShop.Core.Common
{
    public static class GeneralTools
    {
        public static string DecimalToString(this decimal input)
        {
            return string.Format("{0:#,0}", input);
        }

        public static string GregorianToPersianDate(this DateTime date)
        {
            var pc = new System.Globalization.PersianCalendar();
            return pc.GetYear(date).ToString("0000/") + pc.GetMonth(date).ToString("00/") + pc.GetDayOfMonth(date).ToString("00");
        }
        public static float ByteToMB(this long fileSize) => fileSize / 1024f / 1024f;

    }
}
