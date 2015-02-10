using System.Collections.Generic;

namespace KendoUI07.Models
{
    /// <summary>
    /// منبع داده فرضی جهت سهولت دموی برنامه
    /// </summary>
    public static class RegistrationsDataSource
    {
        private static readonly IList<Registration> _cachedItems;
        static RegistrationsDataSource()
        {
            _cachedItems = createRegistrationsDataSource();
        }

        public static IList<Registration> LatestRegistrations
        {
            get { return _cachedItems; }
        }

        /// <summary>
        /// هدف صرفا تهیه یک منبع داده آزمایشی ساده تشکیل شده در حافظه است
        /// </summary>
        private static IList<Registration> createRegistrationsDataSource()
        {
            return new List<Registration>
            {
                new Registration
                {
                    Id = 1,
                    CourseName = "C++",
                    Credit = 1000,
                    Email = "tst1@site.com",
                    Tel = "12345678",
                    UserName = "UserX"
                },
                new Registration
                {
                    Id = 2,
                    CourseName = "C#",
                    Credit = 2000,
                    Email = "tst2@site.com",
                    Tel = "12345678",
                    UserName = "UserY"
                },
                new Registration
                {
                    Id = 3,
                    CourseName = "Java",
                    Credit = 1000,
                    Email = "tst3@site.com",
                    Tel = "12345678",
                    UserName = "UserZ"
                },
            };
        }
    }
}