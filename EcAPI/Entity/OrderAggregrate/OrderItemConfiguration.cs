using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcAPI.Entity.OrderAggregrate
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(i => i.ItmeOrdered, io =>
            {
                io.WithOwner();
            });
            builder.Property(i=>i.Price)
            .HasColumnType("decimal(18,2)");
        }
    }
}