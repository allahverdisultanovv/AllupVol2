namespace AllupVol2.Utilities.Extensions
{
    public static class StringHelper
    {
        public static string Capitalize(this string word)
        {
            word=word.Substring(0, 1).ToUpper()+word.Substring(1,word.Length).ToLower();
            return word;
        }
    }
}
