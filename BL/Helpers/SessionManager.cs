using BO;

namespace PL.Helpers
{
    /// <summary>
    /// Manages the current user session within the presentation layer,
    /// including identification and authorization role.
    /// </summary>
    public static class SessionManager
    {
        /// <summary>
        /// Stores the identifier of the currently logged-in user.
        /// </summary>
        public static string UserId { get; set; } = "";

        /// <summary>
        /// Stores the role of the currently logged-in user.
        /// </summary>
        public static UserRole Role { get; set; }
    }
}
