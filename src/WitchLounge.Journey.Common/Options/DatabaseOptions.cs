using System.ComponentModel.DataAnnotations;

namespace WitchLounge.Journey.Common;

public sealed class DatabaseOptions
{
    public const string Key = "Database";

    [Required(ErrorMessage = "ConnectionString is required")]
    public required string ConnectionString { get; set; }
}