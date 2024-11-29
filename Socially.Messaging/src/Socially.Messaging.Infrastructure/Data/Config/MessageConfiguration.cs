using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socially.Messaging.Core.MessageAggregate;

namespace Socially.Messaging.Infrastructure.Data.Config;
public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
  public void Configure(EntityTypeBuilder<Message> builder)
  {
    builder.Property(m => m.Content)
        .HasMaxLength(DataSchemaConstants.DEFAULT_CONTENT_LENGTH)
        .IsRequired();

    builder.HasOne<User>()
        .WithMany() 
        .HasForeignKey(m => m.ReceiverId)
        .OnDelete(DeleteBehavior.Restrict); 


    builder.HasOne<User>()
        .WithMany()
        .HasForeignKey(m => m.SenderId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.Property(m => m.Status)
        .HasConversion(
            status => status.ToString(),
            value => Enum.Parse<MessageStatus>(value));
  }
}
