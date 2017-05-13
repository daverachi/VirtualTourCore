using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualTourCore.Core.Models.ModelMaps
{
    public class SecurityUserMap : EntityTypeConfiguration<SecurityUser>
    {
        public SecurityUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("SecurityUser");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.RegistrationCodeId).HasColumnName("RegistrationCodeId");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.PasswordHash).HasColumnName("PasswordHash");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.UserName).HasColumnName("Username");
            this.Property(t => t.Admin).HasColumnName("IsSystemAdmin");
        }
    }
}
