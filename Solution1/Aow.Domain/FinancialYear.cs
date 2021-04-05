using AowCore.Domain.Common;
using System;

namespace Aow.Domain
{
    public class FinancialYear : AuditableEntity<Guid>
    {     
        public string Name { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }     
        public bool IsActive { get; set; }
        public bool? IsLocked { get; set; }       
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
