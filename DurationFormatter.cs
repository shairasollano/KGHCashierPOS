namespace KGHCashierPOS
{
    public static class DurationFormatter
    {
        public static string Format(int totalMinutes)
        {
            if (totalMinutes < 60)
            {
                return $"{totalMinutes} min";
            }
            else
            {
                int hours = totalMinutes / 60;
                int minutes = totalMinutes % 60;

                if (minutes == 0)
                    return $"{hours} hr";
                else
                    return $"{hours} hr {minutes} min";
            }
        }

        public static int Parse(string durationText)
        {
            int totalMinutes = 0;

            if (durationText.Contains("hr"))
            {
                int hrIndex = durationText.IndexOf("hr");
                string hourPart = durationText.Substring(0, hrIndex).Trim();

                if (int.TryParse(hourPart, out int hours))
                {
                    totalMinutes += hours * 60;
                }

                if (durationText.Contains("min"))
                {
                    string afterHr = durationText.Substring(hrIndex + 2);
                    string minPart = afterHr.Replace("min", "").Trim();

                    if (int.TryParse(minPart, out int minutes))
                    {
                        totalMinutes += minutes;
                    }
                }
            }
            else if (durationText.Contains("min"))
            {
                string minPart = durationText.Replace("min", "").Trim();
                if (int.TryParse(minPart, out int minutes))
                {
                    totalMinutes = minutes;
                }
            }

            return totalMinutes;
        }
    }
}