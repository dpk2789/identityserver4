using System;

namespace Aow.Domain
{
    public class UserCompanies
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public string ApplicationUserId { get; set; }
       
    }
}
