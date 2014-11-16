namespace Mailgun.Extensions
{
    /// <summary>
    /// Extensions to booleans
    /// </summary>
    public static class BooleanExtensions
    {
        /// <summary>
        /// Get a boolean value as a string in the "yes"/"no" format
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string AsYesNo(this bool val)
        {
            return val ? "yes" : "no";
        }
    }
}