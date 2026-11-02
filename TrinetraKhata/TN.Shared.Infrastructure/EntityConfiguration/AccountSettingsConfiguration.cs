using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;

namespace TN.Shared.Infrastructure.EntityConfiguration
{
    public class AccountSettingsConfiguration : IEntityTypeConfiguration<AccountSettings>
    {
        public void Configure(EntityTypeBuilder<AccountSettings> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}
